using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Importer.Withings.Domain;
using Utils;

namespace Importer.Withings
{
    public class WithingsClientQueryAdapter : IWithingsClientQueryAdapter
    {
        private readonly IWithingsClient _withingsClient;
        
        public WithingsClientQueryAdapter(IWithingsClient withingsClient)
        {
            _withingsClient = withingsClient;
        }

        public async Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime, string accessToken)
        {
            var measureGroups = await _withingsClient.GetMeasureGroups(accessToken);

            var dateFilteredMeasures = measureGroups.Where(x => x.date.ToDateFromUnixTime() >= sinceDateTime);

            return dateFilteredMeasures;

        }

    }
}