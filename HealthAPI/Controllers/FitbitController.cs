using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class FitbitController : Controller
    {
        private readonly HealthContext _context;

        public FitbitController(HealthContext context)
        {
            _context = context;

            //if (_context.TodoItems.Count() == 0)
            //{
            //    _context.TodoItems.Add(new TodoItem { Name = "Item1" });
            //    _context.SaveChanges();
            //}
        }

        // GET api/fitbit
        [HttpGet]
        public IEnumerable<Weights> Get()
        {
            return _context.Weights.OrderBy(x=>x.DateTime);
        }

        // GET api/values/5
        [HttpGet("/weights")]
        public IEnumerable<Weights> GetWeights()
        {
            return _context.Weights.OrderBy(x => x.DateTime);
        }
    }
}
