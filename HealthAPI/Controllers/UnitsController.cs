using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            //if (_context.TodoItems.Count() == 0)
            //{
            //    _context.TodoItems.Add(new TodoItem { Name = "Item1" });
            //    _context.SaveChanges();
            //}
        }

        // GET api/weights
        [HttpGet]
        public IEnumerable<Units> Get()
        {
            return _context.Units.OrderBy(x=>x.DateTime).ToList();
        }

        
    }
}
