using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeTracker.Domain.Entities;
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

        public ActivityDto GetActivityById(int activityId)
        {
            var activity = _context.Activities
                .SingleOrDefault(a => a.ActivityID == activityId);

            // TODO add better error handling - custom response wrapper(?)
            if (activity == null)
                return null;

            // map domain entity (Activity) to dto (ActivityDto)

            var activityDto = new ActivityDto
            {
                ActivityID = activity.ActivityID,
                Name = activity.Name,
                TimeStart = activity.TimeStart,
                TimeEnd = activity.TimeEnd,
                TimeTotal = activity.TimeTotal
            };

            return activityDto;
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

        public void StartActivity(ActivityStartDto activity)
        {
            // look for currently active Activity - if found, stop it and start this one
            var currentlyActive = _context.Activities
                .SingleOrDefault(a => a.TimeEnd == null);
            if (currentlyActive != null)
            {
                // TODO update with current time sent from client instead of server time
                StopActivity(new ActivityStopDto { TimeEnd = DateTime.Now }); 
            }

            // create new Activity
            var entity = new Activity
            {
                Name = activity.Name,
                TimeStart = activity.TimeStart,
                // properties below yet unspecified
                TimeEnd = null,
                TimeTotal = null
            };

            _context.Activities.Add(entity);
            _context.SaveChanges();
        }

        public void StopActivity(ActivityStopDto activity)
        {
            var entity = _context.Activities
                .SingleOrDefault(a => a.TimeEnd == null);

            // TODO Add error handling - when no activity to stop
            if (entity == null)
                return;

            // update properties
            entity.TimeEnd = activity.TimeEnd;
            entity.TimeTotal = (int)((activity.TimeEnd - entity.TimeStart).TotalSeconds); // TODO Validate TimeEnd value

            _context.Activities.Update(entity);

            _context.SaveChanges();
        }
    }
}
