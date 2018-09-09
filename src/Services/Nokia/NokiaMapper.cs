using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Models;
using Services.Fitbit.Domain;
using Services.Nokia.Domain;
using Utils;

namespace Services.Nokia
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

                //todo set if available
                // FatRatioPercentage = (Decimal)(weightMeasure.measures.FirstOrDefault(x => x.type == FatRatioPercentageMeasureTypeId).value * Math.Pow(10, weightMeasure.measures.FirstOrDefault(x => x.type == FatRatioPercentageMeasureTypeId).unit)),
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