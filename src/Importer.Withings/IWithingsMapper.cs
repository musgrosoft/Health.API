using System.Collections.Generic;
using Importer.Withings.Domain;
using Repositories.Health.Models;

namespace Importer.Withings
{
    public interface IWithingsMapper
    {
        IEnumerable<Weight> MapToWeights(IEnumerable<Response.Measuregrp> weightMeasuresGroups);
        IEnumerable<BloodPressure> MapToBloodPressures(IEnumerable<Response.Measuregrp> bloodPressureMeasuresGroups);
    }
}