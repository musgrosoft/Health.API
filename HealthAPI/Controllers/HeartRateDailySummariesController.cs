using System;
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

        [HttpPost]
        public IActionResult Create([FromBody] Models.HeartRateDailySummaries heartRateDailySummaries)
        {
            try
            {
                if (heartRateDailySummaries == null)
                {
                    return BadRequest();
                }

                var existingItem = _context.HeartRateDailySummaries.FirstOrDefault(x => x.DateTime == heartRateDailySummaries.DateTime);

                if (existingItem != null)
                {
                    existingItem.DateTime = heartRateDailySummaries.DateTime;
                    existingItem.DataSource = heartRateDailySummaries.DataSource;
                    existingItem.RestingHeartRate = heartRateDailySummaries.RestingHeartRate;
                    existingItem.OutOfRangeMinutes = heartRateDailySummaries.OutOfRangeMinutes;
                    existingItem.FatBurnMinutes = heartRateDailySummaries.FatBurnMinutes;
                    existingItem.CardioMinutes = heartRateDailySummaries.CardioMinutes;
                    existingItem.PeakMinutes = heartRateDailySummaries.PeakMinutes;
                    

                    _context.HeartRateDailySummaries.Update(existingItem);
                }
                else
                {
                    _context.HeartRateDailySummaries.Add(heartRateDailySummaries);
                }


                _context.SaveChanges();

                //return CreatedAtRoute("GetTodo", weight);
                return Created("/bum", heartRateDailySummaries);
                //return new NoContentResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
