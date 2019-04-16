using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Withings.Domain;

namespace Services.Withings
{
    public interface IWithingsClient
    {
        Task Subscribe();
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups();
        Task<string> GetWeightSubscription();
        Task<string> GetBloodPressureSubscription();
    }
}