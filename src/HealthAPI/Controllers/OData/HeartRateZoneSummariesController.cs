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
        [Route("odata/HeartRateDailySummaries/GroupByWeek")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<HeartRateZoneSummary> GetByWeek()
        {
            return _context.HeartRateDailySummaries.OrderBy(x => x.DateTime);
            
            var dailyHeartZones = _context.HeartRateDailySummaries;

            var weekGroups = dailyHeartZones.GroupBy(x => x.Week);

            var weeklyHeartZones = new List<HeartRateZoneSummary>();
            foreach (var group in weekGroups)
            {
                var heartZone = new HeartRateZoneSummary
                {
                    DateTime = group.Key,
                    OutOfRangeMinutes = group.Sum(x => x.OutOfRangeMinutes),
                    FatBurnMinutes = group.Sum(x => x.FatBurnMinutes),
                    CardioMinutes = group.Sum(x => x.CardioMinutes),
                    PeakMinutes = group.Sum(x => x.PeakMinutes)

                };

                weeklyHeartZones.Add(heartZone);
            }

            return weeklyHeartZones.AsQueryable();
        }


    }
}
