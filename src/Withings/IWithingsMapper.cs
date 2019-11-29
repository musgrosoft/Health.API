using System.Collections.Generic;
using Repositories.Health.Models;
using Withings.Domain;
using Withings.Domain.WithingsMeasureGroupResponse;

namespace Withings
{
    public interface IWithingsMapper
    {
        IEnumerable<Weight> MapToWeights(IEnumerable<Measuregrp> weightMeasuresGroups);
        IEnumerable<BloodPressure> MapToBloodPressures(IEnumerable<Measuregrp> bloodPressureMeasuresGroups);
    }
}