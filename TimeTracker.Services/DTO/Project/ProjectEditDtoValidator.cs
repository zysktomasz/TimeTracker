using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Domain.Enums;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.Services.DTO.Project
{
    public class ProjectEditDtoValidator : AbstractValidator<ProjectEditDto>
    {
        private readonly IProjectService _projectService;

        public ProjectEditDtoValidator(IProjectService projectService)
        {
            _projectService = projectService;

            RuleFor(x => x.ProjectID)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(project => project)
                .Must(BeUniqueNameUnlessItsTheSame).WithMessage("You have another project with this name.");

            RuleFor(x => x.Color)
                .NotEmpty()
                .NotNull()
                .Must(BeValidColorEnum).WithMessage("This is not right color :(");
        }

        private bool BeUniqueNameUnlessItsTheSame(ProjectEditDto project)
        {
            // get project from database by name - to check if its already taken
            var projectFromDb = _projectService.GetProjectByName(project.Name);

            // if it is the same project you are trying update (so you dont change name, but change other property
            // like Color); then allow updating
            if (projectFromDb.ProjectID == project.ProjectID)
                return true;

            // now check your other projects for unique name
            return (projectFromDb == null) ? true : false; 
        }

        private bool BeValidColorEnum(string colorValue)
        {
            ColorEnum testColorEnum;

            if (!Enum.TryParse(colorValue, out testColorEnum))
            {
                return false;
            }
            return true;
        }
    }
}
