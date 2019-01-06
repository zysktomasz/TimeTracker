using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services.DTO.Project;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.WebApi.Controllers
{
    [Route("api/project")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        // test
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/project/{projectId}
        [HttpGet("{projectId}", Name = "ProjectById")]
        public IActionResult GetSingleProject(int projectId)
        {
            var project = _projectService.GetProjectById(projectId);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        // GET: api/project/all
        [HttpGet("all")]
        public IActionResult GetAllProjects()
        {
            var projects = _projectService.GetAllProjects();

            return Ok(projects);
        }

        // POST: api/project
        [HttpPost]
        public IActionResult CreateProject([FromBody]ProjectCreateEditDto project)
        {
            if (project == null)
                return BadRequest("Project object cannot be null");

            var newProject = _projectService.CreateProject(project);

            return CreatedAtRoute("ProjectById", new { projectId =  newProject.ProjectID }, newProject);
        }

        // DELETE: api/project/{projectId}
        [HttpDelete("{projectId}")]
        public IActionResult RemoveProject(int projectId)
        {
            var project = _projectService.GetProjectById(projectId);

            if (project == null)
                return NotFound();

            _projectService.RemoveProject(projectId);
            return NoContent();
        }

        // PUT: api/project/{projectId}
        [HttpPut("{projectId}")]
        public IActionResult UpdateProject(int projectId, [FromBody] ProjectCreateEditDto updatedProject)
        {
            var projectToUpdate = _projectService.GetProjectById(projectId);

            if (projectToUpdate == null)
                return NotFound();

            _projectService.UpdateProject(projectToUpdate, updatedProject);
            return NoContent();
        }
    }
}