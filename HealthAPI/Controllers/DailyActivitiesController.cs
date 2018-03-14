using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
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
        public IEnumerable<DailyActivitySummaries> Get()
        {
            return _context.DailyActivitySummaries.OrderBy(x=>x.DateTime);
        }

        
    }
}
