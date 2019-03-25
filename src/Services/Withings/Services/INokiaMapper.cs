using System.Collections.Generic;
using Nokia.Domain;
using Repositories.Health.Models;

namespace Nokia.Services
{
    public interface INokiaMapper
    {
        IEnumerable<Weight> MapMeasuresGroupsToWeights(IEnumerable<Response.Measuregrp> weightMeasuresGroups);
        IEnumerable<BloodPressure> MapMeasuresGroupsToBloodPressures(IEnumerable<Response.Measuregrp> bloodPressureMeasuresGroups);
    }
}