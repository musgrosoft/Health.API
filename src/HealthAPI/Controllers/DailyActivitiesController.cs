using System;
using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
//    [Route("api/[controller]")]
    public class DailyActivitiesController : ODataController
    {
        private readonly HealthContext _context;

        public DailyActivitiesController(HealthContext context)
        {
            _context = context;
        }

        // GET api/DailyActivities
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<DailyActivity> Get()
        {
            return _context.DailyActivitySummaries.AsQueryable();
        }


        [HttpGet]
        [Route("api/DailyActivities/GroupByWeek")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<DailyActivity> GetByWeek()
        {
            var dailyActivities = _context.DailyActivitySummaries.OrderBy(x => x.DateTime).ToList();

            var weekGroups = dailyActivities.GroupBy(x => x.DateTime.AddDays(-(int)x.DateTime.DayOfWeek));


            var weeklyActivities = new List<DailyActivity>();
            foreach (var group in weekGroups)
            {
                var activity = new DailyActivity
                {
                    DateTime = group.Key,
                    SedentaryMinutes = group.Sum(x => x.SedentaryMinutes),
                    LightlyActiveMinutes = group.Sum(x => x.LightlyActiveMinutes),
                    FairlyActiveMinutes = group.Sum(x => x.FairlyActiveMinutes),
                    VeryActiveMinutes = group.Sum(x => x.VeryActiveMinutes)
                };

                weeklyActivities.Add(activity);
            }

            return weeklyActivities.AsQueryable();
        }


        [HttpPost]
        [Route("api/DailyActivitySummaries")]
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
