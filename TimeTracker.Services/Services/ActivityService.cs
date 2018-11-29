using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly TimeTrackerDbContext _context;

        public ActivityService(TimeTrackerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            
            var activityDto = _mapper.Map<ActivityDto>(activity);

            return activityDto;
        }

        public IEnumerable<ActivityDto> GetAllActivities()
        {
            var activitiesFromDb = _context.Activities.Include(a => a.Project).AsEnumerable();
            var result = _mapper.Map<IEnumerable<ActivityDto>>(activitiesFromDb);

            return result;
        }

        public void RemoveActivity(int activityId)
        {
            var activity = new Activity { ActivityID = activityId };

            _context.Activities.Remove(activity);
            _context.SaveChanges();
        }

        public ActivityStartReturnDto StartActivity(ActivityStartDto activity)
        {
            // look for currently active Activity - if found, stop it and start this one
            var currentlyActive = GetCurrentlyActiveActivity();

            if (currentlyActive != null)
            {
                // TODO update with current time sent from client instead of server time
                StopActivity(new ActivityStopDto { TimeEnd = DateTime.Now });
            }

            var entity = new Activity
            {
                Name = activity.Name,
                TimeStart = activity.TimeStart ?? DateTime.Now
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

            var activityReturnDto = _mapper.Map<ActivityStartReturnDto>(entity);
            activityReturnDto.ProjectID = entity.Project?.ProjectID;

            return activityReturnDto;
        }

        public void StopActivity(ActivityStopDto activity)
        {
            var entity = GetCurrentlyActiveActivity();

            // update properties
            entity.TimeEnd = activity.TimeEnd ?? DateTime.Now;
            entity.TimeTotal = (int)((entity.TimeEnd - entity.TimeStart).Value.TotalSeconds); // TODO Validate TimeEnd value

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
