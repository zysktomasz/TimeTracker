using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Domain.Enums;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.Services.DTO.Project
{
    public class ProjectCreateEditDtoValidator : AbstractValidator<ProjectCreateEditDto>
    {
        private readonly IProjectService _projectService;

        public ProjectCreateEditDtoValidator(IProjectService projectService)
        {
            _projectService = projectService;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .Must(BeUniqueName).WithMessage("Project with this name already exists.");

            RuleFor(x => x.Color)
                .NotEmpty()
                .NotNull()
                .Must(BeValidColorEnum).WithMessage("This is not right color :(");
        }

        private bool BeUniqueName(string name)
        {
            var project = _projectService.GetProjectByName(name);
            return (project == null) ? true : false;
        }

        private bool BeValidColorEnum(string colorValue)
        {
            ColorEnum testColorEnum;
            
            if(!Enum.TryParse(colorValue, out testColorEnum))
            {
                return false;
            }
            return true;
        }
    }
}
