using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Models;

namespace HealthAPI.Controllers.OData
{
    //[Route("api/[controller]")]
    public class AlcoholIntakesController : ODataController
    {
        private readonly HealthContext _context;

        public AlcoholIntakesController(HealthContext context)
        {
            _context = context;
        }

        // GET api/Units
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<AlcoholIntake> Get()
        {
            return _context.AlcoholIntakes.AsQueryable();
        }

        [HttpPost]
        [Route("api/Units/AddMovingAverages")]
        public IActionResult Create([FromBody] AlcoholIntake alcoholIntake)
        {
            try
            {
                if (alcoholIntake == null)
                {
                    return BadRequest();
                }

                var existingItem = _context.AlcoholIntakes.FirstOrDefault(x => x.DateTime == alcoholIntake.DateTime);

                if (existingItem != null)
                {
                    existingItem.DateTime = alcoholIntake.DateTime;
                    existingItem.Units = alcoholIntake.Units;

                    _context.AlcoholIntakes.Update(alcoholIntake);
                }
                else
                {
                    _context.AlcoholIntakes.Add(alcoholIntake);
                }


                _context.SaveChanges();

                //return CreatedAtRoute("GetTodo", weight);
                return Created("/bum", alcoholIntake);
                //return new NoContentResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
