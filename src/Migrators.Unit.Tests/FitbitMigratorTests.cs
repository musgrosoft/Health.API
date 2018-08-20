using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Migrators.Fitbit;
using Moq;
using Repositories.Models;
using Services.Fitbit;
using Services.Health;
using Utils;
using Xunit;

namespace Migrators.Unit.Tests
{
    public class FitbitMigratorTests
    {
        private Mock<IFitbitService> _fitbitClient;
        private Mock<ILogger> _logger;
        private Mock<IHealthService> _healthService;
        private readonly Mock<ICalendar> _calendar;
        private FitbitMigrator _fitbitMigrator;
        private readonly DateTime latestDate = new DateTime(2012, 3, 4);


        private const int SEARCH_DAYS_PREVIOUS = 10;

        public FitbitMigratorTests()
        {
            _fitbitClient = new Mock<IFitbitService>();
            _logger = new Mock<ILogger>();
            _healthService = new Mock<IHealthService>();
            _calendar = new Mock<ICalendar>();
        

            _fitbitMigrator = new FitbitMigrator(_healthService.Object, _logger.Object, _fitbitClient.Object, _calendar.Object);
        }

        [Fact]
        public async Task ShouldMigrateStepCounts()
        {
            _healthService.Setup(x => x.GetLatestStepCountDate(It.IsAny<DateTime>())).Returns(latestDate);
            _healthService.Setup(x => x.UpsertStepCounts(It.IsAny<IEnumerable<StepCount>>()));
            
            var stepCounts = new List<StepCount>
            {
                new StepCount{ CreatedDate = new DateTime(2010, 12, 1), Count = 111 },
                new StepCount{ CreatedDate = new DateTime(2022, 12, 22), Count = 222}
            };

            _fitbitClient.Setup(x => x.GetStepCounts(latestDate.AddDays(-SEARCH_DAYS_PREVIOUS),It.IsAny<DateTime>())).Returns(Task.FromResult((IEnumerable<StepCount>)stepCounts));

            await _fitbitMigrator.MigrateStepCounts();

            _healthService.Verify(x => x.UpsertStepCounts(stepCounts), Times.Once);
        }


        [Fact]
        public async Task ShouldMigrateActivityData()
        {
            _healthService.Setup(x => x.GetLatestActivitySummaryDate(It.IsAny<DateTime>())).Returns(latestDate);
            _healthService.Setup(x => x.UpsertActivitySummaries(It.IsAny<IEnumerable<ActivitySummary>>()));

            var dailyActivities = new List<ActivitySummary>
            {
                new ActivitySummary{ CreatedDate = new DateTime(2010, 12, 1), VeryActiveMinutes = 111 },
                new ActivitySummary{ CreatedDate = new DateTime(2010, 12, 1), VeryActiveMinutes = 222 }
            };

            _fitbitClient.Setup(x => x.GetActivitySummaries(latestDate.AddDays(-SEARCH_DAYS_PREVIOUS), It.IsAny<DateTime>())).Returns(Task.FromResult((IEnumerable<ActivitySummary>)dailyActivities));

            await _fitbitMigrator.MigrateActivitySummaries();

            _healthService.Verify(x => x.UpsertActivitySummaries(dailyActivities), Times.Once);
        }

        [Fact]
        public async Task ShouldMigrateRestingHeartRateData()
        {
            _healthService.Setup(x => x.GetLatestRestingHeartRateDate(It.IsAny<DateTime>())).Returns(latestDate);
            _healthService.Setup(x => x.UpsertRestingHeartRates(It.IsAny<IEnumerable<RestingHeartRate>>()));

            var restingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate{ CreatedDate = new DateTime(2010, 12, 1), Beats = 111 },
                new RestingHeartRate{ CreatedDate = new DateTime(2010, 12, 1), Beats = 222 }
            };

            _fitbitClient.Setup(x => x.GetRestingHeartRates(latestDate.AddDays(-SEARCH_DAYS_PREVIOUS), It.IsAny<DateTime>())).Returns(Task.FromResult((IEnumerable<RestingHeartRate>)restingHeartRates));

            await _fitbitMigrator.MigrateRestingHeartRates();

            _healthService.Verify(x => x.UpsertRestingHeartRates(restingHeartRates), Times.Once);
        }

        [Fact]
        public async Task ShouldMigrateHeartZoneData()
        {
            _healthService.Setup(x => x.GetLatestHeartSummaryDate(It.IsAny<DateTime>())).Returns(latestDate);
            _healthService.Setup(x => x.UpsertHeartSummaries(It.IsAny<IEnumerable<HeartRateSummary>>()));

            var heartZones = new List<HeartRateSummary>
            {
                new HeartRateSummary(){ CreatedDate = new DateTime(2010, 12, 1), CardioMinutes = 111 },
                new HeartRateSummary{ CreatedDate = new DateTime(2022, 12, 22), CardioMinutes = 222}
            };

            _fitbitClient.Setup(x => x.GetHeartSummaries(latestDate.AddDays(-SEARCH_DAYS_PREVIOUS), It.IsAny<DateTime>())).Returns(Task.FromResult((IEnumerable<HeartRateSummary>)heartZones));

            await _fitbitMigrator.MigrateHeartSummaries();

            _healthService.Verify(x => x.UpsertHeartSummaries(heartZones), Times.Once);
        }
    }
}