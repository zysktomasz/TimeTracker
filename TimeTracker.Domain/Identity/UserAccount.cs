using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TimeTracker.Domain.Entities;

namespace TimeTracker.Domain.Identity
{
    public class UserAccount : IdentityUser
    {
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
