using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Models;
using Utils;

namespace HealthAPI.Controllers.OData
{
    public class StepCountsController : ODataController
    {
        private readonly HealthContext _context;

        public StepCountsController(HealthContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<StepCount> Get()
        {
            return _context.StepCounts;
        }

        [HttpGet]
        [Route("odata/StepCounts/GroupByWeek")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<StepCount> GetByWeek()
        {
            var dailyStepCounts = _context.StepCounts;

            var weekGroups = dailyStepCounts.GroupBy(x => x.CreatedDate.GetWeekStartingOnMonday());
            
            var weeklyStepCounts = new List<StepCount>();
            foreach (var group in weekGroups)
            {
                var stepCount = new StepCount
                {
                    CreatedDate = group.Key,
                    Count = group.Sum(x => x.Count)
                };

                weeklyStepCounts.Add(stepCount);
            }

            return weeklyStepCounts.AsQueryable();
        }

        [Route("odata/StepCounts/GroupByMonth")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<StepCount> GetByMonth()
        {
            var dailySteps = _context.StepCounts.OrderBy(x => x.CreatedDate).ToList();

            var monthGroups = dailySteps.GroupBy(x => x.CreatedDate.GetFirstDayOfMonth());

            var monthlySteps = new List<StepCount>();

            foreach (var group in monthGroups)
            {
                var stepCount = new StepCount
                {
                    CreatedDate = group.Key,
                    Count = (int)group.Average(x => x.Count)
                };

                monthlySteps.Add(stepCount);
            }

            return monthlySteps.AsQueryable();
        }



    }
}
