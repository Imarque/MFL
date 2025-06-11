using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Muthoot.Classes;
using Muthoot.Controllers.Classes;
using System.Data;
using Muthoot.Models;
using Muthoot.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Runtime.Serialization.Json;
using DocumentFormat.OpenXml.Office.Word;
using System.Web;
using DocumentFormat.OpenXml.EMMA;
using System.Net;
using Newtonsoft.Json;

namespace Muthoot.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Utilities _uti;
    private readonly General _gen;
    private readonly UserInfo _userDeatils;
    private readonly SessionHandler _session;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    private const string Process = "Muthoot";
    private const string ControllerName = "Home";

    string userId;
    int privilege;
    int region;
    int userType;
    string privilegename;
    string campaignId;
    string crtObjectId;
    string userCrtObjectId;
    string dialer_customerId;
    string sessionId;
    string leadId;
    string lead_name;
    string dialer_callType;
    string crm_push_generated_time;
    string instanceId;
    string locale;
    string origin;
    string iframeId;

    public HomeController(Utilities uti, General gen, SessionHandler session, IHttpContextAccessor contextAccessor, IConfiguration configuration, IWebHostEnvironment env, ILogger<HomeController> logger)
    {
        _uti = uti;
        _gen = gen;
        _session = session;
        _contextAccessor=contextAccessor;
        _configuration = configuration;
        _env = env;
        _logger = logger;
    }
    public IActionResult Home(DashBoard _mod)
    {
        try
        {
            UserInfo usersdeatils = new UserInfo();
            _session.ValidateSession(ref usersdeatils);
            var session = HttpContext.Session;
            var userDetailsJson = session.GetString("UserDetails");
            string MonthCode = DateTime.Now.Month.ToString()+DateTime.Now.Year.ToString();

            DataSet ds = _gen.DashBoard("Calling", MonthCode, DateTime.Now.ToString("yyyy-MM-dd"));

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // MTD
                DataRow row1 = ds.Tables[0].Rows[0];
                _mod.OverallBaseCount = Convert.ToInt32(row1["BASECOUNT"]);
                _mod.BaseDailed = Convert.ToInt32(row1["Dialed"]);
                _mod.Connected = Convert.ToInt32(row1["Connected"]);
                _mod.NotConnected = Convert.ToInt32(row1["NotConnected"]);
                _mod.Pending = _mod.OverallBaseCount - (_mod.Connected + _mod.NotConnected);
                _mod.Disbursal = Convert.ToInt32(row1["Disbursal"]);

                _mod.BaseDailedPercentage = _mod.OverallBaseCount > 0
                    ? (_mod.BaseDailed * 100) / _mod.OverallBaseCount
                    : 0;
                _mod.ConnectedPercentage = _mod.BaseDailed > 0
                    ? (_mod.Connected * 100) / _mod.BaseDailed
                    : 0;
                _mod.NotConnectedPercentage = _mod.BaseDailed > 0
                    ? (_mod.NotConnected * 100) / _mod.BaseDailed
                    : 0;
                _mod.PendingPercentage = _mod.OverallBaseCount > 0
                    ? (_mod.Pending * 100) / _mod.OverallBaseCount
                    : 0;
                _mod.DisbursalPercentage = _mod.BaseDailed > 0
                    ? (_mod.Disbursal * 100) / _mod.BaseDailed
                    : 0;
            }

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                // FTD
                DataRow row2 = ds.Tables[1].Rows[0];
                _mod.FTDBaseDailed = Convert.ToInt32(row2["FTDDialed"]);
                _mod.FTDConnected = Convert.ToInt32(row2["FTDConnected"]);
                _mod.FTDNotConnected = Convert.ToInt32(row2["FTDNotConnected"]);


                _mod.FTDPending = _mod.OverallBaseCount - (_mod.FTDConnected + _mod.FTDNotConnected);

                _mod.FTDDisbursal = Convert.ToInt32(row2["FTDDisbursal"]);


                _mod.FTDBaseDailedPercentage = _mod.OverallBaseCount > 0
                    ? (_mod.FTDBaseDailed * 100) / _mod.OverallBaseCount
                    : 0;

                _mod.FTDConnectedPercentage = _mod.FTDBaseDailed > 0
                    ? (_mod.FTDConnected * 100) / _mod.FTDBaseDailed
                    : 0;

                _mod.FTDNotConnectedPercentage = _mod.FTDBaseDailed > 0
                    ? (_mod.FTDNotConnected * 100) / _mod.FTDBaseDailed
                    : 0;

                _mod.FTDPendingPercentage = _mod.OverallBaseCount > 0
                    ? (_mod.FTDPending * 100) / _mod.OverallBaseCount
                    : 0;

                _mod.FTDDisbursalPercentage = _mod.FTDBaseDailed > 0
                    ? (_mod.FTDDisbursal * 100) / _mod.FTDBaseDailed
                    : 0;

            }

        }
        catch (Exception ex)
        {
            _uti.WriteToFile(Process, ControllerName, "Home Iactionresult", ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }

        return View(_mod);
    }
    public JsonResult Get_DashBoards([FromBody] DashBoard _mod)
    {
        try
        {
            string dashBoard = _mod.DashboardKey;
            string monthCode = _mod.MonthCode;
            DateTime today = _mod.Today;

            DataSet ds = _gen.DashBoard(dashBoard, monthCode, today.ToString("yyyy-MM-dd"));

            if (dashBoard=="Calling")
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // MTD
                    DataRow row1 = ds.Tables[0].Rows[0];
                    _mod.OverallBaseCount = Convert.ToInt32(row1["BASECOUNT"]);
                    _mod.BaseDailed = Convert.ToInt32(row1["Dialed"]);
                    _mod.Connected = Convert.ToInt32(row1["Connected"]);
                    _mod.NotConnected = Convert.ToInt32(row1["NotConnected"]);
                    _mod.Pending = _mod.OverallBaseCount - (_mod.Connected + _mod.NotConnected);
                    _mod.Disbursal = Convert.ToInt32(row1["Disbursal"]);

                    _mod.BaseDailedPercentage = _mod.OverallBaseCount > 0
                        ? (_mod.BaseDailed * 100) / _mod.OverallBaseCount
                        : 0;
                    _mod.ConnectedPercentage = _mod.BaseDailed > 0
                        ? (_mod.Connected * 100) / _mod.BaseDailed
                        : 0;
                    _mod.NotConnectedPercentage = _mod.BaseDailed > 0
                        ? (_mod.NotConnected * 100) / _mod.BaseDailed
                        : 0;
                    _mod.PendingPercentage = _mod.OverallBaseCount > 0
                        ? (_mod.Pending * 100) / _mod.OverallBaseCount
                        : 0;
                    _mod.DisbursalPercentage = _mod.BaseDailed > 0
                        ? (_mod.Disbursal * 100) / _mod.BaseDailed
                        : 0;
                    //_mod.BaseDailedPercentage = Convert.ToInt32(row1["DialedPercent"]);
                    //_mod.ConnectedPercentage = Convert.ToInt32(row1["ConnectedPercent"]);
                    //_mod.NotConnectedPercentage = Convert.ToInt32(row1["NotConnectedPercent"]);
                    //_mod.PendingPercentage = Convert.ToInt32(row1["PendingPercent"]);
                    //_mod.DisbursalPercentage = Convert.ToInt32(row1["DisbursalPercent"]);
                }
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    // FTD
                    DataRow row2 = ds.Tables[1].Rows[0];
                    _mod.FTDBaseDailed = Convert.ToInt32(row2["FTDDialed"]);
                    _mod.FTDConnected = Convert.ToInt32(row2["FTDConnected"]);
                    _mod.FTDNotConnected = Convert.ToInt32(row2["FTDNotConnected"]);


                    _mod.FTDPending = _mod.OverallBaseCount - (_mod.FTDConnected + _mod.FTDNotConnected);

                    _mod.FTDDisbursal = Convert.ToInt32(row2["FTDDisbursal"]);


                    _mod.FTDBaseDailedPercentage = _mod.OverallBaseCount > 0
                        ? (_mod.FTDBaseDailed * 100) / _mod.OverallBaseCount
                        : 0;

                    _mod.FTDConnectedPercentage = _mod.FTDBaseDailed > 0
                        ? (_mod.FTDConnected * 100) / _mod.FTDBaseDailed
                        : 0;

                    _mod.FTDNotConnectedPercentage = _mod.FTDBaseDailed > 0
                        ? (_mod.FTDNotConnected * 100) / _mod.FTDBaseDailed
                        : 0;

                    _mod.FTDPendingPercentage = _mod.OverallBaseCount > 0
                        ? (_mod.FTDPending * 100) / _mod.OverallBaseCount
                        : 0;

                    _mod.FTDDisbursalPercentage = _mod.FTDBaseDailed > 0
                        ? (_mod.FTDDisbursal * 100) / _mod.FTDBaseDailed
                        : 0;
                    _mod.status="success";
                    _mod.msg="Data Found";
                    _mod.DashboardKey=dashBoard;

                }
                else
                {
                    _mod.status="error";
                    _mod.msg="No Data Found";
                }
            }
            else if (dashBoard=="CallBack")
            {
                if ((ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)&&(ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0))
                {
                    _mod.DashboardKey=dashBoard;
                    _mod.FollowupHead="Call Back Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["CallBackCount"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["CallBackPedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;

                    _mod.status="success";
                    _mod.msg="Data Found";
                }
                else
                {
                    _mod.status="error";
                    _mod.msg="No Data Found";
                }
            }
        }
        catch (Exception ex)
        {
            _uti.WriteToFile(Process, ControllerName, "Get_DashBoards", ex.Message.ToString());
            _mod.status = "error";
            _mod.msg = ex.Message.ToString();
            return Json(_mod);
        }
        return Json(_mod);


    }
    public JsonResult Get_FollowupDashBoards([FromBody] DashBoard _mod)
    {
        try
        {
            string followupKey = _mod.FollowupKey;
            string monthCode = _mod.MonthCode;
            DateTime today = _mod.Today;

            DataSet ds = _gen.DashBoard("CallBack", monthCode, today.ToString("yyyy-MM-dd"));

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                if(followupKey=="callbackfollowup")
                {
                    _mod.DashboardKey=followupKey;
                    _mod.FollowupHead="Call Back Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["CallBackCount"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["CallBackPedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;
                    _mod.status="success";
                    _mod.msg= _mod.FollowupHead+" Data Found";
                }
                else if (followupKey=="1")
                {
                    _mod.DashboardKey=followupKey;
                    _mod.FollowupHead="User Profile Qualified Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["StageCode1"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["Stage1PedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;
                    _mod.status="success";
                    _mod.msg= _mod.FollowupHead+" Data Found";
                }
                else if (followupKey=="2")
                {
                    _mod.DashboardKey=followupKey;
                    _mod.FollowupHead="Application Fresh Loan Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["StageCode2"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["Stage2PedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;
                    _mod.status="success";
                    _mod.msg= _mod.FollowupHead+" Data Found";
                }
                else if (followupKey=="3")
                {
                    _mod.DashboardKey=followupKey;
                    _mod.FollowupHead="Details Submitted Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["StageCode3"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["Stage3PedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;
                    _mod.status="success";
                    _mod.msg= _mod.FollowupHead+" Data Found";
                }
                else if (followupKey=="4")
                {
                    _mod.DashboardKey=followupKey;
                    _mod.FollowupHead="KYC Success Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["StageCode4"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["Stage4PedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;
                    _mod.status="success";
                    _mod.msg= _mod.FollowupHead+" Data Found";
                }
                else if (followupKey=="5")
                {
                    _mod.DashboardKey=followupKey;
                    _mod.FollowupHead="Forwarded to Credit-UR Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["StageCode5"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["Stage5PedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;
                    _mod.status="success";
                    _mod.msg= _mod.FollowupHead+" Data Found";
                }
                else if (followupKey=="6")
                {
                    _mod.DashboardKey=followupKey;
                    _mod.FollowupHead="Loan Approved Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["StageCode6"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["Stage6PedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;
                    _mod.status="success";
                    _mod.msg= _mod.FollowupHead+" Data Found";
                }
                else if (followupKey=="7")
                {
                    _mod.DashboardKey=followupKey;
                    _mod.FollowupHead="Bank Added Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["StageCode7"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["Stage7PedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;
                    _mod.status="success";
                    _mod.msg= _mod.FollowupHead+" Data Found";
                }
                else if (followupKey=="8")
                {
                    _mod.DashboardKey=followupKey;
                    _mod.FollowupHead="Sign Agreement Followup";
                    _mod.TotalFollowups = Convert.ToInt32(ds.Tables[0].Rows[0]["StageCode8"]);
                    _mod.PendingFollowups = Convert.ToInt32(ds.Tables[1].Rows[0]["Stage8PedingCount"]);
                    _mod.CompletedFollowups = _mod.TotalFollowups - _mod.PendingFollowups;
                    _mod.status="success";
                    _mod.msg= _mod.FollowupHead+" Data Found";
                }
                else
                {
                    _mod.status="error";
                    _mod.msg="No Data Found";
                }

            }
            else
            {
                _mod.status="error";
                _mod.msg="No Data Found";
            }
        }
        catch (Exception ex)
        {
            _uti.WriteToFile(Process, ControllerName, "Get_DashBoards", ex.Message.ToString());
            _mod.status = "error";
            _mod.msg = ex.Message.ToString();
            return Json(_mod);
        }
        return Json(_mod);
    }
    [HttpPost]
    public JsonResult AgentWise_FollowupPeingList(string MonthCode)
    {
        string monthCode = MonthCode;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        int serialNumber = 1;

        try
        {
            DataSet ds = _gen.DashBoard("CallBack", monthCode, "");
            if (ds.Tables.Count > 0 && ds.Tables[2].Rows.Count>0)
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();
                    row.Add("SNo", serialNumber++);

                    foreach (DataColumn col in ds.Tables[2].Columns)
                    {
                        string columnName = col.ColumnName;
                        object value = dr[col];
                        object finalValue = value;

                        if (value != DBNull.Value)
                        {
                            finalValue=value;
                        }
                        else
                        {
                            finalValue = "";
                        }

                        row.Add(columnName, finalValue);
                    }

                    rows.Add(row);
                }

            }

            return Json(rows);
        }
        catch (Exception ex)
        {
            _uti.WriteToFile(Process, ControllerName, "AgentWise_FollowupPeingList", ex.Message);
            throw;
        }
    }
    public IActionResult CRM()
    {
        var session = HttpContext.Session;
        try
        {
            CRM_Model _mod = new CRM_Model();
            DataTable dt = new DataTable();

            _uti.WriteToFile(Process, ControllerName, "QueryString", "=======");

            if (Request.Query["campaignId"].ToString()!="")
            {
                userId = Request.Query["userId"].ToString();
                privilege = 30;
                region = 1;
                userType = 1;
                privilegename = "User";

                UserInfo _userDeatils = new UserInfo();

                _userDeatils.userId=userId;
                _userDeatils.userName=userId;
                _userDeatils.privilege=privilege;
                _userDeatils.region=region;
                _userDeatils.isMobileDevice=false;
                _userDeatils.sessionId=Request.Query["sessionId"].ToString();
                _userDeatils.userType=userType.ToString();
                _userDeatils.privilegename=privilegename;

                HttpContext.Session.SetString("userId", _userDeatils.userId);
                HttpContext.Session.SetString("UserName", _userDeatils.userName);
                HttpContext.Session.SetString("privilege", _userDeatils.privilege.ToString());
                HttpContext.Session.SetString("region", _userDeatils.region.ToString());
                HttpContext.Session.SetString("sessionId", _userDeatils.sessionId);
                HttpContext.Session.SetString("userType", _userDeatils.userType);
                HttpContext.Session.SetString("UserDetails", _userDeatils.ToString());
                HttpContext.Session.SetString("PrivilegeName", _userDeatils.privilegename);
                _uti.WriteToFile(Process, ControllerName, "QueryString", "userId :" + userId + " - Step 1:  QueryString_Validation");
            }
            else if (session.GetString("userId")!=null)
            {
                string ses = session.GetString("userId");
                userId=session.GetString("EmpCode");
                privilege=Convert.ToInt32(session.GetString("privilege"));
                region=Convert.ToInt32(session.GetString("region"));
                privilegename=session.GetString("PrivilegeName");
                _uti.WriteToFile(Process, ControllerName, "QueryString", "userId :" + userId + " - Step 2:  Session_Validation");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
            _uti.WriteToFile(Process, ControllerName, "QueryString", "userId :" + userId + " - Step 3: After Validation");

            _mod.userId=userId.ToString();
            _mod.MainDispositionList = new List<SelectListItem>();
            _mod.SubDispositionList = new List<SelectListItem>();

            _uti.WriteToFile(Process, ControllerName, "QueryString", "userId :" + userId + " - Step 3: After Dispositionlist");

            _mod.MobileNumber=Request.Query["phone"].ToString();
            _mod.campaignId=Request.Query["campaignId"].ToString();
            _mod.crtObjectId=Request.Query["crtObjectId"].ToString();
            _mod.userCrtObjectId=Request.Query["userCrtObjectId"].ToString();
            _mod.customerId=Request.Query["customerId"].ToString();
            _mod.sessionId=Request.Query["sessionId"].ToString();
            _mod.leadId=Request.Query["leadId"].ToString();
            _mod.lead_name=Request.Query["lead_name"].ToString();
            _mod.dialer_callType=Request.Query["dialer_callType"].ToString();
            _mod.crm_push_generated_time=Request.Query["crm_push_generated_time"].ToString();
            _mod.instanceId=Request.Query["instanceId"].ToString();
            _mod.locale=Request.Query["locale"].ToString();
            _mod.origin=Request.Query["origin"].ToString();
            _mod.iframeId=Request.Query["iframeId"].ToString();


            string customerInfo = Request.Query["customerInfo"].ToString();
            if (!(customerInfo == null) && (customerInfo != ""))
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(customerInfo)))
                {
                    // Deserialization from JSON  
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(DeSerialization_CustInfo));
                    DeSerialization_CustInfo bsObj2 = (DeSerialization_CustInfo)deserializer.ReadObject(ms);
                    string Name = bsObj2.name;
                }
            }
            if (Request.Query["campaignId"].ToString()!="" && Request.Query["sessionId"].ToString()!="")
            {
                _mod.Dialer_Config=Request.Query["campaignId"].ToString()+"|"+Request.Query["sessionId"].ToString();
            }
            else
            {
                _mod.Dialer_Config="";
            }
            if (Request.Query["value"].ToString() !="")
            {
                _mod.MobileNumber = Utilities.Decrypt(Request.Query["value"].ToString());
            }
            if (!string.IsNullOrEmpty(_mod.MobileNumber))
            {
                _mod.SearchKey="MobileNo";
                _mod.SearchValue =_mod.MobileNumber;
            }
            return View(_mod);
        }
        catch (Exception ex)
        {
            _uti.WriteToFile(Process, ControllerName, "CRM Iactionresult", ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    [HttpPost]
    public JsonResult Get_CustomerDetails([FromBody] CRM_Model _mod)
    {
        try
        {
            string userId = _mod.userId;
            string searchBy = _mod.SearchKey;
            string value = Utilities.Encrypt(_mod.SearchValue);

            DataTable dt = _gen.Fetch_CustomerDetails(searchBy, value);
            _uti.WriteToFile(Process, ControllerName, "Get_CustomerDetails", "Customer Deatils Fetached From DB");
            if (dt.Rows.Count>0)
            {
                string EncryptedMobileNo = dt.Rows[0]["MobileNumber"].ToString();
                string MobileNumber = Utilities.Decrypt(dt.Rows[0]["MobileNumber"].ToString());
                string MaskedMobileNumber = MaskMobileNumber(MobileNumber);
                string Pan = Utilities.Decrypt(dt.Rows[0]["Pan"].ToString());
                string MaskedPan = MaskMobileNumber(Pan);

                _mod.Stage = dt.Rows[0]["StageCode"]?.ToString() ?? "";
                _mod.StageName = dt.Rows[0]["Stage"]?.ToString() ?? "";
                _mod.Product = dt.Rows[0]["Product"]?.ToString() ?? "";
                _mod.ProductCode = dt.Rows[0]["ParentCode"]?.ToString() ?? "N/A";
                _mod.ProductType = dt.Rows[0]["ProductType"]?.ToString() ?? "N/A";
                _mod.MobileNumber = MobileNumber;
                _mod.Pan = Pan;

                _mod.Id = dt.Rows[0]["Id"]?.ToString() ?? "N/A";
                _mod.MonthCode = dt.Rows[0]["MonthCode"]?.ToString() ?? "N/A";
                _mod.LoanDetails = $"ROI: {dt.Rows[0]["ROI"]?.ToString() ?? "N/A"}\n" +
                                   $"Tenure:{dt.Rows[0]["Tenure"]?.ToString() ?? "N/A"}\n" +
                                   $"ExpectedLoanAmount: {dt.Rows[0]["ExpectedLoanAmount"]?.ToString() ?? "N/A"}\n" +
                                   $"ProcessingFee: {dt.Rows[0]["ProcessingFee"]?.ToString() ?? "N/A"}\n"+
                                   $"ProcessingFeeType: {dt.Rows[0]["ProcessingFeeType"]?.ToString() ?? "N/A"}\n" +
                                   $"ExpiryAt: {dt.Rows[0]["ExpiryAt"]?.ToString() ?? "N/A"}\n" +
                                   $"RepaymentBankName: {dt.Rows[0]["RepaymentBankName"]?.ToString() ?? "N/A"}\n" +
                                   $"RepaymentBankAccountNumber: {dt.Rows[0]["RepaymentBankAccountNumber"]?.ToString() ?? "N/A"}\n " +
                                   $"RepaymentIfscCode: {dt.Rows[0]["RepaymentIfscCode"]?.ToString() ?? "N/A"}\n" +
                                   $"RepaymentIAccountType: {dt.Rows[0]["RepaymentIAccountType"]?.ToString() ?? "N/A"}\n" +
                                   $"UdayamNumber: {dt.Rows[0]["UdayamNumber"]?.ToString() ?? "N/A"}\n" +
                                   $"UCIC: {dt.Rows[0]["UCIC"]?.ToString() ?? "N/A"}\n" +
                                   $"maxAmount: {dt.Rows[0]["maxAmount"]?.ToString() ?? "N/A"}\n" +
                                   $"MinAmount: {dt.Rows[0]["MinAmount"]?.ToString() ?? "N/A"}\n" +
                                   $"MinTenure: {dt.Rows[0]["MinTenure"]?.ToString() ?? "N/A"}\n" +
                                   $"MaxTenure: {dt.Rows[0]["MaxTenure"]?.ToString() ?? "N/A"}\n" +
                                   $"MaxEmi: {dt.Rows[0]["MaxEmi"]?.ToString() ?? "N/A"}\n" +
                                   $"InstallmentProgramme: {dt.Rows[0]["InstallmentProgramme"]?.ToString() ?? "N/A"}\n" +
                                   $"CustBrnCode: {dt.Rows[0]["CustBrnCode"]?.ToString() ?? "N/A"}\n" +
                                   $"DisbursedLoanAmount: {dt.Rows[0]["DisbursedLoanAmount"]?.ToString() ?? "N/A"}\n" +
                                   $"LoanAccountNo: {dt.Rows[0]["LoanAccountNo"]?.ToString() ?? "N/A"}\n";

                _mod.CustomerDetails = $"CustomerName:{dt.Rows[0]["CustomerName"]?.ToString() ?? "N/A"}\n" +
                                       $"Pan:{MaskedPan}\n" +
                                       $"MobileNumber:{MaskedMobileNumber}\n" +
                                       $"DateOfBirth:{dt.Rows[0]["DateOfBirth"]?.ToString() ?? "N/A"}\n" +
                                       $"Occupation:{dt.Rows[0]["Occupation"]?.ToString() ?? "N/A"}\n" +
                                       $"Gender:{dt.Rows[0]["Gender"]?.ToString() ?? "N/A"}\n" +
                                       $"CustomerAddressLine1:{dt.Rows[0]["CustomerAddressLine1"]?.ToString() ?? "N/A"}\n" +
                                       $"CustomerAddressLine2:{dt.Rows[0]["CustomerAddressLine2"]?.ToString() ?? "N/A"}\n" +
                                       $"LandMark:{dt.Rows[0]["LandMark"]?.ToString() ?? "N/A"}\n" +
                                       $"CurrentAddressPincode:{dt.Rows[0]["CurrentAddressPincode"]?.ToString() ?? "N/A"}\n" +
                                       $"CurrrentAddressState:{dt.Rows[0]["CurrrentAddressState"]?.ToString() ?? "N/A"}\n" +
                                       $"TypeOfBusiness:{dt.Rows[0]["TypeOfBusiness"]?.ToString() ?? "N/A"}\n" +
                                       $"NatureOfBusiness:{dt.Rows[0]["NatureOfBusiness"]?.ToString() ?? "N/A"}\n" +
                                       $"Industry:{dt.Rows[0]["Industry"]?.ToString() ?? "N/A"}\n" +
                                       $"SubIndustry:{dt.Rows[0]["SubIndustry"]?.ToString() ?? "N/A"}\n" +
                                       $"BusinessName:{dt.Rows[0]["BusinessName"]?.ToString() ?? "N/A"}\n" +
                                       $"DateOfIncopration:{dt.Rows[0]["DateOfIncopration"]?.ToString() ?? "N/A"}\n" +
                                       $"BusinessAddrressLine1:{dt.Rows[0]["BusinessAddrressLine1"]?.ToString() ?? "N/A"}\n" +
                                       $"BusinessAddrressLine2:{dt.Rows[0]["BusinessAddrressLine2"]?.ToString() ?? "N/A"}\n" +
                                       $"BusinessAddrressLandmark:{dt.Rows[0]["BusinessAddrressLandmark"]?.ToString() ?? "N/A"}\n" +
                                       $"BusinessAddrressPincode:{dt.Rows[0]["BusinessAddrressPincode"]?.ToString() ?? "N/A"}\n" +
                                       $"BusinessAddrressState:{dt.Rows[0]["BusinessAddrressState"]?.ToString() ?? "N/A"}\n" +
                                       $"BusinessAddrressCity:{dt.Rows[0]["BusinessAddrressCity"]?.ToString() ?? "N/A"}\n" +
                                       $"Email:{dt.Rows[0]["Email"]?.ToString() ?? "N/A"}\n" +
                                       $"CustomerCif:{dt.Rows[0]["CustomerCif"]?.ToString() ?? "N/A"}\n" +
                                       $"PrimaryRegionalLanguage:{dt.Rows[0]["PrimaryRegionalLanguage"]?.ToString() ?? "N/A"}\n";
                _mod.userId=userId;

                if (searchBy == "MobileNo")
                {
                    _mod.MaskedValue = MaskedMobileNumber;
                }
                else if (searchBy == "PANNo")
                {
                    _mod.MaskedValue = MaskedPan;
                }
                if (!string.IsNullOrEmpty(_mod.Id))
                {
                    _mod.status = "success";
                    _mod.msg = "Data Found";
                }


            }
            else
            {
                _mod.status = "error";
                _mod.msg = "No Data Found";
            }
            _uti.WriteToFile(Process, ControllerName, "Get_CustomerDetails", "Customer Deatils Fetached From DB");
            return Json(_mod);
        }
        catch (Exception ex)
        {
            _uti.WriteToFile(Process, ControllerName, "Get_CustomerDetails", ex.Message.ToString());
            _mod.status = "error";
            _mod.msg = ex.Message.ToString();
            return Json(_mod);
        }
    }
    public string MaskMobileNumber(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length < 4)
            return value;

        string last4Digits = value.Substring(value.Length - 4);
        return "XXXXXX" + last4Digits;
    }
    [HttpPost]
    public JsonResult getHistory_List(string MobileNumber, string MonthCode)
    {
        string mobileNumber = Utilities.Encrypt(MobileNumber);
        string monthCode = MonthCode;
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        int serialNumber = 1;

        try
        {
            DataTable dt = _gen.GetCallingHistory(mobileNumber, monthCode);
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
                                string decrypted = Utilities.Decrypt(mobileEncrypted);
                                finalValue = MaskMobileNumber(decrypted);
                            }
                            else if (columnName.Equals("Pan", StringComparison.OrdinalIgnoreCase) && value is string panEncrypted)
                            {
                                string decrypted = Utilities.Decrypt(panEncrypted);
                                finalValue = MaskMobileNumber(decrypted);
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

            }

            return Json(rows);
        }
        catch (Exception ex)
        {
            _uti.WriteToFile("getHistory_List", "CallingHistoryController", "getHistory_List", ex.Message);
            throw;
        }
    }
    [HttpPost]
    public JsonResult SaveCustomerDetails([FromBody] CRM_Model _mod)
    {
        Console.WriteLine(_mod);
        if (_mod == null)
        {
            return Json(new { status = "error", msg = "_mod is NULL!" });
        }
        //if (Session["UserName"] == null)
        //{
        //    uti.WriteToFile(process, controller, "Saving Customer Details In CRM", "UserName not null");
        //    return Json(new { redirect = Url.Action("Login", "Account") }, JsonRequestBehavior.AllowGet);
        //}
        try
        {
            _uti.WriteToFile(Process, ControllerName, "Saving Customer Details In CRM", "Enter Try catch");
            DataTable dt = new DataTable();

            string id, Stage, MonthCode, MainDisposition, SubDisposition, callBackDate, Remarks, CreatedBy, UpdatedBy;

            id=_mod.Id;
            Stage=_mod.Stage;
            MonthCode = _mod.MonthCode;
            MainDisposition=_mod.MainDisposition;
            SubDisposition = _mod.SubDisposition;
            DateTime rawDate = _mod.CallBackDate;
            if (rawDate == DateTime.MinValue)
            {
                callBackDate = null;
            }
            else
            {
                callBackDate = rawDate.ToString("yyyy-MM-dd HH:mm:ss");
            }

            Remarks = _mod.Remarks;
            CreatedBy = _mod.userId;
            UpdatedBy = _mod.userId;

            _uti.WriteToFile(Process, ControllerName, "Saving Customer Details In CRM", "data stored in variables");

            dt = _gen.InsertCustomerDetails("INSERT", id, Stage, MonthCode, MainDisposition, SubDisposition,
                                           callBackDate, Remarks, CreatedBy, UpdatedBy);
            if (dt.Rows.Count>0)
            {
                if (dt.Rows[0]["MSG"].Equals("ADDED"))
                {
                    _mod.status = "success";
                    _mod.msg = "Details Saved Successfully";
                    _uti.WriteToFile(Process, ControllerName, "Saving Customer Details In CRM", "Details Saved Successfully");
                }
            }
            else
            {
                _mod.status = "error";
                _mod.msg = "Deatils Not Saved";
                _uti.WriteToFile(Process, ControllerName, "Saving Customer Details In CRM", "Deatils Not Saved");
            }
            return Json(_mod);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            _mod.status = "error";
            _mod.msg = ex.Message.ToString();
            _uti.WriteToFile(Process, ControllerName, "Saving Customer Details In CRM", ex.Message.ToString());
            return Json(_mod);
        }
    }
    [HttpPost]
    public JsonResult Dispose_Call_Dialer([FromBody] CRM_Model _mod)
    {
        //Classes.SessionHandler.ValidateSession(ref _userdetails);
        try
        {
            string CallBackDate;
            _uti.WriteToFile(Process, ControllerName, "Dispose_Call_Dialer", "Start");
            if (_mod.campaignId != null)
            {
                DateTime rawDate = _mod.CallBackDate;
                if (rawDate == DateTime.MinValue)
                {
                    CallBackDate = null;
                }
                else
                {
                    //CallBackDate = rawDate.ToString("yyyy-MM-dd HH:mm:ss");
                    CallBackDate = rawDate.ToString("dd-MM-yyyy HH:mm:ss");
                }
                Dispose_Call disp = new Dispose_Call(_contextAccessor, _configuration, _env);
                disp.CallDispose(_mod.campaignId, _mod.crtObjectId,
                _mod.userCrtObjectId, _mod.customerId, _mod.sessionId, _mod.MobileNumber,
                _mod.dispositionName, CallBackDate, "", Process);
                _mod.status = "success";
                _mod.msg = "Call Disposed Successfully";
                _uti.WriteToFile(Process, ControllerName, "Dispose_Call_Dialer", "Call Disposed Successfully");
            }

            else
            {
                _mod.status = "error";
                _mod.msg = "Call not Disposed";
                _uti.WriteToFile(Process, ControllerName, "Dispose_Call_Dialer", "Call not Disposed");
            }


            return Json(_mod);
        }
        catch (Exception ex)
        {
            _uti.WriteToFile(Process, ControllerName, "Dispose_Call_Dialer", ex.Message.ToString());
            _mod.status = "error";
            _mod.msg = ex.Message.ToString();
            return Json(_mod);
        }
    }
    [HttpGet]
    public IActionResult GetDropDownMainDisposition(string stage)
    {
        if (string.IsNullOrEmpty(stage))
        {
            stage="All";
        }
        try
        {
            DataTable dt = _gen.GetUIElements("MainDispo", stage, "", "", "");

            var maindipo = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                maindipo.Add(new SelectListItem
                {
                    Value = row["ID"].ToString(),
                    Text = row["NAME"].ToString()
                });
            }

            return Json(maindipo);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            System.Diagnostics.Debug.WriteLine("Error: " + ex.Message.ToString());
            _uti.WriteToFile(Process, ControllerName, "MaindispositionLoad", ex.Message);
            return Json(new { success = false, message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult GetSubDispositionsByMain(string mainDispoId)
    {
        if (string.IsNullOrEmpty(mainDispoId))
        {
            return Json(new List<SelectListItem>());
        }

        try
        {
            DataTable dt = _gen.GetUIElements("SubDispo", mainDispoId, "", "", "");

            var subList = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                subList.Add(new SelectListItem
                {
                    Value = row["ID"].ToString(),
                    Text = row["NAME"].ToString()
                });
            }

            return Json(subList);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            System.Diagnostics.Debug.WriteLine("Error: " + ex.Message.ToString());
            _uti.WriteToFile(Process, ControllerName, "SubdispositionLoad", ex.Message);
            return Json(new { success = false, message = ex.Message });
        }
    }
    //Agent Followups Part
    public IActionResult AgentFollowup()
    {
        UserInfo usersdeatils = new UserInfo();
        _session.ValidateSession(ref usersdeatils);
        var session = _contextAccessor.HttpContext.Session;
        var userDetailsJson = session.GetString("UserDetails");

        AgentFollowup _mod = new AgentFollowup();
        try
        {
            _mod.FollowupList=GetDropdown_FollowupType();
            _mod.StageFollowupList = GetFollowupList();
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message.ToString();
            _uti.WriteToFile(Process, ControllerName, "AgentFollowup PageLoad", ex.Message.ToString());
        }
        return View(_mod);
    }
    [HttpPost]
    public IActionResult AgentFollowup(AgentFollowup follow)
    {
        UserInfo usersdeatils = new UserInfo();
        _session.ValidateSession(ref usersdeatils);
        var session = _contextAccessor.HttpContext.Session;
        var userDetailsJson = session.GetString("UserDetails");
        var followup = follow.Followup;

        try
        {
            string startdate = follow.StartDate.ToString("yyyy-MM-dd");
            string enddate = follow.EndDate.ToString("yyyy-MM-dd");
            string UserId = session.GetString("EmpCode");
            int privilage = Convert.ToInt32(usersdeatils.privilege);
            follow.FollowupList=GetDropdown_FollowupType();
            follow.StageFollowupList = GetFollowupList();

            if (privilage == 6)
            {
                follow.AgentFollowupLists = AgentWise_Followup("AgentWise", startdate, enddate, UserId,followup);
            }
            else
            {
                follow.AgentFollowupLists = Agent_Followup("OverAll", startdate, enddate, followup);
            }

            if (follow.AgentFollowupLists != null && follow.AgentFollowupLists.Count > 0)
            {
                TempData["Message"] = "Data Found!";
                ViewBag.RecordCount = follow.AgentFollowupLists?.Count ?? 0;
            }
            else
            {
                TempData["ErrorMessage"] = "No Data Found!";
                ViewBag.RecordCount = 0;
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = ex.Message;
        }

        return View(follow);
    }
    public List<AgentFollowupList> AgentWise_Followup(string type, string startdate, string enddate, string UserId,string Followup)
    {
        List<AgentFollowupList> followup = new List<AgentFollowupList>();
        DataTable dt = _gen.Get_AgentFollowup_Details(type, startdate, enddate, UserId, Followup);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string MobileNumber = Utilities.Decrypt(row["MobileNumber"].ToString());
                string MaskedMobileNumber = MaskMobileNumber(MobileNumber);

                AgentFollowupList followupList = new AgentFollowupList
                {
                    Stage = row["Stage"].ToString(),
                    CustomerName = row["CustomerName"].ToString(),
                    MobileNumber = MaskedMobileNumber,
                    MobileNumberHidden = MobileNumber,
                    Disposition = row["Disposition"].ToString(),
                    FollowupDate = row["FollowUpDate"].ToString(),
                    CreatedBy = row["CreatedBy"].ToString(),
                    CreatedOn = row["CreatedOn"].ToString()
                };

                followup.Add(followupList);
            }
        }     

        return followup;
    }
    public List<AgentFollowupList> Agent_Followup(string type, string startdate, string enddate,string Followup)
    {
        List<AgentFollowupList> followup = new List<AgentFollowupList>();
        DataTable dt = _gen.Get_AgentFollowup_Details(type, startdate, enddate, "", Followup);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                string MobileNumber = Utilities.Decrypt(row["MobileNumber"].ToString());
                string MaskedMobileNumber = MaskMobileNumber(MobileNumber);

                AgentFollowupList followupList = new AgentFollowupList
                {
                    Stage = row["Stage"].ToString(),
                    CustomerName = row["CustomerName"].ToString(),
                    MobileNumber = MaskedMobileNumber,
                    MobileNumberHidden = MobileNumber,
                    Disposition = row["Disposition"].ToString(),
                    FollowupDate = row["FollowUpDate"].ToString(),
                    CreatedBy = row["CreatedBy"].ToString(),
                    CreatedOn = row["CreatedOn"].ToString()
                };

                followup.Add(followupList);
            }
        }

        return followup;
    }
    [HttpPost]
    public JsonResult Encrypt(string mobileNo, string Dialer_Config)
    {
        _uti.WriteToFile(Process, ControllerName, "Encryption", "Start");
        //if (Session["UserName"] == null)
        //{
        //    return Json(new { redirect = Url.Action("Login", "Account") }, JsonRequestBehavior.AllowGet);
        //}
        try
        {
            AgentFollowup _mod = new AgentFollowup();
            WebClient client = new WebClient();
            _mod.mobileNo = Utilities.Encrypt(mobileNo.ToString());
            _uti.WriteToFile(Process, ControllerName, "Encryption", "mobilenumberEncypted");


            if (Dialer_Config != null && Dialer_Config != "")
            {
                string s = Dialer_Config.ToString();
                string[] subs = s.Split('|');
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                var input = new
                {
                    campaignId = subs[0],
                    sessionId = subs[1],
                    phone = mobileNo,
                    requestId = "0",
                };

                string inputJson = JsonConvert.SerializeObject(input);
                dynamic json = JsonConvert.DeserializeObject(inputJson);
                string baseURL = "http://192.168.25.20:8888/ameyowebaccess/command/?command=manual-dial&data=" + inputJson + "";
                client.OpenRead(baseURL);
            }
            else
            {
                _mod.status = "error";
                _mod.msg = "";
            }

            return Json(_mod);
        }
        catch (Exception ex)
        {
            _uti.WriteToFile("Encrypt", ControllerName, "Encryption", ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    public List<SelectListItem> GetDropdown_FollowupType()
    {
        List<SelectListItem> reportType = new List<SelectListItem>();
        DataTable dt = new DataTable();

        dt = _gen.GetUIElements("FollowupType", "", "", "", "");

        foreach (DataRow row in dt.Rows)
        {
            reportType.Add(new SelectListItem { Value = row["ID"].ToString(), Text = row["Name"].ToString() });
        }
        return reportType;

    }
    public List<Stage> GetFollowupList()
    {
        DateTime date = DateTime.Now;
        string monthCode = date.ToString("MMyyyy");
        List<Stage> stage = new List<Stage>();

        DataSet ds = _gen.DashBoard("StageList", monthCode,"");

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            stage.Add(new Stage { Title = row["Title"].ToString(), PendingCount = (int)row["PendingCount"],TotalCount =(int)row["TotalCount"],Completed=(int)row["Completed"] });
        }
        return stage;

    }

}
