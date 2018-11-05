using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Services.DTO.Activity;

namespace TimeTracker.Services.Interfaces
{
    public interface IActivityService
    {
        IEnumerable<ActivityDto> GetAllActivities();
    }
}
