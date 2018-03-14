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

        
    }
}
