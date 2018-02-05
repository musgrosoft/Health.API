using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthAPI.Models;
using HealthAPI.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [EnableCors("CorsPolicy")]
   // [Route("api/[controller]")]
    public class WeightsController : Controller
    {
        private readonly HealthContext _context;

        public WeightsController(HealthContext context)
        {
            _context = context;

            //if (_context.TodoItems.Count() == 0)
            //{
            //    _context.TodoItems.Add(new TodoItem { Name = "Item1" });
            //    _context.SaveChanges();
            //}
        }

        // GET api/weights
        [EnableCors("CorsPolicy")]
        [HttpGet]
        [Route("api/weightsx")]
        public IEnumerable<Weight> Get()
        {
            List<Weight> weights = _context.Weights.Select(w => new Weight
            {
                Kg = w.WeightKg,
                DateTime = w.DateTime
            }).OrderBy(x => x.DateTime).ToList();

            AddMovingAverages(weights, 10);

            return weights;//.OrderBy(x=>x.DateTime);
        }

        private void AddMovingAverages(List<Weight> weights, int period)
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
