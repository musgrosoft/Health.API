using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System;
using System.Collections.Generic;

namespace HealthAPI.Controllers
{

    [Produces("application/json")]
 //   [Route("odata/Weights")]
    [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
    public class WeightsController : ODataController
    {
        private readonly HealthContext _context;
        
        public WeightsController(HealthContext context)
        {
            _context = context;
            
        }

        [HttpGet]
      //  [EnableQuery]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Models.Weight> Get()
        {
            return _context.Weights.AsQueryable();
        }

        //       

        [HttpGet]
        //[Route("api/weightsx")]
        [Route("odata/Weights/LastWeight")]
        public ViewModels.Weight GetLastWeight()
        {
            var weights = GetMovingAverages();
            var lastWeight = weights.OrderByDescending(x => x.DateTime).FirstOrDefault();
            return lastWeight;
        }

        [HttpGet]
        //[Route("api/weightsx")]
        [Route("odata/Weights/LastHealthyWeight")]
        public ViewModels.Weight GetLastHealthyWeight()
        {
            decimal healthyWeight = 88.7M;

            var weights = GetMovingAverages();
            var lastHealthyWeight = weights.OrderByDescending(x => x.DateTime).FirstOrDefault(x => x.MovingAverageKg < healthyWeight);
            return lastHealthyWeight;
        }

        [HttpGet]
        //[Route("api/weightsx")]
        [Route("odata/Weights/HealthyWeight")]
        public Decimal GetHealthyWeight()
        {
            decimal healthyWeight = 88.7M;
            
            return healthyWeight;
        }

        [HttpGet]
        //[Route("api/weightsx")]
        [Route("odata/Weights/WithMovingAverages")]
        public IEnumerable<ViewModels.Weight> GetMovingAverages()
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

                //return CreatedAtRoute("GetTodo", weight);
                return Created("/bum", weight);
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
