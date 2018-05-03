using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;

namespace Services.Nokia
{
    public interface INokiaClient
    {
        Task<IEnumerable<BloodPressure>> GetBloodPressures(DateTime sinceDateTime);
        Task<IEnumerable<Weight>> GetScaleMeasures(DateTime sinceDateTime);
    }
}