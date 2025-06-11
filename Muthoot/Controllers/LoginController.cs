using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Net;
using System.Text.Json;
using Muthoot.Classes;
using Muthoot.Helper;
using Microsoft.Extensions.Logging;
using DocumentFormat.OpenXml.Drawing.Charts;
using Muthoot.Models;
using System.Diagnostics;
using System;

namespace Muthoot.Controllers.Classes
{
    public class LoginController : Controller
    {
        private readonly Utilities _device;
        private readonly Utilities _uti;
        private readonly General _gen;
        private readonly ILogger<LoginController> _logger;

        private const string Process = "Muthoot";
        private const string ControllerName = "Login";

        public LoginController(Utilities device, Utilities uti, General gen, ILogger<LoginController> logger)
        {
            _device = device;
            _uti = uti;
            _gen = gen;
            _logger = logger;
        }

        public ActionResult Login(
            string campaignId, string phone, string crtObjectId, string userCrtObjectId,
            string customerId, string sessionId, string leadId, string lead_name,
            string userId, string crm_push_generated_time, string instanceId,
            string locale, string origin, string iframeId, string customerInfo,
            string associationType, string callType)
        {
            // Clear the session
            HttpContext.Session.Clear();
            _uti.WriteToFile(Process, ControllerName, "ActionResult Login", "Session Clear");
            _uti.WriteToFile(Process, ControllerName, "ActionResult Login", "Start");

            Login _mod = new Login();

            // Get IP Address
            string ipaddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipaddress = HttpContext.Request.Headers["X-Forwarded-For"];
            }

            _mod.isMobileDevice = _device.IsMobileDevice() ? 1 : 0;
            _mod.sessionId = HttpContext.Session.Id;
            _mod.ipAddress = ipaddress;

            // If campaignId is provided, populate other properties
            if (!string.IsNullOrEmpty(campaignId))
            {
                _mod.phone = phone;
                _mod.campaignId = campaignId;
                _mod.crtObjectId = crtObjectId;
                _mod.userCrtObjectId = userCrtObjectId;
                _mod.customerId = customerId;
                _mod.dialer_sessionId = sessionId;
                _mod.leadId = leadId;
                _mod.lead_name = lead_name;
                _mod.dialer_userId = userId;
                _mod.crm_push_generated_time = crm_push_generated_time;
                _mod.instanceId = instanceId;
                _mod.locale = locale;
                _mod.origin = origin;
                _mod.iframeId = iframeId;
                _mod.customerInfo = customerInfo;
                _mod.associationType = associationType;
                _mod.callType = callType;
            }

            // Read encrypted username & password from query string
            string userNameEncrypted = HttpContext.Request.Query["UName"];
            string passwordEncrypted = HttpContext.Request.Query["UCode"];

