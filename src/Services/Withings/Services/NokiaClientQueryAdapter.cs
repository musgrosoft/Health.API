using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nokia.Domain;
using Utils;

namespace Nokia.Services
{
    public class NokiaClientQueryAdapter : INokiaClientQueryAdapter
    {
        private readonly INokiaClient _nokiaClient;
        
        public NokiaClientQueryAdapter(INokiaClient nokiaClient)
        {
            _nokiaClient = nokiaClient;
        }

        public async Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime)
        {
            var measureGroups = await _nokiaClient.GetMeasureGroups();

            var dateFilteredMeasures = measureGroups.Where(x => x.date.ToDateFromUnixTime() >= sinceDateTime);

            return dateFilteredMeasures;

        }

    }
}