using Microsoft.EntityFrameworkCore;
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
                    .Include(a => a.Project)
                .AsNoTracking() // "cheat" - avoids issue with removing already tracked entity (in RemoveEntity)
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
                TimeTotal = activity.TimeTotal,
                ProjectName = activity.Project?.Name
            };

            return activityDto;
        }

        public IEnumerable<ActivityDto> GetAllActivities()
        {
            var result = _context.Activities
                    .Include(a => a.Project)
                .Select(act => new ActivityDto
                {
                    ActivityID = act.ActivityID,
                    Name = act.Name,
                    TimeStart = act.TimeStart,
                    TimeEnd = act.TimeEnd.Value,
                    TimeTotal = act.TimeTotal.Value,
                    ProjectName = act.Project.Name ?? null
                })
                .AsEnumerable();

            return result;
        }

        public void RemoveActivity(int activityId)
        {
            // handle Activity object by its ID
            var activity = new Activity { ActivityID = activityId };

            _context.Activities.Remove(activity);
            _context.SaveChanges();
        }

        public int StartActivity(ActivityStartDto activity)
        {
            // look for currently active Activity - if found, stop it and start this one
            var currentlyActive = GetCurrentlyActiveActivity();

            if (currentlyActive != null)
            {
                // TODO update with current time sent from client instead of server time
                StopActivity(new ActivityStopDto { TimeEnd = DateTime.Now });
            }


            // create new Activity
            var entity = new Activity
            {
                Name = activity.Name,
                TimeStart = activity.TimeStart
            };

            // FluentValidation already validated that if client sents ProjectID it corresponds to existing Project
            // otherwise it doesn't even fire this method
            if (activity.ProjectID != null)
            {
                var assignedProject = _context.Projects.Where(p => p.ProjectID == activity.ProjectID).SingleOrDefault();
                entity.Project = assignedProject;
            }

            _context.Activities.Add(entity);
            _context.SaveChanges();

            return entity.ActivityID;
        }

        public void StopActivity(ActivityStopDto activity)
        {
            var entity = GetCurrentlyActiveActivity();

            // update properties
            entity.TimeEnd = activity.TimeEnd;
            entity.TimeTotal = (int)((activity.TimeEnd - entity.TimeStart).TotalSeconds); // TODO Validate TimeEnd value

            _context.Activities.Update(entity);

            _context.SaveChanges();
        }

        private Activity GetCurrentlyActiveActivity()
        {
            var entity = _context.Activities
                .SingleOrDefault(a => a.TimeEnd == null);

            if (entity == null)
                return null;

            return entity;
        }
    }
}
