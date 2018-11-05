using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Services.DTO.Activity
{
    public class ActivityDto
    {
        public int ActivityID { get; set; }
        public string Name { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? TimeTotal { get; set; }
    }
}
