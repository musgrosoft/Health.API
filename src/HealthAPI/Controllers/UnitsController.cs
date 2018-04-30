using System;
using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    //[Route("api/[controller]")]
    public class UnitsController : ODataController
    {
        private readonly HealthContext _context;

        public UnitsController(HealthContext context)
        {
            _context = context;
        }

        // GET api/Units
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<Units> Get()
        {
            return _context.Units.OrderBy(x=>x.DateTime).ToList();
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        [Route("api/Units/GroupByWeek")]
        public IEnumerable<Units> GetByWeek()
        {
            var dailyUnits = _context.Units.OrderBy(x => x.DateTime).ToList();

            var groups = dailyUnits.GroupBy(x => x.Week);
            
            var weeklyUnits = new List<Units>();
            foreach (var group in groups)
            {
                var unit = new Units
                {
                    DateTime = group.Key,
                    Units1 = group.Sum(x => x.Units1)
                };

                weeklyUnits.Add(unit);
            }

            return weeklyUnits;
        }

        [HttpPost]
        [Route("api/Units/AddMovingAverages")]
        public IActionResult Create([FromBody] Models.Units dailyUnits)
        {
            try
            {
                if (dailyUnits == null)
                {
                    return BadRequest();
                }

                var existingItem = _context.Units.FirstOrDefault(x => x.DateTime == dailyUnits.DateTime);

                if (existingItem != null)
                {
                    existingItem.DateTime = dailyUnits.DateTime;
                    existingItem.Units1 = dailyUnits.Units1;

                    _context.Units.Update(dailyUnits);
                }
                else
                {
                    _context.Units.Add(dailyUnits);
                }


                _context.SaveChanges();

                //return CreatedAtRoute("GetTodo", weight);
                return Created("/bum", dailyUnits);
                //return new NoContentResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
