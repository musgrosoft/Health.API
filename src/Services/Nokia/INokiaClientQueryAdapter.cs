using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Nokia.Domain;

namespace Services.Nokia
{
    public interface INokiaClientQueryAdapter
    {
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime);
    }
}