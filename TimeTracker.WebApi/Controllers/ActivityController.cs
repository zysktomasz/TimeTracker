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
    [ApiController]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        // GET: api/activity/{activityId}
        [HttpGet("{activityId}", Name = "ActivityById")]
        public IActionResult GetSingleActivity(int activityId)
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

            var newActivityId = _activityService.StartActivity(activity);

            return CreatedAtRoute("ActivityById", new { activityId = newActivityId }, activity);
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

        // DELETE: api/activity/{activityId}
        [HttpDelete("{activityId}")]
        public IActionResult RemoveActivity(int activityId)
        {
            // check if Activity with specified ID exists
            var activity = _activityService.GetActivityById(activityId);

            if (activity == null)
                return NotFound();

            // exists -> remove it
            _activityService.RemoveActivity(activityId);
            return NoContent();
        }
    }
}
