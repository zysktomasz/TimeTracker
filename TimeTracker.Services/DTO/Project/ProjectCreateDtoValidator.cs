using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.Services.DTO.Project
{
    public class ProjectCreateDtoValidator : AbstractValidator<ProjectCreateDto>
    {
        private readonly IProjectService _projectService;

        public ProjectCreateDtoValidator(IProjectService projectService)
        {
            _projectService = projectService;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .Must(BeUniqueName).WithMessage("Project with this name already exists.");
        }

        private bool BeUniqueName(string name)
        {
            var project = _projectService.GetProjectByName(name);
            return (project == null) ? true : false;
        }
    }
}
