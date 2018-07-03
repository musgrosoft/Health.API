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
    public class HeartRateSummariesController : ODataController
    {
        private readonly HealthContext _context;

        public HeartRateSummariesController(HealthContext context)
        {
            _context = context;
        }

        // GET api/HeartRateSummaries
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<HeartRateSummary> Get()
        {
            return _context.HeartRateSummaries.OrderBy(x=>x.CreatedDate);
        }
        
        [HttpGet]
        [Route("odata/HeartRateSummaries/GroupByWeek")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<HeartRateSummary> GetByWeek()
        {   
            var dailyHeartZones = _context.HeartRateSummaries;

            var weekGroups = dailyHeartZones.GroupBy(x => x.CreatedDate.GetWeekStartingOnMonday());

            var weeklyHeartZones = new List<HeartRateSummary>();
            foreach (var group in weekGroups)
            {
                var heartZone = new HeartRateSummary
                {
                    CreatedDate = group.Key,
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
        [Route("odata/HeartRateSummaries/GroupByMonth")]
        [EnableQuery(AllowedQueryOptions = Microsoft.AspNet.OData.Query.AllowedQueryOptions.All)]
        public IEnumerable<HeartRateSummary> GetByMonth()
        {
            var dailyHearts = _context.HeartRateSummaries.OrderBy(x => x.CreatedDate).ToList();

            var monthGroups = dailyHearts.GroupBy(x => x.CreatedDate.GetFirstDayOfMonth());


            var monthlyHearts = new List<HeartRateSummary>();
            foreach (var group in monthGroups)
            {
                var heart = new HeartRateSummary
                {
                    CreatedDate = group.Key,
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
