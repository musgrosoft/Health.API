using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Models;

namespace HealthAPI.Controllers.OData
{
    //[Route("api/[controller]")]
    public class HeartRateZoneSummariesController : ODataController
    {
        private readonly HealthContext _context;

        public HeartRateZoneSummariesController(HealthContext context)
        {
            _context = context;
        }

        // GET api/HeartRateDailySummaries
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<HeartRateZoneSummary> Get()
        {
            return _context.HeartRateDailySummaries.OrderBy(x=>x.DateTime);
        }

        // GET api/HeartRateDailySummaries
        [HttpGet]
        [Route("api/HeartRateDailySummaries/GroupByWeek")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<HeartRateZoneSummary> GetByWeek()
        {
            return _context.HeartRateDailySummaries.OrderBy(x => x.DateTime);
        }


    }
}
