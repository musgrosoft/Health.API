using System;
using System.Collections.Generic;
using System.Linq;
using Services.Fitbit;
using Services.Fitbit.Domain;
using Xunit;

namespace Services.Tests.Fitbit
{
    public class FitbitMapperTests
    {
        private FitbitMapper _fitMapper;

        public FitbitMapperTests()
        {
            _fitMapper = new FitbitMapper();
        }

        [Fact]
        public void ShouldMapFromFitbitDailyActivitiesToStepCounts()
        {
            //Given
            IEnumerable<FitbitDailyActivity> fitbitDailyActivities = new List<FitbitDailyActivity>
            {
                new FitbitDailyActivity {DateTime = new DateTime(2018, 1, 1), summary = new Summary {steps = 111}},
                new FitbitDailyActivity {DateTime = new DateTime(2018, 1, 2), summary = new Summary {steps = 222}},
                new FitbitDailyActivity {DateTime = new DateTime(2018, 1, 3), summary = new Summary {steps = 333}}
            };

            //When
            var result = _fitMapper.MapToStepCounts(fitbitDailyActivities);

            //Then
            Assert.Equal(3,result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Count == 111);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Count == 222);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Count == 333);

        }

        //public void ShouldMapFromActivitiesHeartToRestingHeartRates()
        //{

        //}

    }
}