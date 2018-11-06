using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Services.DTO.Activity;

namespace TimeTracker.Services.Interfaces
{
    public interface IActivityService
    {
        IEnumerable<ActivityDto> GetAllActivities();
        ActivityDto GetActivityById(int activityId);

        void StartActivity(ActivityStartDto activity);
        void StopActivity(ActivityStopDto activity);
    }
}
