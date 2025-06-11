using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Muthoot.Models
{
    public class Common
    {
        public string status { get; set; }
        public string msg { get; set; }
        public string responsecode { get; set; }

        public string Type { get; set; }
        public List<Activity> activity { get; set; }
    }
    public class Activity
    {
        public int sno { get; set; }
        public string activityName { get; set; }
        public string activity_desc { get; set; }
        public string activityTime { get; set; }
        public string activityBy { get; set; }
        public string process { get; set; }
    }

    public class DateRange
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }
}