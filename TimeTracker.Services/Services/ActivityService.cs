using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TimeTracker.Domain.Entities;
using TimeTracker.Domain.Identity;
using TimeTracker.Persistance;
using TimeTracker.Services.DTO.Activity;
using TimeTracker.Services.Interfaces;

namespace TimeTracker.Services.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IMapper _mapper;
        private readonly TimeTrackerDbContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserAccount _currentUser;

        public ActivityService(TimeTrackerDbContext context, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;

            // gets UserAccount object by user email from JWT
            // replaces async _userManager.GetUserAsync(User)
            // TODO: probably should reconsider this
            string userEmail = _httpContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _currentUser = _context.Users.FirstOrDefault(u => u.Email == userEmail);
        }

        //private UserAccount GetCurrentUserAccount()
        //{
        //    string userEmail = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        //    var currUser = _context.Users.FirstOrDefault(u => u.Email == userEmail);

        //    return currUser;
        //}


        public ActivityDto GetActivityById(int activityId)
        {
            var activity = _context
                            .Activities
                                .Where(a => a.UserAccount == _currentUser)
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

            var activitiesFromDb = _context
                                    .Activities
                                        .Where(a => a.UserAccount == _currentUser)
                                        .Include(a => a.Project)
                                    .OrderByDescending(a => a.TimeStart)
                                    .AsEnumerable();
            var result = _mapper.Map<IEnumerable<ActivityDto>>(activitiesFromDb);

            return result;
        }

        public void RemoveActivity(int activityId)
        {
            // TODO: validate if request is sent by user who owns this activity
            // TODO: validate if request is sent by user who owns this activity
            // TODO: validate if request is sent by user who owns this activity
            var activity = new Activity { ActivityID = activityId };

            _context.Activities.Remove(activity);
            _context.SaveChanges();
        }

        public ActivityStartReturnDto StartActivity(ActivityStartDto activity)
        {
            // TODO: validate if assignedProject is owned by _currentUser
            // TODO: validate if assignedProject is owned by _currentUser
            // TODO: validate if assignedProject is owned by _currentUser

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
                TimeStart = activity.TimeStart ?? DateTime.Now,
                UserAccount = _currentUser
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
            var entity = _context
                        .Activities
                            .Where(a => a.UserAccount == _currentUser)
                        .SingleOrDefault(a => a.TimeEnd == null);

            if (entity == null)
                return null;

            return entity;
        }
    }
}
