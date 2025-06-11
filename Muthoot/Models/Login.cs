using System.ComponentModel.DataAnnotations;

namespace Muthoot.Models
{
    public class Login
    {
        [Required(ErrorMessage = "UserName is Required")]
        [Display(Name = "UserName")]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "Username Required Minimum 1 and Maximum 15 Characters")]
        [DataType(DataType.Text)]
        public string userId { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [Display(Name = "Password")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Password Required Minimum 3 and Maximum 15 Characters")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public int? privilege { get; set; }

        [Display(Name = "IP Address")]
        public string ipAddress { get; set; }

        [Display(Name = "Session Id")]
        public string sessionId { get; set; }

        [Display(Name = "User Type")]
        public string userType { get; set; }

        [Display(Name = "Is Mobile Device?")]
        public int isMobileDevice { get; set; }

        //Newly Added for OTP

        public string otp { get; set; }
        public int? two_factor_authentication { get; set; }

        //Newly Added Ends

        public string status { get; set; }
        public string msg { get; set; }
        public string responsecode { get; set; }

        //added arul 

        public string campaignId { get; set; }
        public string associationType { get; set; }
        public string lead_name { get; set; }
        public string dialer_sessionId { get; set; }
        public string maskingEnabled { get; set; }
        public string dialer_userId { get; set; }
        public string callType { get; set; }
        public string crm_push_generated_time { get; set; }
        public string userCrtObjectId { get; set; }
        public string phone { get; set; }
        public string customerId { get; set; }
        public string crtObjectId { get; set; }
        public string leadId { get; set; }
        public string maskedPhone { get; set; }
        public string instanceId { get; set; }
        public string locale { get; set; }
        public string origin { get; set; }
        public string iframeId { get; set; }

        public string customerInfo { get; set; }
    }
}

