using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Muthoot.Models
{
	public class Reports
	{
        [DisplayName("From Date")]
		public DateTime FromDate { get; set; }
        [DisplayName("To Date")]
        public DateTime ToDate { get; set; }
        [DisplayName("Select Report")]
        public string Report { get; set; }
        public List<SelectListItem>  ReportList { get; set; }
        public DataTable ReportData { get; set; }
        public string ActionType { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public List<Dictionary<string, object>> ReportRows { get; set; }
        public int TotalRecords { get; set; }
    }
}