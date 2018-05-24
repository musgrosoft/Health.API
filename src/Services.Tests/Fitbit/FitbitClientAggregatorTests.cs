using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Services.Fitbit;
using Services.Fitbit.Domain;
using Xunit;
using Utils;

namespace Services.Tests.Fitbit
{
    public class FitbitClientAggregatorTests
    {
        private Mock<IFitbitClient> _fitbitClient;
        private Mock<ILogger> _logger;
        private FitbitClientClientAggregator _fitbitClientClientAggregator;

        public FitbitClientAggregatorTests()
        {
            _fitbitClient = new Mock<IFitbitClient>();
            _logger = new Mock<ILogger>();
            _fitbitClientClientAggregator = new FitbitClientClientAggregator(_fitbitClient.Object, _logger.Object);
        }

        [Fact]
        public async Task ShouldGetFitbitDailyActivities()
        {
            DateTime day1 = new DateTime(2017, 1, 1);
            DateTime day2 = new DateTime(2017, 1, 2);
            DateTime day3 = new DateTime(2017, 1, 3);
            DateTime day4 = new DateTime(2017, 1, 4);
            DateTime day5 = new DateTime(2017, 1, 5);

            //Given
            _fitbitClient.Setup(x => x.GetFitbitDailyActivity(day1)).Returns(Task.FromResult(new FitbitDailyActivity { DateTime = day1, summary = new Summary { steps = 1 } }));
            _fitbitClient.Setup(x => x.GetFitbitDailyActivity(day2)).Returns(Task.FromResult(new FitbitDailyActivity { DateTime = day2, summary = new Summary { steps = 2 } }));
            _fitbitClient.Setup(x => x.GetFitbitDailyActivity(day3)).Returns(Task.FromResult(new FitbitDailyActivity { DateTime = day3, summary = new Summary { steps = 3 } }));
            _fitbitClient.Setup(x => x.GetFitbitDailyActivity(day4)).Returns(Task.FromResult(new FitbitDailyActivity { DateTime = day4, summary = new Summary { steps = 4 } }));
            _fitbitClient.Setup(x => x.GetFitbitDailyActivity(day5)).Returns(Task.FromResult(new FitbitDailyActivity { DateTime = day5, summary = new Summary { steps = 5 } }));

            //When
            var fitbitDailyActivities = await _fitbitClientClientAggregator.GetFitbitDailyActivities(day1, day5);

            //Then
            Assert.Equal(5, fitbitDailyActivities.Count());
            Assert.Contains(fitbitDailyActivities, x => x.DateTime == day1 && x.summary.steps == 1);
            Assert.Contains(fitbitDailyActivities, x => x.DateTime == day2 && x.summary.steps == 2);
            Assert.Contains(fitbitDailyActivities, x => x.DateTime == day3 && x.summary.steps == 3);
            Assert.Contains(fitbitDailyActivities, x => x.DateTime == day4 && x.summary.steps == 4);
            Assert.Contains(fitbitDailyActivities, x => x.DateTime == day5 && x.summary.steps == 5);

        }

        [Fact]
        public async Task ShouldGetFitbitHeartActivities()
        {
            DateTime fromDate = new DateTime(2018, 1, 15);
            DateTime toDate = new DateTime(2018, 3, 15);

            //Given 
            _fitbitClient.Setup(x => x.GetMonthOfFitbitActivities(fromDate.AddMonths(1))).Returns(Task.FromResult(new FitBitActivity
            {
                activitiesHeart = new List<ActivitiesHeart>
                {
                    new ActivitiesHeart{dateTime = new DateTime(2018,1,20) , value = new Value {restingHeartRate = 11} },
                    new ActivitiesHeart{dateTime = new DateTime(2018,1,21) , value = new Value {restingHeartRate = 12} },
                }
            }));

            _fitbitClient.Setup(x => x.GetMonthOfFitbitActivities(fromDate.AddMonths(2))).Returns(Task.FromResult(new FitBitActivity
            {
                activitiesHeart = new List<ActivitiesHeart>
                {
                    new ActivitiesHeart{dateTime = new DateTime(2018,2,20) , value = new Value {restingHeartRate = 21} },
                    new ActivitiesHeart{dateTime = new DateTime(2018,2,21) , value = new Value {restingHeartRate = 22} },
                }
            }));

            _fitbitClient.Setup(x => x.GetMonthOfFitbitActivities(fromDate.AddMonths(3))).Returns(Task.FromResult(new FitBitActivity
            {
                activitiesHeart = new List<ActivitiesHeart>
                {
                    new ActivitiesHeart{dateTime = new DateTime(2018,3,14) ,value = new Value {restingHeartRate = 31} },
                    new ActivitiesHeart{dateTime = new DateTime(2018,3,16) ,value = new Value {restingHeartRate = 32} },
                }
            }));

            //When
            var heartActivities = await _fitbitClientClientAggregator.GetFitbitHeartActivities(fromDate, toDate);

            Assert.Equal(5, heartActivities.Count());
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 11);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 12);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 21);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 22);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 31);

        }
    }
}