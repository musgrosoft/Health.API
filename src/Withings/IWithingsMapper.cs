using System.Collections.Generic;
using Repositories.Health.Models;
using Withings.Domain;

namespace Withings
{
    public interface IWithingsMapper
    {
        IEnumerable<Weight> MapToWeights(IEnumerable<WithingsMeasureGroupResponse.Measuregrp> weightMeasuresGroups);
        IEnumerable<BloodPressure> MapToBloodPressures(IEnumerable<WithingsMeasureGroupResponse.Measuregrp> bloodPressureMeasuresGroups);
    }
}