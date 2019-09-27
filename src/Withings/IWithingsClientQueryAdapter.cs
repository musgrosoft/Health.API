using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Withings.Domain;

namespace Withings
{
    public interface IWithingsClientQueryAdapter
    {
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime, string accessToken);
        Task<IEnumerable<Series>> GetSleepSeries(DateTime sinceDateTime, string accessToken);
    }
}