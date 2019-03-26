using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Withings.Domain;

namespace Services.Withings.Services
{
    public interface INokiaClientQueryAdapter
    {
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime);
    }
}