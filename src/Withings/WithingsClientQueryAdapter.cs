using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;
using Withings.Domain.WithingsMeasureGroupResponse;

namespace Withings
{
    public class WithingsClientQueryAdapter : IWithingsClientQueryAdapter
    {
        private readonly IWithingsClient _withingsClient;
        
        public WithingsClientQueryAdapter(IWithingsClient withingsClient)
        {
            _withingsClient = withingsClient;
        }

        public async Task<IEnumerable<Measuregrp>> GetMeasureGroups(DateTime sinceDateTime, string accessToken)
        {
            var measureGroups = await _withingsClient.GetMeasureGroups(accessToken, sinceDateTime);

            var dateFilteredMeasures = measureGroups.Where(x => x.date.ToDateFromUnixTime() >= sinceDateTime);

            return dateFilteredMeasures;

        }
        
    }
}