using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<RestingHeartRate> Get()
        {
            return _context.RestingHeartRate.OrderBy(x=>x.DateTime);
        }

        
    }
}
