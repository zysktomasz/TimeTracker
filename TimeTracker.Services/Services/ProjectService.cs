﻿using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly TimeTrackerDbContext _context;

        public ProjectService(TimeTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            var projectsFromDb = _context.Projects.AsEnumerable();
            var result = _mapper.Map<IEnumerable<ProjectDto>>(projectsFromDb);

            return result;
        }

        public ProjectDto GetProjectById(int projectId)
        {
            var project = _context.Projects
                .AsNoTracking()
                .SingleOrDefault(p => p.ProjectID == projectId);

            if (project == null)
                return null;

            var projectDto = _mapper.Map<ProjectDto>(project);

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
