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
        public IEnumerable<DailyActivitySummary> Get(string groupBy = "day")
        {
            
            var dailyActivities =  _context.DailyActivitySummaries.OrderBy(x=>x.DateTime).ToList();

            if (groupBy.ToLower() == "week")
            {
                var weekGroups = dailyActivities.GroupBy(x => x.DateTime.AddDays(-(int)x.DateTime.DayOfWeek));


                var weeklyActivities = new List<DailyActivitySummary>();
                foreach (var group in weekGroups)
                {
                    var activity = new DailyActivitySummary
                    {
                        DateTime = group.Key,
                        //  ActiveMinutes = group.Sum(x => x.ActiveMinutes)
                        SedentaryMinutes = group.Sum(x => x.SedentaryMinutes),
                        LightlyActiveMinutes = group.Sum(x => x.LightlyActiveMinutes),
                        FairlyActiveMinutes = group.Sum(x => x.FairlyActiveMinutes),
                        VeryActiveMinutes = group.Sum(x => x.VeryActiveMinutes)
                    };

                    weeklyActivities.Add(activity);
                }

                return weeklyActivities;
            }

            return dailyActivities;
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
