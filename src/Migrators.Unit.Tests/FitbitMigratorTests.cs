﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Repositories.Models;
using Services.Fitbit;
using Services.MyHealth;
using Utils;
using Xunit;

namespace Migrators.Unit.Tests
{
    public class FitbitMigratorTests
    {
        private Mock<IFitbitClient> _fitbitClient;
        private Mock<ILogger> _logger;
        private Mock<IHealthService> _healthService;
        private FitbitMigrator _fitbitMigrator;
        private readonly DateTime latestDate = new DateTime(2012, 3, 4);


        private const int SEARCH_DAYS_PREVIOUS = 10;

        public FitbitMigratorTests()
        {
            _fitbitClient = new Mock<IFitbitClient>();
            _logger = new Mock<ILogger>();
            _healthService = new Mock<IHealthService>();
        

            _fitbitMigrator = new FitbitMigrator(_healthService.Object, _logger.Object, _fitbitClient.Object);
        }

        [Fact]
        public async Task ShouldMigrateStepCounts()
        {
            _healthService.Setup(x => x.GetLatestStepCountDate()).Returns(latestDate);
            _healthService.Setup(x => x.UpsertStepCounts(It.IsAny<IEnumerable<StepCount>>())).Returns(Task.CompletedTask);
            
            var stepCounts = new List<StepCount>
            {
                new StepCount{ DateTime = new DateTime(2010, 12, 1), Count = 111 },
                new StepCount{ DateTime = new DateTime(2022, 12, 22), Count = 222}
            };

            _fitbitClient.Setup(x => x.GetStepCounts(latestDate.AddDays(-SEARCH_DAYS_PREVIOUS))).Returns(Task.FromResult((IEnumerable<StepCount>)stepCounts));

            await _fitbitMigrator.MigrateStepData();

            _healthService.Verify(x => x.UpsertStepCounts(stepCounts), Times.Once);
        }


        [Fact]
        public async Task ShouldMigrateActivityData()
        {
            _healthService.Setup(x => x.GetLatestDailyActivityDate()).Returns(latestDate);
            _healthService.Setup(x => x.UpsertDailyActivities(It.IsAny<IEnumerable<DailyActivity>>())).Returns(Task.CompletedTask);

            var dailyActivities = new List<DailyActivity>
            {
                new DailyActivity{ DateTime = new DateTime(2010, 12, 1), VeryActiveMinutes = 111 },
                new DailyActivity{ DateTime = new DateTime(2010, 12, 1), VeryActiveMinutes = 222 }
            };

            _fitbitClient.Setup(x => x.GetDailyActivities(latestDate.AddDays(-SEARCH_DAYS_PREVIOUS))).Returns(Task.FromResult((IEnumerable<DailyActivity>)dailyActivities));

            await _fitbitMigrator.MigrateActivity();

            _healthService.Verify(x => x.UpsertDailyActivities(dailyActivities), Times.Once);
        }

        [Fact]
        public async Task ShouldMigrateRestingHeartRateData()
        {
            _healthService.Setup(x => x.GetLatestRestingHeartRateDate()).Returns(latestDate);
            _healthService.Setup(x => x.UpsertRestingHeartRates(It.IsAny<IEnumerable<RestingHeartRate>>())).Returns(Task.CompletedTask);

            var restingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate{ DateTime = new DateTime(2010, 12, 1), Beats = 111 },
                new RestingHeartRate{ DateTime = new DateTime(2010, 12, 1), Beats = 222 }
            };

            _fitbitClient.Setup(x => x.GetRestingHeartRates(latestDate.AddDays(-SEARCH_DAYS_PREVIOUS))).Returns(Task.FromResult((IEnumerable<RestingHeartRate>)restingHeartRates));

            await _fitbitMigrator.MigrateRestingHeartRateData();

            _healthService.Verify(x => x.UpsertRestingHeartRates(restingHeartRates), Times.Once);
        }

        [Fact]
        public async Task ShouldMigrateHeartZoneData()
        {
            _healthService.Setup(x => x.GetLatestHeartRateDailySummaryDate()).Returns(latestDate);
            _healthService.Setup(x => x.UpsertDailyHeartSummaries(It.IsAny<IEnumerable<HeartRateZoneSummary>>())).Returns(Task.CompletedTask);

            var heartZones = new List<HeartRateZoneSummary>
            {
                new HeartRateZoneSummary(){ DateTime = new DateTime(2010, 12, 1), CardioMinutes = 111 },
                new HeartRateZoneSummary{ DateTime = new DateTime(2022, 12, 22), CardioMinutes = 222}
            };

            _fitbitClient.Setup(x => x.GetHeartZones(latestDate.AddDays(-SEARCH_DAYS_PREVIOUS))).Returns(Task.FromResult((IEnumerable<HeartRateZoneSummary>)heartZones));

            await _fitbitMigrator.MigrateHeartZoneData();

            _healthService.Verify(x => x.UpsertDailyHeartSummaries(heartZones), Times.Once);
        }
    }
}