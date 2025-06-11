using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Muthoot.Classes
{
    [Serializable]
    public class UserInfo
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string userType { get; set; }
        public int privilege { get; set; }
        public int Process { get; set; }
        public string designation { get; set; }
        public int region { get; set; }
        public bool isMobileDevice { get; set; }
        public string sessionId { get; set; }
        public int? spl_access { get; set; }
        public string privilegename { get; set; }

    }
}