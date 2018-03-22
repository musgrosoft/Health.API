using System;
using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using HealthAPI.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
   // [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    public class WeightsController : Controller
    {
        private readonly HealthContext _context;
        
        public WeightsController(HealthContext context)
        {
            _context = context;
            
        }


        // GET api/weights
        //[EnableCors("CorsPolicy")]
        [HttpGet]
        //[Route("api/weightsx")]
        public IEnumerable<ViewModels.Weight> Get()
        {
            List<ViewModels.Weight> weights = _context.Weights.Select(w => new ViewModels.Weight
            {
                Kg = w.WeightKg,
                DateTime = w.DateTime
            }).OrderBy(x => x.DateTime).ToList();

            AddMovingAverages(weights, 10);

            return weights;//.OrderBy(x=>x.DateTime);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Models.Weight weight)
        {
            try
            {
                if (weight == null)
                {
                    return BadRequest();
                }

                var existingItem = _context.Weights.FirstOrDefault(x => x.DateTime == weight.DateTime);

                if (existingItem != null)
                {
                    existingItem.WeightKg = weight.WeightKg;
                    existingItem.FatRatioPercentage = weight.FatRatioPercentage;

                    _context.Weights.Update(existingItem);
                }
                else
                {
                    _context.Weights.Add(weight);
                }


                _context.SaveChanges();

                return CreatedAtRoute("GetTodo", weight);
                //return new NoContentResult();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        private void AddMovingAverages(List<ViewModels.Weight> weights, int period)
        {
            for (int i = 0; i < weights.Count(); i++)
            {
                if (i >= period - 1)
                {
                    decimal total = 0;
                    for (int x = i; x > (i - period); x--)
                        total += weights[x].Kg;
                    decimal average = total / period;
                    // result.Add(series.Keys[i], average);
                    weights[i].MovingAverageKg = average;
                }
                else
                {
                    //weights[i].MovingAverageKg = weights[i].Kg;
                    weights[i].MovingAverageKg = null;
                }

            }
        }


    }
}
