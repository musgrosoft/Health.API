﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Models;

namespace HealthAPI.Controllers.OData
{
    //[Route("api/[controller]")]
    public class StepCountsController : ODataController
    {
        private readonly HealthContext _context;

        public StepCountsController(HealthContext context)
        {
            _context = context;
        }

        // GET api/Stepsx
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

            var weekGroups = dailyStepCounts.GroupBy(x => x.Week);
            
            var weeklyStepCounts = new List<StepCount>();
            foreach (var group in weekGroups)
            {
                var stepCount = new StepCount
                {
                    DateTime = group.Key,
                    Count = group.Sum(x => x.Count)
                };

                weeklyStepCounts.Add(stepCount);
            }

            return weeklyStepCounts.AsQueryable();
        }


    }
}