using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;


namespace HealthAPI.ODataControllers
{
    [Produces("application/json")]
    public class BloodPressuresController : ODataController
    {
        private readonly HealthContext _context;

        public BloodPressuresController(HealthContext context)
        {
            _context = context;
        }
        
        // odata/BloodPressures
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Models.BloodPressure> Get()
        {
            return _context.BloodPressures.AsQueryable();
        }
        
        [HttpDelete]
        [Route("api/BloodPressures")]
        public IActionResult Delete(DateTime id)
        {
            try
            {
                var existingItem = _context.BloodPressures.FirstOrDefault(x => x.DateTime == id);
                if (existingItem != null)
                {
                    _context.BloodPressures.Remove(existingItem);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("api/BloodPressures")]
        public IActionResult Create([FromBody] Models.BloodPressure bloodPressure)
        {
            try
            {
                if (bloodPressure == null)
                {
                    return BadRequest();
                }

                var existingItem = _context.BloodPressures.FirstOrDefault(x => x.DateTime == bloodPressure.DateTime);

                if (existingItem != null)
                {
                    existingItem.Diastolic = bloodPressure.Diastolic;
                    existingItem.Systolic = bloodPressure.Systolic;

                    _context.BloodPressures.Update(existingItem);
                }
                else
                {
                    _context.BloodPressures.Add(bloodPressure);
                }


                _context.SaveChanges();

                //return CreatedAtRoute("GetTodo", bloodPressure);
                //return new NoContentResult();
                return Created("/bum", bloodPressure);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }



        [HttpPost]
        [Route("api/BloodPressures/AddMovingAverages")]
        public IActionResult AddMovingAverages(int period = 10)
        {
            try
            {
                var bloodPressures = _context.BloodPressures.OrderBy(x => x.DateTime).ToList(); 

                for (int i = 0; i < bloodPressures.Count(); i++)
                {
                    if (i >= period - 1)
                    {
                        int systolicTotal = 0;
                        int diastolicTotal = 0;
                        for (int x = i; x > (i - period); x--)
                        {
                            systolicTotal += bloodPressures[x].Systolic;
                            diastolicTotal += bloodPressures[x].Diastolic;
                        }
                        int averageSystolic = systolicTotal / period;
                        int averageDiastolic = diastolicTotal / period;
                        // result.Add(series.Keys[i], average);
                        bloodPressures[i].MovingAverageSystolic = averageSystolic;
                        bloodPressures[i].MovingAverageDiastolic = averageDiastolic;
                    }
                    else
                    {
                        //bloodPressures[i].MovingAverageSystolic = bloodPressures[i].Systolic;
                        //bloodPressures[i].MovingAverageDiastolic = bloodPressures[i].Diastolic;
                        bloodPressures[i].MovingAverageSystolic = null;
                        bloodPressures[i].MovingAverageDiastolic = null;
                    }

                    _context.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}