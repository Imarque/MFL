using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Muthoot.Models
{
    public class Upload
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string UploadType { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.csv)$", ErrorMessage = "Only csv files allowed.")]
        public IFormFile PostedFile { get; set; }
        public List<UploadHistory> UploadHistory { get; set; }
    }
    public class UploadHistory
    {
        public string BaseType { get; set; }
        public string EmpolyeeCode { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedCount { get; set; }
        public string UploadedTime { get; set; }
    }
}