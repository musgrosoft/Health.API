using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using Withings.Domain;
using Withings.Domain.WithingsMeasureGroupResponse;
using Xunit;

namespace Withings.Tests.Unit
{
    public class WithingsMapperTests
    {
        private const int WeightKgMeasureTypeId = 1;
        private const int FatRatioPercentageMeasureTypeId = 6;
        private const int DiastolicBloodPressureMeasureTypeId = 9;
        private const int SystolicBloodPressureMeasureTypeId = 10;

        private WithingsMapper _withingsMapper;

        public WithingsMapperTests()
        {
            _withingsMapper = new WithingsMapper();
        }

        [Fact]
        public void ShouldMapMeasuresGroupsToWeights()
        {
            var measuresGroups = new List<Measuregrp>
            {
                new Measuregrp {date = (int)(new DateTime(2018,1,1).ToUnixTimeFromDate()), measures = new List<Measure>
                {
                    new Measure {type = WeightKgMeasureTypeId, unit = 0, value = 90},
                    new Measure {type = FatRatioPercentageMeasureTypeId, unit = 0, value = 80},
                }},
                new Measuregrp {date = (int)(new DateTime(2018,1,2).ToUnixTimeFromDate()), measures = new List<Measure>
                {
                    new Measure {type = WeightKgMeasureTypeId, unit = 0, value = 91}
                }},
                new Measuregrp {date = (int)(new DateTime(2018,1,3).ToUnixTimeFromDate()), measures = new List<Measure>
                {
                    new Measure {type = WeightKgMeasureTypeId, unit = 0, value = 92}
                }}
            };

            var result = _withingsMapper.MapToWeights(measuresGroups);

            //Then
            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Kg == 90 && x.FatRatioPercentage == 80);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Kg == 91);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Kg == 92);
        }

        [Fact]
        public void ShouldMapMeasuresGroupsToBloodPressures()
        {
            var measuresGroups = new List<Measuregrp>
            {
                new Measuregrp {date = (int)(new DateTime(2018,1,1).ToUnixTimeFromDate()), measures = new List<Measure>
                {
                    new Measure {type = DiastolicBloodPressureMeasureTypeId, unit = 0, value = 71},
                    new Measure {type = SystolicBloodPressureMeasureTypeId, unit = 0, value = 131},
                }},
                new Measuregrp {date = (int)(new DateTime(2018,1,2).ToUnixTimeFromDate()), measures = new List<Measure>
                {
                    new Measure {type = DiastolicBloodPressureMeasureTypeId, unit = 0, value = 72},
                    new Measure {type = SystolicBloodPressureMeasureTypeId, unit = 0, value = 132},
                }},
                new Measuregrp {date = (int)(new DateTime(2018,1,3).ToUnixTimeFromDate()), measures = new List<Measure>
                {
                    new Measure {type = DiastolicBloodPressureMeasureTypeId, unit = 0, value = 73},
                    new Measure {type = SystolicBloodPressureMeasureTypeId, unit = 0, value = 133},
                }}
            };

            var result = _withingsMapper.MapToBloodPressures(measuresGroups);

            //Then
            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Diastolic == 71 && x.Systolic == 131);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Diastolic == 72 && x.Systolic == 132);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Diastolic == 73 && x.Systolic == 133);
        }
    }
}