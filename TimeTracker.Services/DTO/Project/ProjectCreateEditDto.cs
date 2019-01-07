using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Domain.Enums;

namespace TimeTracker.Services.DTO.Project
{
    public class ProjectCreateEditDto
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
