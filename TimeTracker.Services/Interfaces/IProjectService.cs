using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Services.DTO.Project;

namespace TimeTracker.Services.Interfaces
{
    public interface IProjectService
    {
        ProjectDto GetProjectById(int projectId);
        ProjectDto GetProjectByName(string name);
        IEnumerable<ProjectDto> GetAllProjects();

        int CreateProject(ProjectCreateDto project);
        void RemoveProject(int projectId);
    }
}
