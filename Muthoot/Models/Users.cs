using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Muthoot.Models
{
	public class Users
	{
        public string Search { get; set; }
        public string EmpCode { get; set; }
        [DisplayName("Employee Name")]
        public string UserName { get; set; }
        public string Password { get; set; }
        [DisplayName("User Name")]
        public string UserID { get; set; }
        [DisplayName("Date Of Joining")]
        public DateTime DOJ { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Privilege { get; set; }
        public List<SelectListItem> PrivilegeList { get; set; }
        public string Region { get; set; }
        public List<SelectListItem> RegionList { get; set; }
        public string AmeyoID { get; set; }
        public string IsActive { get; set; }
        public List<SelectListItem> IsActiveList { get; set; }
    }
}