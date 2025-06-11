using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Muthoot.Models
{
	public class AgentFollowup
	{
		[DisplayName("Start Date")]
		public DateTime StartDate { get; set; }
		[DisplayName("End Date")]
		public DateTime EndDate { get; set; }
        public string Followup { get; set; }
        [DisplayName("Followup Type")]
        public List<SelectListItem> FollowupList { get; set; }
        public List<AgentFollowupList> AgentFollowupLists { get; set; }
        public string Dialer_Config { get; set; }
        public string mobileNo { get; set; }
        public string status { get; set; }
        public string msg { get; set; }
        public List<Stage> StageFollowupList { get; set; } = new List<Stage>();
    }
    public class Stage
    {
        public string Title { get; set; }
        public int PendingCount { get; set; }
        public int TotalCount { get; set; }
        public int Completed { get; set; }
    }

    public class AgentFollowupList
	{
        public string Stage { get; set; }
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNumberHidden { get; set; }
        public string Disposition { get; set; }
        public string FollowupDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
    }
}