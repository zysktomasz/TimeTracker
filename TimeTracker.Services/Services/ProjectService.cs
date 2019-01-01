using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TimeTracker.Domain.Entities;
using TimeTracker.Domain.Identity;
using TimeTracker.Persistance;
using TimeTracker.Services.DTO.Project;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly TimeTrackerDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserAccount _currentUser;

        public ProjectService(TimeTrackerDbContext context, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;

            string userEmail = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _currentUser = _context.Users.FirstOrDefault(u => u.Email == userEmail);
        }

        public ProjectDto CreateProject(ProjectCreateDto project)
        {
            var entity = new Project
            {
                Name = project.Name,
                UserAccount = _currentUser
            };

            _context.Projects.Add(entity);
            _context.SaveChanges();

            var projectDto = _mapper.Map<ProjectDto>(entity);

            return projectDto;
        }

        public IEnumerable<ProjectDto> GetAllProjects()
        {
            var projectsFromDb = _context
                                    .Projects
                                        .Where(p => p.UserAccount == _currentUser)
                                    .OrderByDescending(p => p.ProjectID)
                                    .AsEnumerable();
            var result = _mapper.Map<IEnumerable<ProjectDto>>(projectsFromDb);

            return result;
        }

        public ProjectDto GetProjectById(int projectId)
        {
            var project = _context
                            .Projects
                                .Where(p => p.UserAccount == _currentUser)
                            .AsNoTracking()
                            .SingleOrDefault(p => p.ProjectID == projectId);

            if (project == null)
                return null;

            var projectDto = _mapper.Map<ProjectDto>(project);

            return projectDto;
        }

        public ProjectDto GetProjectByName(string name)
        {
            var project = _context
                            .Projects
                                .Where(p => p.UserAccount == _currentUser)
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
            // TODO: Validate if request is sent by project's owner
            // TODO: Validate if request is sent by project's owner
            // TODO: Validate if request is sent by project's owner

            var projectToRemove = new Project { ProjectID = projectId };

            _context.Projects.Remove(projectToRemove);
            _context.SaveChanges();
        }
    }
}
