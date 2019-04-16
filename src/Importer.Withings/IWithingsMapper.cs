using System.Collections.Generic;
using Importer.Withings.Domain;
using Repositories.Health.Models;

namespace Importer.Withings
{
    public interface IWithingsMapper
    {
        IEnumerable<Weight> MapMeasuresGroupsToWeights(IEnumerable<Response.Measuregrp> weightMeasuresGroups);
        IEnumerable<BloodPressure> MapMeasuresGroupsToBloodPressures(IEnumerable<Response.Measuregrp> bloodPressureMeasuresGroups);
    }
}