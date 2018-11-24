using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Domain.Entities;
using TimeTracker.Persistance;
using TimeTracker.Services.DTO.Project;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly TimeTrackerDbContext _context;

        public ProjectService(TimeTrackerDbContext context)
        {
            _context = context;
        }

        public int CreateProject(ProjectCreateDto project)
        {
            var entity = new Project
            {
                Name = project.Name
            };

            _context.Projects.Add(entity);
            _context.SaveChanges();

            return entity.ProjectID;
        }

        public IEnumerable<ProjectDto> GetAllProjects()
        {
            var result = _context.Projects
                .Select(p => new ProjectDto
                {
                    ProjectID = p.ProjectID,
                    Name = p.Name
                })
                .AsEnumerable();

            return result;
        }

        public ProjectDto GetProjectById(int projectId)
        {
            var project = _context.Projects
                .AsNoTracking()
                .SingleOrDefault(p => p.ProjectID == projectId);

            if (project == null)
                return null;

            // map domain entity (Project) to DTO (ProjectDto)
            var projectDto = new ProjectDto
            {
                ProjectID = project.ProjectID,
                Name = project.Name
            };

            return projectDto;
        }

        public ProjectDto GetProjectByName(string name)
        {
            var project = _context.Projects
                .AsNoTracking()
                .SingleOrDefault(p => p.Name == name);

            if (project == null)
                return null;

            // map domain entity (Project) to DTO (ProjectDto)
            var projectDto = new ProjectDto
            {
                ProjectID = project.ProjectID,
                Name = project.Name
            };

            return projectDto;
        }

        public void RemoveProject(int projectId)
        {
            var projectToRemove = new Project { ProjectID = projectId };

            _context.Projects.Remove(projectToRemove);
            _context.SaveChanges();
        }
    }
}
