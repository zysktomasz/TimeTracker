using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TimeTracker.Domain.Enums;
using TimeTracker.Domain.Identity;

namespace TimeTracker.Domain.Entities
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(24)")]
        public ColorEnum Color { get; set; }

        public ICollection<Activity> Activities { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
