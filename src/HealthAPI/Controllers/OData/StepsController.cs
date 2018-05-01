using System;
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
        [Route("api/Steps/GroupByWeek")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<StepCount> GetByWeek()
        {
            var dailyStepCounts = _context.StepCounts;

            var weekGroups = dailyStepCounts.GroupBy(x => x.DateTime.AddDays(-(int)x.DateTime.DayOfWeek));


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

        [HttpPost]
        [Route("api/Steps")]
        public IActionResult Create([FromBody] StepCount stepCount)
        {
            try
            {
                if (stepCount == null)
                {
                    return BadRequest();
                }

                var existingItem = _context.StepCounts.FirstOrDefault(x => x.DateTime == stepCount.DateTime);

                if (existingItem != null)
                {
                    existingItem.DateTime = stepCount.DateTime;
                    existingItem.Count = stepCount.Count;

                    _context.StepCounts.Update(existingItem);
                }
                else
                {
                    _context.StepCounts.Add(stepCount);
                }


                _context.SaveChanges();

                //return CreatedAtRoute("GetTodo", weight);
                return Created("/bum", stepCount);
                //return new NoContentResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
