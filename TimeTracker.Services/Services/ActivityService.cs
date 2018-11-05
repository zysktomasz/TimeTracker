using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Persistance;
using TimeTracker.Services.DTO.Activity;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.Services.Services
{
    public class ActivityService : IActivityService
    {
        private readonly TimeTrackerDbContext _context;

        public ActivityService(TimeTrackerDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ActivityDto> GetAllActivities()
        {
            var result = _context.Activities
                .Select(act => new ActivityDto
                {
                    ActivityID = act.ActivityID,
                    Name = act.Name,
                    TimeStart = act.TimeStart,
                    TimeEnd = act.TimeEnd.Value,
                    TimeTotal = act.TimeTotal.Value
                })
                .AsEnumerable();

            return result;
        }
    }
}
