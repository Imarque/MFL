using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Muthoot.Classes;
using Muthoot.Helper;
using Muthoot.Models;
using Nancy.Session;
using System.Data;
using System.Globalization;
using System.Security.Cryptography.Xml;

namespace Muthoot.Controllers
{
    public class UploadController : Controller
    {
        private readonly Utilities _uti;
        private readonly General _gen;
        private readonly SessionHandler _session;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        private const string Process = "Muthoot";
        private const string ControllerName = "UserManager";
        public UploadController(Utilities uti, General gen, SessionHandler session, IHttpContextAccessor contextAccessor, IConfiguration configuration, IWebHostEnvironment env)
        {
            _uti = uti;
            _gen = gen;
            _session = session;
            _contextAccessor=contextAccessor;
            _configuration = configuration;
            _env = env;
        }
        public IActionResult Upload(Upload upload)
        {
            try
            {
                _uti.WriteToFile(Process, ControllerName, "Upload page open", "Start");
                if (TempData["Message"]!=null)
                {
                    ViewBag.Message = TempData["Message"]; ;
                }
                if (TempData["ErrorMessage"]!=null)
                {
                    ViewBag.Error = TempData["ErrorMessage"]; ;
                }
                UserInfo usersdeatils = new UserInfo();
                _session.ValidateSession(ref usersdeatils);
                var session = HttpContext.Session;
                var userDetailsJson = session.GetString("UserDetails");
                var privilege = usersdeatils.privilege;
                if (privilege>4)
                {
                    session.Clear();
                    return RedirectToAction("Login", "Login");
                }
                ViewBag.Type = GetDropdownType();
                upload.UploadHistory = GetUploadHistory();
                _uti.WriteToFile(Process, ControllerName, "Upload page open", "END");
                
            }
            catch (Exception ex)
            {
                _uti.WriteToFile(Process, ControllerName, "Upload page open", ex.Message.ToString());
                TempData["ErrorMessage"]=ex.Message.ToString();
            }
            return View(upload);


        }
        public List<SelectListItem> GetDropdownType()
        {
            List<SelectListItem> uploadType = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();

                dt = _gen.GetUIElements("uploadType", "", "", "", "");

                foreach (DataRow row in dt.Rows)
                {
                    uploadType.Add(new SelectListItem { Value = row["ID"].ToString(), Text = row["Name"].ToString() });
                }
            }
            catch (Exception ex)
            {
                _uti.WriteToFile(Process, ControllerName, "GetDropdownType", ex.Message.ToString());
                TempData["ErrorMessage"]=ex.Message.ToString();
            }
            return uploadType;

        }
        public void EncryptedData(string inputCsvPath, string outputCsvPath)
        {
            _uti.WriteToFile(Process, ControllerName, "EncryptedData", "Start");
            try
            {

                using (var reader = new StreamReader(inputCsvPath))
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                using (var writer = new StreamWriter(outputCsvPath))
                using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<dynamic>();
                    bool wroteHeader = false;

                    foreach (IDictionary<string, object> record in records)
                    {
                        var dict = new Dictionary<string, object>();

                        foreach (var kv in record)
                        {
                            string key = kv.Key;
                            string value = kv.Value?.ToString() ?? "";
                            if (value.Length>0)
                            {
                                if (key.Equals("MobileNumber", StringComparison.OrdinalIgnoreCase) ||
                                    key.Equals("Pan", StringComparison.OrdinalIgnoreCase))
                                {
                                    value = Utilities.Encrypt(value);
                                }
                            }

                            dict[key] = value;
                        }

                        if (!wroteHeader)
                        {
                            foreach (var k in dict.Keys)
                                csvWriter.WriteField(k);
                            csvWriter.NextRecord();
                            wroteHeader = true;
                        }

                        foreach (var v in dict.Values)
                            csvWriter.WriteField(v);
                        csvWriter.NextRecord();
                    }
                }
                _uti.WriteToFile(Process, ControllerName, "EncryptedData", "End");
            }
            catch (Exception ex)
            {
                _uti.WriteToFile(Process, ControllerName, "EncryptedData", ex.Message.ToString());
                TempData["ErrorMessage"]=ex.Message.ToString();
            }
        }
        [HttpPost]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public IActionResult Uploadcsv(IFormFile csvfile, string UploadType)
        {
            _uti.WriteToFile(Process, ControllerName, "CSVUPload", "Start");
            try
            {

                UserInfo usersdeatils = new UserInfo();
                _session.ValidateSession(ref usersdeatils);
                var session = HttpContext.Session;
                var userDetailsJson = session.GetString("UserDetails");
                var privilege = usersdeatils.privilege;
                if (privilege>4)
                {
                    session.Clear();
                    return RedirectToAction("Login", "Login");
                }

                if (string.IsNullOrEmpty(UploadType))
                {
                    TempData["ErrorMessage"] = "Please select an upload type.";
                    ViewBag.Type = GetDropdownType();
                    return View("Upload");
                }

                if (csvfile == null || csvfile.Length == 0)
                {
                    TempData["ErrorMessage"] = "Please upload a CSV file.";
                    ViewBag.Type = GetDropdownType();
                    return View("Upload");
                }

                if (Path.GetExtension(csvfile.FileName).ToLower() != ".csv")
                {
                    TempData["ErrorMessage"] = "Only CSV files are allowed.";
                    ViewBag.Type = GetDropdownType();
                    return View("Upload");
                }

                if (Convert.ToInt32(UploadType) == 1)
                {
                    if (!csvfile.FileName.Equals("base_upload.csv", StringComparison.OrdinalIgnoreCase))
                    {
                        TempData["ErrorMessage"] = "Invalid file name! Only 'base_upload.csv' is allowed.";
                        ViewBag.Type = GetDropdownType();
                        return View("Upload");
                    }


                    //string inputPath = _env.MapPath("~/Input/" + csvfile.FileName);
                    string inputPath = Path.Combine(_env.WebRootPath, "Input" ,csvfile.FileName);
                    
                    //string EncryptedPath = Server.MapPath("~/Uploads/" ,csvfile.FileName);
                    string EncryptedPath = Path.Combine(_env.WebRootPath, "Uploads", csvfile.FileName);


                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "Input"));
                    Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "Uploads"));

                    using (var inputStream = new FileStream(inputPath, FileMode.OpenOrCreate))
                    {
                        csvfile.CopyTo(inputStream);
                    }
                    using (var encryptedStream = new FileStream(EncryptedPath, FileMode.OpenOrCreate))
                    {
                        csvfile.CopyTo(encryptedStream);
                    }

                    EncryptedData(inputPath, EncryptedPath);
                    _uti.WriteToFile(Process, ControllerName, "CSVUPload", "CSV Encrypted Successfully");

                    if (CheckFile("base_upload.csv") > 0)
                    {
                        string userName = usersdeatils.userName;
                        string empCode = usersdeatils.userId;
                        string baseType = UploadType;

                        DataTable dt = _gen.BaseUpload(userName, empCode, baseType);
                        if (dt.Rows[0]["MSG"].ToString().Equals("INSERTED")&& Convert.ToInt32(dt.Rows[0]["BASECOUNT"])>0)
                        {
                            TempData["Message"] = "Base Uploaded Successfully!";
                            _uti.WriteToFile(Process, ControllerName, "CSVUPload", "base uploaded successfully in DB");
                        }
                        else if (dt.Rows[0]["MSG"].ToString().Equals("INSERTED")&& Convert.ToInt32(dt.Rows[0]["BASECOUNT"])<1)
                        {
                            TempData["ErrorMessage"] = "Base Not Uploaded!";
                            _uti.WriteToFile(Process, ControllerName, "CSVUPload", "base uploaded failed in DB");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Failed To Upload The Base!";
                            _uti.WriteToFile(Process, ControllerName, "CSVUPload", "base uploaded failed in DB");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _uti.WriteToFile(Process, ControllerName, "CSVUPload", ex.Message.ToString());
                TempData["ErrorMessage"]=ex.Message.ToString();
            }
            ViewBag.Type = GetDropdownType();
            Upload model = new Upload();
            model.UploadHistory = GetUploadHistory();
            return RedirectToAction("Upload");
        }
        public int CheckFile(string fileName)
        {
            // Construct the absolute path to the "Uploads" folder
            string uploadPath = Path.Combine(_env.WebRootPath, "Uploads");
            string filePath = Path.Combine(uploadPath, fileName);

            if (System.IO.File.Exists(filePath))
                return 1;
            else
                return 0;
        }
        public List<UploadHistory> GetUploadHistory()
        {
            List<UploadHistory> history = new List<UploadHistory>();
            try
            {
                DataTable dt = new DataTable();
                dt = _gen.GetUIElements("UploadHistory", "", "", "", "");

                if (dt.Rows.Count>0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        UploadHistory list = new UploadHistory
                        {
                            BaseType = row["BaseType"].ToString(),
                            EmpolyeeCode = row["EmpolyeeCode"].ToString(),
                            UploadedBy = row["UploadedBy"].ToString(),
                            UploadedCount = row["UploadedCount"].ToString(),
                            UploadedTime = row["UploadedTime"].ToString()
                        };
                        history.Add(list);
                    }
                }
            }
            catch (Exception ex)
            {
                _uti.WriteToFile(Process, ControllerName, "GetUploadHistory", ex.Message.ToString());
                TempData["ErrorMessage"]=ex.Message.ToString();
            }
            return history;
        }
        public IActionResult DownloadSampleCsv()
        {
            string fileName = "base_upload.csv";
            string filePath = Path.Combine(_env.WebRootPath, "Sample", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                _uti.WriteToFile(Process, ControllerName, "DownloadSampleCsv", ex.Message.ToString());
                TempData["ErrorMessage"]=ex.Message.ToString();
                return StatusCode(500, "Internal server error while downloading file.");
            }
        }


    }
}
