using System;
using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using HealthAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class DailyActivitiesController : Controller
    {
        private readonly HealthContext _context;

        public DailyActivitiesController(HealthContext context)
        {
            _context = context;
        }

        // GET api/DailyActivities
        [HttpGet]
        public IEnumerable<Activity> Get()
        {
            return _context.DailyActivitySummaries.OrderBy(x=>x.DateTime).Select(x=>new Activity
            {
                Day = x.DateTime,
                ActiveMinutes = x.FairlyActiveMinutes.Value + x.VeryActiveMinutes.Value

            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] Models.DailyActivitySummary activity)
        {
            try
            {
                if (activity == null)
                {
                    return BadRequest();
                }

                var existingItem = _context.DailyActivitySummaries.FirstOrDefault(x => x.DateTime == activity.DateTime);

                if (existingItem != null)
                {
                    existingItem.DateTime = activity.DateTime;
                    existingItem.SedentaryMinutes = activity.SedentaryMinutes;
                    existingItem.LightlyActiveMinutes = activity.LightlyActiveMinutes;
                    existingItem.FairlyActiveMinutes = activity.FairlyActiveMinutes;
                    existingItem.VeryActiveMinutes = activity.VeryActiveMinutes;

                    _context.DailyActivitySummaries.Update(existingItem);
                }
                else
                {
                    _context.DailyActivitySummaries.Add(activity);
                }


                _context.SaveChanges();

                //return CreatedAtRoute("GetTodo", weight);
                return Created("/bum", activity);
                //return new NoContentResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


    }
}
