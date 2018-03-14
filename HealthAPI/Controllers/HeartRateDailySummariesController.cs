using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class HeartRateDailySummariesController : Controller
    {
        private readonly HealthContext _context;

        public HeartRateDailySummariesController(HealthContext context)
        {
            _context = context;
        }

        // GET api/HeartRateDailySummaries
        [HttpGet]
        public IEnumerable<HeartRateDailySummaries> Get()
        {
            return _context.HeartRateDailySummaries.OrderBy(x=>x.DateTime);
        }

        
    }
}
