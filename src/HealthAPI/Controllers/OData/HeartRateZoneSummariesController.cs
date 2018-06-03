using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.Models;
using Utils;

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
        public IEnumerable<HeartSummary> Get()
        {
            return _context.HeartSummaries.OrderBy(x=>x.DateTime);
        }

        // GET api/HeartRateDailySummaries
        [HttpGet]
        [Route("odata/HeartRateDailySummaries/GroupByWeek")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<HeartSummary> GetByWeek()
        {   
            var dailyHeartZones = _context.HeartSummaries;

            var weekGroups = dailyHeartZones.GroupBy(x => x.DateTime.GetWeekStartingOnMonday());

            var weeklyHeartZones = new List<HeartSummary>();
            foreach (var group in weekGroups)
            {
                var heartZone = new HeartSummary
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


        [HttpGet]
        [Route("odata/HeartRateDailySummaries/GroupByMonth")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<HeartSummary> GetByMonth()
        {
            var dailyHearts = _context.HeartSummaries.OrderBy(x => x.DateTime).ToList();

            var monthGroups = dailyHearts.GroupBy(x => x.DateTime.GetFirstDayOfMonth());


            var monthlyHearts = new List<HeartSummary>();
            foreach (var group in monthGroups)
            {
                var heart = new HeartSummary
                {
                    DateTime = group.Key,
                    FatBurnMinutes = (int)group.Average(x => x.FatBurnMinutes),
                    CardioMinutes = (int)group.Average(x => x.CardioMinutes),
                    PeakMinutes = (int)group.Average(x => x.PeakMinutes)
                };

                monthlyHearts.Add(heart);
            }

            return monthlyHearts.AsQueryable();
        }



    }
}
