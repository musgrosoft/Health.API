using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Importer.Withings.Domain;

namespace Importer.Withings
{
    public interface IWithingsClientQueryAdapter
    {
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime, string accessToken);
        Task<IEnumerable<Series>> GetSleepSeries(DateTime sinceDateTime, string accessToken);
    }
}