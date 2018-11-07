using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services.DTO.Project;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.WebApi.Controllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/project/{projectId}
        [HttpGet("{projectId}", Name = "ProjectById")]
        public IActionResult GetProjectById(int projectId)
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
        public IActionResult CreateProject([FromBody]ProjectCreateDto project)
        {
            if (project == null)
                return BadRequest("Project object cannot be null");

            var newProjectId = _projectService.CreateProject(project);

            return CreatedAtRoute("ProjectById", new { projectId =  newProjectId }, project);
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
    }
}