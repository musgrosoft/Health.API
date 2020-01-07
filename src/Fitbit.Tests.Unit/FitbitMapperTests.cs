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
            IEnumerable<Sleep> sleeps = new List<Sleep>
            {
                new Sleep
                {
                    dateOfSleep = new DateTime(2010, 2, 3),
                    endTime = new DateTime(2011,5,6),
                    minutesAsleep = 5,
                    minutesAwake = 6,
                    startTime = new DateTime(2022, 3, 4),
                    type = "wibble type",
                    infoCode = 123456,
                    levels = new Levels
                    {
                        summary = new Summary
                        {
                            deep = new SleepData {minutes = 1},
                            rem = new SleepData {minutes = 2},
                            light = new SleepData {minutes = 3},
                            wake = new SleepData {minutes = 4},
                        }
                    }
                }
                
            };

            //When
            var result = _fitbitMapper.MapFitbitSleepsToSleepSummaries(sleeps).ToList();

            //Then
            Assert.Single(result);

            Assert.Equal(1, result[0].DeepMinutes);
            Assert.Equal(2, result[0].RemMinutes);
            Assert.Equal(3, result[0].LightMinutes);
            Assert.Equal(4, result[0].WakeMinutes);

            Assert.Equal(new DateTime(2010, 2, 3), result[0].DateOfSleep);
            Assert.Equal(new DateTime(2022, 3, 4), result[0].StartTime);
            Assert.Equal(new DateTime(2011, 5, 6), result[0].EndTime);

            Assert.Equal(5, result[0].MinutesAsleep);
            Assert.Equal(6, result[0].MinutesAwake);

            Assert.Equal("wibble type", result[0].Type);
            Assert.Equal(123456, result[0].InfoCode);

        }

    }
}