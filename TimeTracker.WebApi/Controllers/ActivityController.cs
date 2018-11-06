using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeTracker.Services.DTO.Activity;
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

        // GET: api/activity/{activityId}
        [HttpGet("{activityId}", Name = "ActivityById")]
        public IActionResult GetActivityById(int activityId)
        {
            var activity = _activityService.GetActivityById(activityId);

            if (activity == null)
                return NotFound();

            return Ok(activity);
        }

        // GET: api/activity/all
        [HttpGet("all")]
        public IActionResult GetAllActivities()
        {
            var activities = _activityService.GetAllActivities();

            return Ok(activities);
        }

        // POST: api/activity/start
        [HttpPost("start")]
        public IActionResult StartActivity([FromBody]ActivityStartDto activity)
        {
            if (activity == null)
                return BadRequest("Activity object cannot be null");

            _activityService.StartActivity(activity);

            return Ok(activity); // replace with CreatedAtRoute
        }

        // PUT: api/activity/stop
        [HttpPut("stop")]
        public IActionResult StopActivity([FromBody]ActivityStopDto activity)
        {
            if (activity == null)
                return BadRequest("Activity object cannot be null");

            _activityService.StopActivity(activity);

            return NoContent(); // successfull stop
        }
    }
}
