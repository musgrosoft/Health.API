using System;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Models;

namespace HealthAPI.Controllers.OData
{

    [Produces("application/json")]
    public class WeightsController : ODataController
    {
        private readonly HealthContext _context;
        
        public WeightsController(HealthContext context)
        {
            _context = context;            
        }

        //   odata/Weights
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IQueryable<Weight> Get()
        {
            return _context.Weights.AsQueryable();
        }
        
        [HttpGet]
        [Route("api/Weights/HealthyWeight")]
        public Decimal GetHealthyWeight()
        {
            decimal healthyWeight = 88.7M;
            
            return healthyWeight;
        }

        //[HttpPost]
        //[Route("api/Weights")]
        //public IActionResult Create([FromBody] Weight weight)
        //{
        //    try
        //    {
        //        if (weight == null)
        //        {
        //            return BadRequest();
        //        }

        //        var existingItem = _context.Weights.FirstOrDefault(x => x.DateTime == weight.DateTime);

        //        if (existingItem != null)
        //        {
        //            existingItem.Kg = weight.Kg;
        //            existingItem.FatRatioPercentage = weight.FatRatioPercentage;

        //            _context.Weights.Update(existingItem);
        //        }
        //        else
        //        {
        //            _context.Weights.Add(weight);
        //        }


        //        _context.SaveChanges();

        //        //return CreatedAtRoute("GetTodo", weight);
        //        return Created("/bum", weight);
        //        //return new NoContentResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }

        //}


        //[HttpPost]
        //[Route("api/Weights/AddMovingAverages")]
        //public IActionResult AddMovingAverages(int period = 10)
        //{
        //    try
        //    {
        //        var orderedWeights = _context.Weights.OrderBy(x => x.DateTime).ToList();

        //        for (int i = 0; i < orderedWeights.Count(); i++)
        //        {
        //            if (i >= period - 1)
        //            {
        //                decimal total = 0;
        //                for (int x = i; x > (i - period); x--)
        //                    total += orderedWeights[x].Kg;
        //                decimal average = total / period;
        //                // result.Add(series.Keys[i], average);
        //                orderedWeights[i].MovingAverageKg = average;
        //            }
        //            else
        //            {
        //                //weights[i].MovingAverageKg = weights[i].Kg;
        //                orderedWeights[i].MovingAverageKg = null;
        //            }

        //            _context.SaveChanges();

        //        }

        //        return Ok();


        //    }
        //    catch (Exception ex)
        //    {
        //        //return internal error
        //        return BadRequest(ex);
        //    }

        //}

    }
}
