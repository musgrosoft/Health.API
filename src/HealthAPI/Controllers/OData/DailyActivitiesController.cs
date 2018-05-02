using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Models;

namespace HealthAPI.Controllers.OData
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
        [Route("odata/DailyActivities/GroupByWeek")]
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


    }
}
