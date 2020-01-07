using System;
using System.Collections.Generic;
using System.Linq;
using Fitbit.Domain;
using Xunit;

namespace Fitbit.Tests.Unit
{
    public class FitbitMapperTests
    {
        private FitbitMapper _fitbitMapper;

        public FitbitMapperTests()
        {
            _fitbitMapper = new FitbitMapper();
        }

        [Fact]
        public void ShouldMapActivitiesHeartsToRestingHeartRates()
        {
            var activitiesHeart = new List<ActivitiesHeart>
                    {
                        new ActivitiesHeart {dateTime = new DateTime(2018,1,1),value = new Value {restingHeartRate = 51}},
                        new ActivitiesHeart {dateTime = new DateTime(2018,1,2),value = new Value {restingHeartRate = 52}},
                        new ActivitiesHeart {dateTime = new DateTime(2018,1,3),value = new Value {restingHeartRate = 53}},
                    };

            var result = _fitbitMapper.MapActivitiesHeartsToRestingHeartRates(activitiesHeart);

            //Then
            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Beats == 51);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Beats == 52);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Beats == 53);

        }

        [Fact]
        public void ShouldNotMapActivitiesHeartsThatHaveZeroRestingHeartRate()
        {
            var activitiesHeart = new List<ActivitiesHeart>
                    {
                        new ActivitiesHeart {dateTime = new DateTime(2018,1,1),value = new Value {restingHeartRate = 0}}
                    };

            var result = _fitbitMapper.MapActivitiesHeartsToRestingHeartRates(activitiesHeart);

            //Then
            Assert.Empty(result);
        }





        [Fact]
        public void ShouldMapSleeps()
        {
            //Given
            IEnumerable<Sleep> sleeps = new List<Sleep>();

            //When
            var result = _fitbitMapper.MapFitbitSleepsToSleepSummaries(sleeps);

            //Then
            //Assert.True(false);
        }

    }
}