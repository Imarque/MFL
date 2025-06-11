using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Muthoot.Classes;
using Muthoot.Helper;
using Muthoot.Models;
using Nancy.Session;
using System.Data;

namespace Muthoot.Controllers
{
    public class UserManagerController : Controller
    {
        private readonly Utilities _uti;
        private readonly General _gen;
        private readonly SessionHandler _session;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        private const string Process = "Muthoot";
        private const string ControllerName = "UserManager";
        public UserManagerController(Utilities uti, General gen, SessionHandler session, IHttpContextAccessor contextAccessor, IConfiguration configuration, IWebHostEnvironment env)
        {
            _uti = uti;
            _gen = gen;
            _session = session;
            _contextAccessor=contextAccessor;
            _configuration = configuration;
            _env = env;
        }
        public IActionResult AdminHome()
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
            return View();
        }
        public ActionResult UserList(int pageNumber = 1)
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

            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["ErrorMessage"];

            List<Users> users = LoadUsers(privilege);

            int pageSize = 5;
            int totalRecords = users.Count;
            var pagedUsers = users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = Math.Ceiling((double)totalRecords / pageSize);

            return View(pagedUsers);
        }
        [HttpPost]
        public ActionResult UserList(string searchTerm, int pageNumber = 1)
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

            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["ErrorMessage"];

            List<Users> users = LoadUsers(privilege);


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.ToLower();
                users = users
                    .Where(u => u.EmpCode.ToLower().Contains(term) || u.UserName.ToLower().Contains(term))
                    .ToList();
            }

            int pageSize = 5;
            int totalRecords = users.Count;
            var pagedUsers = users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.SearchTerm = searchTerm;

            return View(pagedUsers);
        }
        private List<Users> LoadUsers(int privilege)
        {
            List<Users> users = new List<Users>();
            DataTable dt = _gen.UserMasterControl("GetActiveUsers", privilege.ToString(), "", "");
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    users.Add(new Users
                    {
                        EmpCode = row["EmpCode"].ToString(),
                        UserName = row["UserName"].ToString(),
                        DOJ = (DateTime)row["DOJ"],
                        Privilege = row["Privilege"].ToString(),
                        Region = row["Region"].ToString()
                    });
                }
                ViewBag.RecordCount = dt.Rows.Count;
            }
            return users;
        }
        public IActionResult UserManager()
        {
            _uti.WriteToFile(Process, ControllerName, "Adding Users Page Load", "Start");

            Users users = new Users();

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
                return RedirectToAction("Login", "Account");
            }
            try
            {
                users.PrivilegeList = GetDropDownPrivilege(privilege);
                users.RegionList = GetDropDownRegion();
                users.IsActiveList = GetDropDownIsActive();

                _uti.WriteToFile(Process, ControllerName, "Adding Users Page Load", "End");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]=ex.Message.ToString();
                _uti.WriteToFile(Process, ControllerName, "Adding Users Page Load", ex.Message.ToString());
            }
            return View(users);
        }
        public IActionResult UserModify(string Empcode)
        {
            _uti.WriteToFile(Process, ControllerName, "Users Modified PageLoad", "Start");
            Users users = new Users();

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
                return RedirectToAction("Login", "Account");
            }
            try
            {
                DataTable dt = new DataTable();
                dt = _gen.UserMasterControl("Manage", Empcode, "", "");
                if (dt.Rows.Count>0)
                {
                    users.EmpCode = dt.Rows[0]["EmpCode"].ToString();
                    users.UserName = dt.Rows[0]["UserName"].ToString();
                    users.UserID = dt.Rows[0]["UserID"].ToString();
                    users.Password = "";
                    users.DOJ=(DateTime)dt.Rows[0]["DOJ"];
                    users.Privilege = dt.Rows[0]["Privilege"].ToString();
                    users.PrivilegeList = GetDropDownPrivilege(privilege);
                    users.Region = dt.Rows[0]["Region"].ToString();
                    users.RegionList = GetDropDownRegion();
                    users.AmeyoID = dt.Rows[0]["AmeyoID"].ToString();
                    users.IsActive = dt.Rows[0]["IsActive"].ToString();
                    users.IsActiveList = GetDropDownIsActive();


                    _uti.WriteToFile(Process, ControllerName, "Users Modified PageLoad", "User Details Found");
                    TempData["Message"]= "User Details Found!";
                }
                else
                {
                    users.PrivilegeList = GetDropDownPrivilege(privilege);
                    users.RegionList = GetDropDownRegion();
                    users.IsActiveList = GetDropDownIsActive();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]=ex.Message.ToString();
                _uti.WriteToFile(Process, ControllerName, "Users Modified PageLoad", ex.Message.ToString());
            }

            return View(users);
        }
        [HttpPost]
        public IActionResult UserModify(Users users)
        {
            UserInfo usersdeatils = new UserInfo();
            _session.ValidateSession(ref usersdeatils);
            var session = HttpContext.Session;
            var userDetailsJson = session.GetString("UserDetails");
            var privilege = usersdeatils.privilege;
            if (privilege>4)
            {
                session.Clear();
                return RedirectToAction("Login", "Account");
            }
            try
            {
                DataTable dt = new DataTable();
                string Empcode, UserName, Password, UserID, DOJ, Privilege, Region, AmeyoID, IsActive, CreatedBy, UpdatedBy;
                Empcode = users.EmpCode;
                UserName = users.UserName;
                Password = users.Password;
                UserID = users.UserID;
                DOJ = users.DOJ.ToString("yyyy-MM-dd");
                Privilege = users.Privilege;
                Region = users.Region;
                AmeyoID = users.AmeyoID;
                IsActive = users.IsActive;
                CreatedBy = session.GetString("UserName");
                UpdatedBy = session.GetString("UserName");
                dt = _gen.Muthoot_InsertUsers("Update", Empcode, UserName, Password, UserID, DOJ, Privilege, Region, AmeyoID, IsActive, CreatedBy, UpdatedBy);
                if (dt.Rows.Count>0)
                {
                    if (dt.Rows[0]["MSG"].Equals("Updated"))
                    {
                        TempData["Message"]="User Details Updated SuccessFully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"]="Failed To Update!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"]=ex.Message.ToString();
                _uti.WriteToFile(Process, ControllerName, "Update User Details", ex.Message.ToString());
            }
            return RedirectToAction("UserList");

        }
        public List<SelectListItem> GetDropDownPrivilege(int privilegeaccss)
        {
            var session = HttpContext.Session;
            UserInfo usersdeatils = new UserInfo();
            List<SelectListItem> privilege = new List<SelectListItem>();

            DataTable dt = new DataTable();
            dt = _gen.UserMasterControl("Privilege", privilegeaccss.ToString(), "", "");

            foreach (DataRow row in dt.Rows)
            {
                privilege.Add(new SelectListItem
                {
                    Value=row["ID"].ToString(),
                    Text =row["NAME"].ToString()
                });
            }
            return privilege;
        }
        public List<SelectListItem> GetDropDownRegion()
        {
            List<SelectListItem> region = new List<SelectListItem>();

            DataTable dt = new DataTable();
            dt = _gen.UserMasterControl("Region", "", "", "");

            foreach (DataRow row in dt.Rows)
            {
                region.Add(new SelectListItem
                {
                    Value=row["ID"].ToString(),
                    Text =row["NAME"].ToString()
                });
            }
            return region;
        }
        public List<SelectListItem> GetDropDownIsActive()
        {
            List<SelectListItem> active = new List<SelectListItem>
            {
                new SelectListItem{ Text="Active",Value="1" },

                new SelectListItem{ Text="InActive",Value="0" }
            };
            return active;
        }
        public ActionResult Back()
        {
            return RedirectToAction("UserList");
        }
    }
}
