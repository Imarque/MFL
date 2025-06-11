using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Muthoot.Models
{
    public class CRM_Model
    {
        [Required]
        public string SearchValue { get; set; }
        public string SearchKey { get; set; }
        public string Stage { get; set; }
        public string StageName { get; set; }
        public string Id { get; set; }
        public string MobileNumber { get; set; }
        [Display(Name = "Dialer Config")]
        public string Dialer_Config { get; set; }
        public string Pan { get; set; }
        public string MonthCode { get; set; }
        public string MaskedValue { get; set; }
        public string Product { get; set; }
        public string ProductCode { get; set; }
        public string ProductType { get; set; }
        public string LoanDetails { get; set; }
        public string CustomerDetails { get; set; }
        public string MainDisposition { get; set; }
        public List<SelectListItem> MainDispositionList { get; set; }
        public string SubDisposition { get; set; }
        public List<SelectListItem> SubDispositionList { get; set; }
        [DisplayName("Followup Date")]
        public DateTime CallBackDate { get; set; }
        public string Remarks { get; set; }
        public List<CallHistory> CallingHistory { get; set; }
        public string userId { get; set; }
        public string sessionId { get; set; }
        public string status { get; set; }
        public string msg { get; set; }
        public string phone { get; set; }
        public string campaignId { get; set; }
        public string associationType { get; set; }
        public string lead_name { get; set; }
        public string maskingEnabled { get; set; }
        public string userName { get; set; }
        public string dialer_callType { get; set; }
        public string crm_push_generated_time { get; set; }
        public string userCrtObjectId { get; set; }
        public string dispositionName { get; set; }
        public string customerId { get; set; }
        public string Privilage { get; set; }
        public string crtObjectId { get; set; }
        public string leadId { get; set; }
        public string maskedPhone { get; set; }
        public string instanceId { get; set; }
        public string locale { get; set; }
        public string origin { get; set; }
        public string iframeId { get; set; }
        public string dialingId { get; set; }
    }
    public class CallHistory
    {
        public string CustomerName { get; set; }
        public string MobileNumber { get; set; }
        public string Pan { get; set; }
        public string MainDisposition { get; set; }
        public string SubDisposition { get; set; }
        public string FollowupDate { get; set; }
        public string Remarks { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}