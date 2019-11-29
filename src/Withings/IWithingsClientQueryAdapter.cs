using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Withings.Domain;
using Withings.Domain.WithingsMeasureGroupResponse;

namespace Withings
{
    public interface IWithingsClientQueryAdapter
    {
        Task<IEnumerable<Measuregrp>> GetMeasureGroups(DateTime sinceDateTime, string accessToken);
//        Task<IEnumerable<Series>> GetSleepSeries(DateTime sinceDateTime, string accessToken);
    }
}