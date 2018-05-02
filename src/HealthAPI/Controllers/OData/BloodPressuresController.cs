using System;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Models;

namespace HealthAPI.Controllers.OData
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
        public IQueryable<BloodPressure> Get()
        {
            return _context.BloodPressures.AsQueryable();
        }

    }
}