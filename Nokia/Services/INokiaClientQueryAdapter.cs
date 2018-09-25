using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nokia.Domain;

namespace Nokia.Services
{
    public interface INokiaClientQueryAdapter
    {
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(DateTime sinceDateTime);
    }
}