using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Withings.Domain;
using Utils;

namespace Services.Withings.Services
{
    public class WithingsClientQueryAdapter : IWithingsClientQueryAdapter
    {
        private readonly IWithingsClient _withingsClient;
        
        public WithingsClientQueryAdapter(IWithingsClient withingsClient)
        {
            _withingsClient = withingsClient;
        }

        public async Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime)
        {
            var measureGroups = await _withingsClient.GetMeasureGroups();

            var dateFilteredMeasures = measureGroups.Where(x => x.date.ToDateFromUnixTime() >= sinceDateTime);

            return dateFilteredMeasures;

        }

    }
}