using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Muthoot.Models
{
    public class Dialer
    {

    }

    public class DeSerialization_CustInfo
    {
        public string name { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string phone3 { get; set; }
        public string fname { get; set; }
        public string mname { get; set; }
        public string lname { get; set; }
        public string maskedphone1 { get; set; }
        public string maskedphone2 { get; set; }
        public string maskedphone3 { get; set; }
    }


    public class CC_Email
    {

       
        public int? ccid { get; set; }
        public int? logId { get; set; }
        public int? sno { get; set; }
        public string createdTime { get; set; }
        public string region { get; set; }
        public string branchCode { get; set; }
       
        public string branchName { get; set; }

        
        public string customerNo { get; set; }

        
        public string customerName { get; set; }

 
        public string accountType { get; set; }

        
        public string accountNo { get; set; }

        
        public string qrc { get; set; }

         
        public string mainType { get; set; }

      
        public string subType { get; set; }

        
        public string addType { get; set; }
 
        public string remarks { get; set; }

        
        public string associate { get; set; }

        
        public string referenceId { get; set; }
        public string mailFor { get; set; }
        public string fromEmail { get; set; }
        public string toEmail { get; set; }
        public string ccList { get; set; }
        public string bccList { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string port { get; set; }
        public string host { get; set; }

        public string mailStatus { get; set; }
        public string mailSentBy { get; set; }
        public string mailTime { get; set; }
        public string mail_error_desc { get; set; }

        public string status { get; set; }
        public string msg { get; set; }
        public string responsecode { get; set; }
        public string otp { get; set; }
        public string employeename { get; set; }

    }
}