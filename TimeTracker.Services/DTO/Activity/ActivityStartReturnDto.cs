using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Services.DTO.Activity
{
    public class ActivityStartReturnDto
    {
        public int ActivityID { get; set; }
        public string Name { get; set; }
        public DateTime? TimeStart { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
    }
}
