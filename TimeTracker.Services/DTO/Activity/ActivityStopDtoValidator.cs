using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Persistance;

namespace TimeTracker.Services.DTO.Activity
{
    public class ActivityStopDtoValidator : AbstractValidator<ActivityStopDto>
    {
        private readonly TimeTrackerDbContext _context;

        public ActivityStopDtoValidator(TimeTrackerDbContext context)
        {
            _context = context;

            // TODO: method below stopped working (because it now returns more than one result
            // gotta find a way to include currentUser in LINQ
            //RuleFor(x => x.TimeEnd.Value)
            //    .Must(HaveCurrentlyRunningActivity).WithMessage("There is no Activity currently running.").When(x => x.TimeEnd != null);
        }

        public bool HaveCurrentlyRunningActivity(DateTime date)
        {
            var activity = _context.Activities.SingleOrDefault(a => a.TimeEnd == null);
            if (activity == null)
                return false;

            return true;
        }
    }
}
