using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Domain.Identity;

namespace TimeTracker.Domain.Entities
{
    public class Activity
    {
        public int ActivityID { get; set; }
        public string Name { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? TimeTotal { get; set; }

        public Project Project { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
