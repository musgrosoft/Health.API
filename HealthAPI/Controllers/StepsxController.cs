using System;
using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using HealthAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class StepsxController : Controller
    {
        private readonly HealthContext _context;

        public StepsxController(HealthContext context)
        {
            _context = context;
        }

        // GET api/Stepsx
        [HttpGet]
        public IEnumerable<StepCount> Get(string groupBy = "day")
        {
            var dailyStepCounts = _context.DailySteps.OrderBy(x => x.DateTime).Select(x=>new StepCount
            {
                DateTime = x.DateTime,
                Steps = x.Steps
            });

            if (groupBy.ToLower() == "week")
            {
                var weekGroups = dailyStepCounts.GroupBy(x => x.DateTime.AddDays(-(int)x.DateTime.DayOfWeek));


                var weeklyStepCounts = new List<StepCount>();
                foreach (var group in weekGroups)
                {
                    var stepCount = new StepCount
                    {
                        DateTime = group.Key,
                        Steps = group.Sum(x => x.Steps)
                    };

                    weeklyStepCounts.Add(stepCount);
                }

                return weeklyStepCounts;
            }

            return dailyStepCounts;

            // return _context.DailySteps.OrderBy(x=>x.DateTime);

            //------------------------------------------------------------------------------------------

            //var dailyStepCounts = GetDailyStepCounts();

            //var weekGroups = dailyStepCounts.GroupBy(x => x.DateTime.AddDays(-(int)x.DateTime.DayOfWeek));


            //var weeklyStepCounts = new List<StepCount>();
            //foreach (var group in weekGroups)
            //{
            //    var stepCount = new StepCount
            //    {
            //        DateTime = group.Key,
            //        Steps = group.Sum(x => x.Steps)
            //    };

            //    weeklyStepCounts.Add(stepCount);
            //}

            //return weeklyStepCounts;


        }


        [HttpPost]
        public IActionResult Create([FromBody] Models.DailySteps dailySteps)
        {
            try
            {
                if (dailySteps == null)
                {
                    return BadRequest();
                }

                var existingItem = _context.DailySteps.FirstOrDefault(x => x.DateTime == dailySteps.DateTime);

                if (existingItem != null)
                {
                    existingItem.DateTime = dailySteps.DateTime;
                    existingItem.Steps = dailySteps.Steps;

                    _context.DailySteps.Update(existingItem);
                }
                else
                {
                    _context.DailySteps.Add(dailySteps);
                }


                _context.SaveChanges();

                //return CreatedAtRoute("GetTodo", weight);
                return Created("/bum", dailySteps);
                //return new NoContentResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
