using System;
using System.Collections.Generic;
using System.Linq;
using Nokia.Domain;
using Repositories.Health.Models;
using Utils;

namespace Nokia.Services
{
    public class NokiaMapper : INokiaMapper
    {

        private const int WeightKgMeasureTypeId = 1;
        private const int FatRatioPercentageMeasureTypeId = 6;
        private const int DiastolicBloodPressureMeasureTypeId = 9;
        private const int SystolicBloodPressureMeasureTypeId = 10;

        public IEnumerable<Weight> MapMeasuresGroupsToWeights(IEnumerable<Response.Measuregrp> weightMeasuresGroups)
        {
            return weightMeasuresGroups
                .Where(x=>x.measures.Any(y=>y.type == WeightKgMeasureTypeId))
                .Select(x => new Weight
            {
                CreatedDate = x.date.ToDateFromUnixTime(),
                Kg = x.measures.First(w => w.type == WeightKgMeasureTypeId).value * Math.Pow(10, x.measures.First(w => w.type == WeightKgMeasureTypeId).unit),

                //set if available
                FatRatioPercentage =
                    x.measures.FirstOrDefault(w => w.type == FatRatioPercentageMeasureTypeId) == null ?
                    (double?)(x.measures.First(w => w.type == FatRatioPercentageMeasureTypeId).value * Math.Pow(10, x.measures.First(w => w.type == FatRatioPercentageMeasureTypeId).unit)) :
                    null
            });
        }

        public IEnumerable<BloodPressure> MapMeasuresGroupsToBloodPressures(IEnumerable<Response.Measuregrp> bloodPressureMeasuresGroups)
        {
            return bloodPressureMeasuresGroups
                .Where(x => x.measures.Any(y => y.type == DiastolicBloodPressureMeasureTypeId))
                .Select(x => new BloodPressure
            {
                CreatedDate = x.date.ToDateFromUnixTime(),
                Diastolic = (int)(x.measures.First(bp => bp.type == DiastolicBloodPressureMeasureTypeId).value * Math.Pow(10, x.measures.First(bp => bp.type == DiastolicBloodPressureMeasureTypeId).unit)),
                Systolic = (int)(x.measures.First(bp => bp.type == SystolicBloodPressureMeasureTypeId).value * Math.Pow(10, x.measures.First(bp => bp.type == SystolicBloodPressureMeasureTypeId).unit)),
            });
        }
    }
}