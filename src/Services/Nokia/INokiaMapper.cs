using System.Collections.Generic;
using Repositories.Models;
using Services.Nokia.Domain;

namespace Services.Nokia
{
    public interface INokiaMapper
    {
        IEnumerable<Weight> MapMeasuresGroupsToWeights(IEnumerable<Response.Measuregrp> weightMeasuresGroups);
        IEnumerable<BloodPressure> MapMeasuresGroupsToBloodPressures(IEnumerable<Response.Measuregrp> bloodPressureMeasuresGroups);
    }
}