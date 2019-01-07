using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Services.DTO.Project
{
    // not like ProjectCreateDto, contains ProjectID sent from client
    // to allow FluentValidation to accept updating project with the same name
    // (meaning it changes just color)
    public class ProjectEditDto
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
