using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Services.DTO.Project
{
    public class ProjectDto
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public int TotalHours { get; set; }
    }
}
