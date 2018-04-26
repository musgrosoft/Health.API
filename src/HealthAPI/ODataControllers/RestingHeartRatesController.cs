using HealthAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNet.OData;

namespace HealthAPI.Controllers
{
   // [Route("api/[controller]")]
    public class RestingHeartRatesController : ODataController
    {
        private readonly HealthContext _context;

        public RestingHeartRatesController(HealthContext context)
        {
            _context = context;
        }

        // GET api/RestingHeartRates
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<RestingHeartRate> Get()
        {
            return _context.RestingHeartRates.AsQueryable();
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] RestingHeartRate restingHeartRate)
        {
            try
            {
                if (restingHeartRate == null)
                {
                    return BadRequest();
                }

                var existingItem = _context.RestingHeartRates.FirstOrDefault(x => x.DateTime == restingHeartRate.DateTime);

                if (existingItem != null)
                {
                    existingItem.DateTime = restingHeartRate.DateTime;
                    existingItem.Beats = restingHeartRate.Beats;

                    _context.RestingHeartRates.Update(existingItem);
                }
                else
                {
                    _context.RestingHeartRates.Add(restingHeartRate);
                }


                _context.SaveChanges();

                //return CreatedAtRoute("GetTodo", weight);
                return Created("/bum", restingHeartRate);
                //return new NoContentResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }




        [HttpPost]
        [Route("odata/RestingHeartRates/AddMovingAverages")]
        public IActionResult AddMovingAverages(int period = 10)
        {
            try
            {
                var heartRates = _context.RestingHeartRates.OrderBy(x => x.DateTime).ToList();

                for (int i = 0; i < heartRates.Count(); i++)
                {
                    if (i >= period - 1)
                    {
                        decimal total = 0;
                        for (int x = i; x > (i - period); x--)
                            total += heartRates[x].Beats;
                        decimal average = total / period;
                        // result.Add(series.Keys[i], average);
                        heartRates[i].MovingAverageBeats = average;
                    }
                    else
                    {
                        //weights[i].MovingAverageKg = weights[i].Kg;
                        heartRates[i].MovingAverageBeats = null;
                    }

                    _context.SaveChanges();

                }

                return Ok();


            }
            catch (Exception ex)
            {
                //return internal error
                return BadRequest(ex);
            }

        }


    }
}
