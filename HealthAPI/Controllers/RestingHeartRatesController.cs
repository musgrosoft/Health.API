using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class RestingHeartRatesController : Controller
    {
        private readonly HealthContext _context;

        public RestingHeartRatesController(HealthContext context)
        {
            _context = context;
        }

        // GET api/bloodpressures
        [HttpGet]
        public IEnumerable<DailySteps> Get()
        {
            return _context.DailySteps.OrderBy(x=>x.DateTime);
        }

        
    }
}
