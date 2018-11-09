using FluentValidation;
using System;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.Services.DTO.Activity
{
    public class ActivityStartDtoValidator : AbstractValidator<ActivityStartDto>
    {


        private readonly IProjectService _projectService;


        public ActivityStartDtoValidator(IProjectService projectService)
        {
            _projectService = projectService;

            RuleFor(x => x.Name)
                .MaximumLength(100);
            RuleFor(x => x.ProjectID)
                .Must(CorrespondToExistingProject).WithMessage("Project with this ProjectID doesn't exist.").When(x => x.ProjectID != null);

        }

        private bool CorrespondToExistingProject(int? projectId)
        {
            var project = _projectService.GetProjectById(projectId.Value); // .Value, because we are sure it has value
            if (project == null)
                return false;

            return true;
        }
    }
}
