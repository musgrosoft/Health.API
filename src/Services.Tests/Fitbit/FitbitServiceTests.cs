using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy.Contributors;
using Moq;
using Services.Fitbit;
using Services.Fitbit.Domain;
using Utils;
using Xunit;

namespace Services.Tests.Fitbit
{
    public class FitbitServiceTests
    {
        private FitbitService _fitbitService;
        private Mock<IConfig> _config;
        private Mock<ILogger> _logger;
        private Mock<IFitbitAggregator> _fitbitAggregator;

        private DateTime fromDate = new DateTime(2017,1,1);
        private DateTime toDate = new DateTime(2018, 2, 2);

        public FitbitServiceTests() 
        {
            _config = new Mock<IConfig>();
            _logger = new Mock<ILogger>();
            _fitbitAggregator = new Mock<IFitbitAggregator>();

            _fitbitService = new FitbitService(_config.Object, _logger.Object, _fitbitAggregator.Object);
        }

        [Fact]
        public async Task ShouldGetStepCounts()
        {
            //Given
            _fitbitAggregator.Setup(x => x.GetFitbitDailyActivities(fromDate, toDate))
                .Returns(Task.FromResult((IEnumerable<FitbitDailyActivity>) new List<FitbitDailyActivity>
                {
                    new FitbitDailyActivity{DateTime = new DateTime(2017,1,1), summary = new Summary {steps = 111}},
                    new FitbitDailyActivity{DateTime = new DateTime(2017,1,2), summary = new Summary {steps = 222}},
                    new FitbitDailyActivity{DateTime = new DateTime(2017,1,3), summary = new Summary {steps = 333}},
                }));

            //When
            var stepCounts = await _fitbitService.GetStepCounts(fromDate, toDate);

            //Then
            Assert.Equal(3,stepCounts.Count());
            Assert.Contains(stepCounts, x => x.DateTime == new DateTime(2017, 1, 1) && x.Count == 111);
            Assert.Contains(stepCounts, x => x.DateTime == new DateTime(2017, 1, 2) && x.Count == 222);
            Assert.Contains(stepCounts, x => x.DateTime == new DateTime(2017, 1, 3) && x.Count == 333);

        }

        [Fact]
        public async Task ShouldGetDailyActivities()
        {
            //Given
            _fitbitAggregator.Setup(x => x.GetFitbitDailyActivities(fromDate, toDate))
                .Returns(Task.FromResult((IEnumerable<FitbitDailyActivity>)new List<FitbitDailyActivity>
                {
                    new FitbitDailyActivity{DateTime = new DateTime(2017,1,1), summary = new Summary {fairlyActiveMinutes = 101, lightlyActiveMinutes = 201, sedentaryMinutes = 301, veryActiveMinutes = 401}},
                    new FitbitDailyActivity{DateTime = new DateTime(2017,1,2), summary = new Summary {fairlyActiveMinutes = 102, lightlyActiveMinutes = 202, sedentaryMinutes = 302, veryActiveMinutes = 402}},
                    new FitbitDailyActivity{DateTime = new DateTime(2017,1,3), summary = new Summary {fairlyActiveMinutes = 103, lightlyActiveMinutes = 203, sedentaryMinutes = 303, veryActiveMinutes = 403}},
                }));

            //When
            var dailyActivities = await _fitbitService.GetDailyActivities(fromDate, toDate);

            //Then
            Assert.Equal(3, dailyActivities.Count());
            Assert.Contains(dailyActivities, x => x.DateTime == new DateTime(2017, 1, 1) && x.FairlyActiveMinutes == 101 && x.LightlyActiveMinutes == 201 && x.SedentaryMinutes == 301 && x.VeryActiveMinutes == 401);
            Assert.Contains(dailyActivities, x => x.DateTime == new DateTime(2017, 1, 2) && x.FairlyActiveMinutes == 102 && x.LightlyActiveMinutes == 202 && x.SedentaryMinutes == 302 && x.VeryActiveMinutes == 402);
            Assert.Contains(dailyActivities, x => x.DateTime == new DateTime(2017, 1, 3) && x.FairlyActiveMinutes == 103 && x.LightlyActiveMinutes == 203 && x.SedentaryMinutes == 303 && x.VeryActiveMinutes == 403);

        }

        [Fact]
        public async Task ShouldGetRestingHeartRates()
        {
            //Given
            _fitbitAggregator.Setup(x=>x.GetFitbitHeartActivities(fromDate, toDate))
                .Returns(Task.FromResult((IEnumerable<ActivitiesHeart>)new List<ActivitiesHeart>
                {
                    new ActivitiesHeart{dateTime = new DateTime(2017,1,1), value = new Value {restingHeartRate = 111}},
                    new ActivitiesHeart{dateTime = new DateTime(2017,1,2), value = new Value {restingHeartRate = 222}},
                    new ActivitiesHeart{dateTime = new DateTime(2017,1,3), value = new Value {restingHeartRate = 333}},
                }));

            //When
            var restingHeartRates = await _fitbitService.GetRestingHeartRates(fromDate, toDate);

            //Then
            Assert.Equal(3, restingHeartRates.Count());
            Assert.Contains(restingHeartRates, x => x.DateTime == new DateTime(2017, 1, 1) && x.Beats == 111);
            Assert.Contains(restingHeartRates, x => x.DateTime == new DateTime(2017, 1, 2) && x.Beats == 222);
            Assert.Contains(restingHeartRates, x => x.DateTime == new DateTime(2017, 1, 3) && x.Beats == 333);

        }
    }
}