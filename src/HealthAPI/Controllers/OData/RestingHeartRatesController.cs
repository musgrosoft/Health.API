using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Models;

namespace HealthAPI.Controllers.OData
{
    [Produces("application/json")]
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
        





    }
}
