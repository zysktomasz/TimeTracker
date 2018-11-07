using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Domain.Entities
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }

        public ICollection<Activity> Activities { get; set; }
    }
}
