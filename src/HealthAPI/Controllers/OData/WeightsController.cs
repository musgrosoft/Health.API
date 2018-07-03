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


    }
}
