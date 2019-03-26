using System.Collections.Generic;
using Repositories.Health.Models;
using Services.Withings.Domain;

namespace Services.Withings.Services
{
    public interface IWithingsMapper
    {
        IEnumerable<Weight> MapMeasuresGroupsToWeights(IEnumerable<Response.Measuregrp> weightMeasuresGroups);
        IEnumerable<BloodPressure> MapMeasuresGroupsToBloodPressures(IEnumerable<Response.Measuregrp> bloodPressureMeasuresGroups);
    }
}