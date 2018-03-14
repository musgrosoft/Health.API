using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class UnitsController : Controller
    {
        private readonly HealthContext _context;

        public UnitsController(HealthContext context)
        {
            _context = context;
        }

        // GET api/Units
        [HttpGet]
        public IEnumerable<Units> Get()
        {
            return _context.Units.OrderBy(x=>x.DateTime).ToList();
        }

        
    }
}
