using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class StepsController : Controller
    {
        private readonly HealthContext _context;

        public StepsController(HealthContext context)
        {
            _context = context;
        }

        // GET api/bloodpressures
        [HttpGet]
        // public IEnumerable<DailySteps> Get([FromUri] string groupBy)
        public IEnumerable<DailySteps> Get(string groupBy = "day")
        {
            //var dailySteps = _context.DailySteps.OrderBy(x => x.DateTime);

            //if (groupBy.ToLower() == "week")
            //{
            //    var weekGroups = dailySteps.GroupBy(x => x.DateTime.AddDays(-(int)x.DateTime.DayOfWeek));


            //    var weeklyStepCounts = new List<StepCount>();
            //    foreach (var group in weekGroups)
            //    {
            //        var stepCount = new StepCount
            //        {
            //            Day = group.Key,
            //            Steps = group.Sum(x => x.Steps)
            //        };

            //        weeklyStepCounts.Add(stepCount);
            //    }

            //    return weeklyStepCounts;
            //}

            return _context.DailySteps.OrderBy(x=>x.DateTime);

            //------------------------------------------------------------------------------------------

            //var dailyStepCounts = GetDailyStepCounts();

            //var weekGroups = dailyStepCounts.GroupBy(x => x.Day.AddDays(-(int)x.Day.DayOfWeek));


            //var weeklyStepCounts = new List<StepCount>();
            //foreach (var group in weekGroups)
            //{
            //    var stepCount = new StepCount
            //    {
            //        Day = group.Key,
            //        Steps = group.Sum(x => x.Steps)
            //    };

            //    weeklyStepCounts.Add(stepCount);
            //}

            //return weeklyStepCounts;


        }

        
    }
}
