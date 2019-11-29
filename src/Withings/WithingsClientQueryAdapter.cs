using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;
using Withings.Domain;

namespace Withings
{
    public class WithingsClientQueryAdapter : IWithingsClientQueryAdapter
    {
        private readonly IWithingsClient _withingsClient;
        
        public WithingsClientQueryAdapter(IWithingsClient withingsClient)
        {
            _withingsClient = withingsClient;
        }

        public async Task<IEnumerable<WithingsMeasureGroupResponse.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime, string accessToken)
        {
            var measureGroups = await _withingsClient.GetMeasureGroups(accessToken);

            var dateFilteredMeasures = measureGroups.Where(x => x.date.ToDateFromUnixTime() >= sinceDateTime);

            return dateFilteredMeasures;

        }

//        public async Task<IEnumerable<Series>> GetSleepSeries(DateTime sinceDateTime, string accessToken)
//        {
//            var allTheSleeps = new List<Series>();
//
//            for (DateTime date = sinceDateTime;
//                date <= DateTime.Now;
//                date = date.AddDays(7))
//            {
//                var series = await _withingsClient.Get7DaysOfSleeps(accessToken, date);
//                
//                allTheSleeps.AddRange(series);
//            }
//
//            return allTheSleeps;
//        }


    }
}