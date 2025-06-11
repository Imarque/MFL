using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Muthoot.Classes;
using Muthoot.Helper;
using Muthoot.Models;
using System.Data;
using System.Web.WebPages;

namespace Muthoot.Controllers
{
    public class ReportController : Controller
    {
        private readonly Utilities _uti;
        private readonly General _gen;
        private readonly UserInfo _userDeatils;
        private readonly SessionHandler _session;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        private const string Process = "Muthoot";
        private const string ControllerName = "Reports";
        public ReportController(Utilities uti, General gen, SessionHandler session, IHttpContextAccessor contextAccessor, IConfiguration configuration, IWebHostEnvironment env)
        {
            _uti = uti;
            _gen = gen;
            _session = session;
            _contextAccessor=contextAccessor;
            _configuration = configuration;
            _env = env;
        }
        public IActionResult Reports()
        {
            UserInfo usersdeatils = new UserInfo();
            _session.ValidateSession(ref usersdeatils);
            var session = HttpContext.Session;
            var userDetailsJson = session.GetString("UserDetails");

            if (TempData["Message"]!=null)
            {
                ViewBag.Message = TempData["Message"];
            }
            if (TempData["ErrorMessage"]!=null)
            {
                ViewBag.Error = TempData["ErrorMessage"];
            }
            _uti.WriteToFile(Process, ControllerName, "Reports PageLoad", "Start");
            Reports model = new Reports();
            try
            {
                model.ReportList=GetDropdown_ReportType();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                _uti.WriteToFile(Process, ControllerName, "Reports PageLoad", ex.Message.ToString());
            }
            return View(model);
        }
        public List<SelectListItem> GetDropdown_ReportType()
        {
            List<SelectListItem> reportType = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();

                dt = _gen.GetUIElements("ReportType", "", "", "", "");

                foreach (DataRow row in dt.Rows)
                {
                    reportType.Add(new SelectListItem { Value = row["ID"].ToString(), Text = row["Name"].ToString() });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
                _uti.WriteToFile(Process, ControllerName, "Reports Dropdown", ex.Message.ToString());
            }
            return reportType;

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reports(Reports model)
        {
            UserInfo usersdeatils = new UserInfo();
            _session.ValidateSession(ref usersdeatils);
            var session = HttpContext.Session;
            var userDetailsJson = session.GetString("UserDetails");

            if (model == null)
                model = new Reports();

            model.ReportList=GetDropdown_ReportType();
            var selectedText = model.ReportList.FirstOrDefault(x => x.Value == model.Report?.ToString())?.Text.Replace(" ", "_");
            string selectedType = model.Report;
            DateTime startDate = model.FromDate;
            DateTime endDate = model.ToDate.AddDays(1);
            DateTime date = DateTime.Now;
            string todayDate = date.ToString("ddMMyyyyHHmmss");
            try
            {
                DataTable downloaddt = new DataTable();
                DataTable dt = _gen.Get_Reports(selectedType, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                if (dt != null && dt.Rows.Count > 0)
                {
                    dt.Columns.Add("S.No", typeof(int)).SetOrdinal(0);
                    int counter = 1;
                    foreach (DataRow row in dt.Rows)
                    {
                        row["S.No"] = counter++;
                    }
                    bool hasPan = dt.Columns.Contains("Pan");
                    bool hasMobile = dt.Columns.Contains("MobileNumber");

                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            if (hasPan && row["Pan"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["Pan"].ToString()))
                            {
                                row["Pan"] = Utilities.Decrypt(row["Pan"].ToString());
                            }

                            if (hasMobile && row["MobileNumber"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["MobileNumber"].ToString()))
                            {
                                row["MobileNumber"] = Utilities.Decrypt(row["MobileNumber"].ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Decryption failed for a row: " + ex.Message);
                            continue;
                        }
                    }

                    downloaddt = dt;
                    TempData["Message"] = "Data Found!";
                }
                else
                {
                    TempData["Message"] = "No data found for export.";
                    return RedirectToAction("Reports");
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(downloaddt, "Report");

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(),
                                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                    $"{selectedText}_{todayDate}Report.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]=ex.Message;
                _uti.WriteToFile(Process, ControllerName, "Reports Export", ex.Message.ToString());
            }
            return View(model);
        }
        [HttpGet]
        public JsonResult ShowReport_List(string StartDate, string EndDate, string ReportType)
        {
            Reports _mod = new Reports();
            _mod.FromDate =Convert.ToDateTime(StartDate);
            _mod.ToDate =Convert.ToDateTime(EndDate);
            DateTime fromdate = _mod.FromDate;
            DateTime todate = _mod.ToDate.AddDays(1);
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            int serialNumber = 1;

            try
            {
                DataTable dt = _gen.Get_Reports(ReportType, fromdate.ToString("yyyy-MM-dd"), todate.ToString("yyyy-MM-dd"));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();
                        row.Add("SNo", serialNumber++);

                        foreach (DataColumn col in dt.Columns)
                        {
                            string columnName = col.ColumnName;
                            object value = dr[col];
                            object finalValue = value;

                            if (value != DBNull.Value)
                            {
                                if (columnName.Equals("MobileNumber", StringComparison.OrdinalIgnoreCase) && value is string mobileEncrypted)
                                {
                                    finalValue = Utilities.Decrypt(mobileEncrypted);
                                }
                                else if (columnName.Equals("Pan", StringComparison.OrdinalIgnoreCase) && value is string panEncrypted)
                                {
                                    finalValue = Utilities.Decrypt(panEncrypted);
                                }
                                else if (value is DateTime dateValue)
                                {
                                    finalValue = dateValue.ToString("dd-MM-yyyy HH:mm:ss");
                                }
                            }
                            else
                            {
                                finalValue = "";
                            }

                            row.Add(columnName, finalValue);
                        }

                        rows.Add(row);
                    }

                    return Json(new
                    {
                        status = "success",
                        msg = "Data Found",
                        reportRows = rows,
                        recordsCount = rows.Count
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = "error",
                        msg = "No Data Found",
                        reportRows = new List<Dictionary<string, object>>(),
                        RecordsCount = 0
                    });
                }
            }
            catch (Exception ex)
            {
                _uti.WriteToFile(Process, ControllerName, "getHistory_List", ex.Message);
                return Json(new
                {
                    status = "error",
                    msg = "Exception occurred: " + ex.Message,
                    reportRows = new List<Dictionary<string, object>>(),
                    RecordsCount = 0
                });
            }
        }
    }
}
