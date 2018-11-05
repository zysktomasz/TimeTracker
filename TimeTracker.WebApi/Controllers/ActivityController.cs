using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeTracker.WebApi.Controllers
{
    [Route("api/activity")]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        // GET: api/activity/all
        [HttpGet("all")]
        public IActionResult GetAllActivities()
        {
            var activities = _activityService.GetAllActivities();

            return Ok(activities);
        }
    }
}
