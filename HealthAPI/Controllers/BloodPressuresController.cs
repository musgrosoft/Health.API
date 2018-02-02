using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class BloodPressuresController : Controller
    {
        private readonly HealthContext _context;

        public BloodPressuresController(HealthContext context)
        {
            _context = context;
        }

        // GET api/bloodpressures
        [HttpGet]
        public IEnumerable<BloodPressures> Get()
        {
            return _context.BloodPressures.OrderBy(x=>x.DateTime);
        }

        
    }
}