            if (!string.IsNullOrEmpty(userNameEncrypted) && !string.IsNullOrEmpty(passwordEncrypted))
            {
                try
                {
                    string userName = Utilities.Decrypt(WebUtility.UrlDecode(userNameEncrypted));
                    string password = Utilities.Decrypt(WebUtility.UrlDecode(passwordEncrypted));

                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                    {
                        _mod.userId = userName;
                        _mod.password = password;
                    }

                    System.Data.DataTable dt = _gen.Login_Check(_mod.userId, _mod.password, _mod.ipAddress, _mod.sessionId);

                    if (dt.Rows.Count > 0 && dt.Rows[0]["MSG"].ToString().ToUpper() == "SUCCESS")
                    {
                        // Set session variables
                        HttpContext.Session.SetString("userId", dt.Rows[0]["userId"].ToString());
                        HttpContext.Session.SetString("EmpCode", dt.Rows[0]["empcode"].ToString());
                        HttpContext.Session.SetString("UserName", dt.Rows[0]["userName"].ToString());
                        HttpContext.Session.SetInt32("Privilege", Convert.ToInt32(dt.Rows[0]["privilege"]));
                        HttpContext.Session.SetString("PrivilegeName", dt.Rows[0]["PrivilegeName"].ToString());
                        HttpContext.Session.SetString("region", dt.Rows[0]["region"].ToString());
                        HttpContext.Session.SetString("sessionId", dt.Rows[0]["sessionId"].ToString());

                        // Serialize and store UserDetails object
                        UserInfo _UserDetails = new UserInfo();
                        _UserDetails.userId = dt.Rows[0]["userId"].ToString();
                        _UserDetails.userName = dt.Rows[0]["userName"].ToString();
                        _UserDetails.privilege = Convert.ToInt32(dt.Rows[0]["privilege"].ToString());
                        _UserDetails.region = Convert.ToInt32(dt.Rows[0]["region"].ToString());
                        _UserDetails.sessionId = dt.Rows[0]["sessionId"].ToString();

                        string userDetailsJson = JsonSerializer.Serialize(_UserDetails);
                        HttpContext.Session.SetString("UserDetails", userDetailsJson);

                        _uti.WriteToFile(Process, ControllerName, "ActionResult Login", "Logged In Successfully");

                        // Redirect to CRM Home page
                        return RedirectToAction("CRM", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = dt.Rows[0]["MSG"].ToString();
                        _uti.WriteToFile(Process, ControllerName, "ActionResult Login", ViewBag.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during login processing");
                    ViewBag.ErrorMessage = "An error occurred during login.";
                }
            }

            return View(_mod);

        }
        [HttpPost]
        public JsonResult Login([FromBody] Login _mod)
        {
            _uti.WriteToFile(Process, ControllerName, "JsonResult Login", "Start");
            try
            {
                System.Data.DataTable dt = _gen.Login_Check(_mod.userId, _mod.password, _mod.ipAddress, _mod.sessionId);
                if (dt.Rows[0]["MSG"].ToString().ToUpper().Equals("SUCCESS"))
                {
                    HttpContext.Session.SetString("userId", dt.Rows[0]["userId"].ToString());
                    HttpContext.Session.SetString("EmpCode", dt.Rows[0]["empcode"].ToString());
                    HttpContext.Session.SetString("UserName", dt.Rows[0]["userName"].ToString());
                    HttpContext.Session.SetInt32("Privilege", Convert.ToInt32(dt.Rows[0]["privilege"]));
                    HttpContext.Session.SetString("PrivilegeName", dt.Rows[0]["PrivilegeName"].ToString());
                    HttpContext.Session.SetString("region", dt.Rows[0]["region"].ToString());
                    HttpContext.Session.SetString("sessionId", dt.Rows[0]["sessionId"].ToString());
                    HttpContext.Session.SetString("PrivilegeName", dt.Rows[0]["PrivilegeName"].ToString());

                    UserInfo _UserDetails = new UserInfo();
                    _UserDetails.userId = dt.Rows[0]["userId"].ToString();
                    _UserDetails.userName = dt.Rows[0]["userName"].ToString();
                    _UserDetails.privilege = Convert.ToInt32(dt.Rows[0]["privilege"].ToString());
                    _UserDetails.region = Convert.ToInt32(dt.Rows[0]["region"].ToString());
                    _UserDetails.sessionId = dt.Rows[0]["sessionId"].ToString();
                    _UserDetails.privilegename = dt.Rows[0]["PrivilegeName"].ToString();

                    string userDetailsJson = JsonSerializer.Serialize(_UserDetails);

                    HttpContext.Session.SetString("UserDetails", userDetailsJson);

                    _mod.privilege = Convert.ToInt32(dt.Rows[0]["privilege"].ToString());
                    _mod.userType = _UserDetails.userType;
                    _mod.status = "success";
                    _mod.msg = "Login Success";
                    _uti.WriteToFile(Process, ControllerName, "JsonResult Login", _mod.msg);
                    return Json(_mod);
                }
                else
                {

                    _mod.status = "error";
                    _mod.msg = dt.Rows[0]["MSG"].ToString();
                    _uti.WriteToFile(Process, ControllerName, "JsonResult Login", _mod.msg);
                    return Json(_mod);
                }
            }
            catch (Exception ex)
            {
                _uti.WriteToFile(Process, ControllerName, "JsonResult Login", ex.Message.ToString());
                _mod.status = "error";
                _mod.msg = ex.Message.ToString();
                return Json(_mod);
            }
        }
        public ActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                _uti.WriteToFile(Process, ControllerName, "Logout", "Logout Done");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login processing");
                _uti.WriteToFile(Process, ControllerName, "JsonResult Login", ex.Message.ToString());
                _uti.WriteToFile(Process, ControllerName, "Logout", ex.Message.ToString());
                ViewBag.ErrorMessage = "An error occurred during login.";
            }
            return RedirectToAction("Login");
        }
        public IActionResult SessionExpired()
        {
            return View();
        }
    }
}
