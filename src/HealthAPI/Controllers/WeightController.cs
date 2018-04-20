using System;
using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using HealthAPI.ViewModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{

    [Produces("application/json")]
 //   [Route("odata/Weights")]
    [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
    public class WeightController : ODataController
    {
        private readonly HealthContext _context;
        
        public WeightController(HealthContext context)
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


    }
}
