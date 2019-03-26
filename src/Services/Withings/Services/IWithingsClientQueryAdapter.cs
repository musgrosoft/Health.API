using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Withings.Domain;

namespace Services.Withings.Services
{
    public interface IWithingsClientQueryAdapter
    {
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime);
    }
}