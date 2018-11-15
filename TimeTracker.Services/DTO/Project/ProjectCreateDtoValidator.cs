using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker.Services.DTO.Project
{
    public class ProjectCreateDtoValidator : AbstractValidator<ProjectCreateDto>
    {
        public ProjectCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
