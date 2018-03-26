using System;
using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using HealthAPI.ViewModels;
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
        public IEnumerable<ViewModels.BloodPressure> Get()
        {
            var bloodPressures = _context.BloodPressures.OrderBy(x => x.DateTime).Select(x=> new ViewModels.BloodPressure { 
                
                DateTime = x.DateTime,
                Systolic = x.Systolic.Value,
                Diastolic = x.Diastolic.Value

                }).ToList();

            AddMovingAverages(bloodPressures, 10);

            return bloodPressures;
        }

        [HttpPost]
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

        //[HttpPut("{id}")]
        //public IActionResult Update(long id, [FromBody] TodoItem item)
        //{
        //    if (item == null || item.Id != id)
        //    {
        //        return BadRequest();
        //    }

        //    var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }

        //    todo.IsComplete = item.IsComplete;
        //    todo.Name = item.Name;

        //    _context.TodoItems.Update(todo);
        //    _context.SaveChanges();
        //    return new NoContentResult();
        //}

        public void AddMovingAverages(List<ViewModels.BloodPressure> bloodPressures, int period)
        {
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

            }
        }

    }
}
