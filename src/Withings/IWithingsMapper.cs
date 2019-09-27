using System.Collections.Generic;
using Repositories.Health.Models;
using Withings.Domain;

namespace Withings
{
    public interface IWithingsMapper
    {
        IEnumerable<Weight> MapToWeights(IEnumerable<Response.Measuregrp> weightMeasuresGroups);
        IEnumerable<BloodPressure> MapToBloodPressures(IEnumerable<Response.Measuregrp> bloodPressureMeasuresGroups);
    }
}