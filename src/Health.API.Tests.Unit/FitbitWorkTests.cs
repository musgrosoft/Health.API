using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitbit;
using Fitbit.Domain;
using HealthAPI.Hangfire;
using Moq;
using Repositories.Health;
using Repositories.Health.Models;
using Services.Health;
using Utils;
using Xunit;

namespace HealthAPI.Tests.Unit
{
    public class FitbitWorkTests
    {
        private Mock<IHealthService> _healthService;
        private Mock<IFitbitService> _fitbitService;
        private Mock<ICalendar> _calendar;
        private Mock<ILogger> _logger;
        private FitbitWork _fitbitWork;

        public FitbitWorkTests()
        {
            _healthService = new Mock<IHealthService>();
            _fitbitService = new Mock<IFitbitService>();
            _calendar = new Mock<ICalendar>();
            _logger = new Mock<ILogger>();

            _fitbitWork = new FitbitWork(_logger.Object,_healthService.Object, _calendar.Object, _fitbitService.Object);
        }

        [Fact]
        public async Task ShouldImportRestingHeartRates()
        {
            var latestDate = new DateTime(2001,2,3);
            var now = new DateTime(2002,3,4);

            _calendar.Setup(x => x.Now()).Returns(now);

            _healthService
                .Setup(x => x.GetLatestRestingHeartRateDate(It.IsAny<DateTime>()))
                .Returns(latestDate);

            IEnumerable<RestingHeartRate> restingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate{ CreatedDate = new DateTime(2010, 12, 1), Beats = 111 },
                new RestingHeartRate{ CreatedDate = new DateTime(2010, 12, 2), Beats = 222 }
            };
        
            _fitbitService
                .Setup(x => x.GetRestingHeartRates(latestDate.AddDays(-10), now))
                .Returns(Task.FromResult(restingHeartRates));

            await _fitbitWork.ImportRestingHeartRates();
        
            _healthService.Verify(x => x.UpsertAsync(restingHeartRates), Times.Once);
        }


        [Fact]
        public async Task ShouldImportSleeps()
        {
            var latestDate = new DateTime(2001, 2, 3);
            var now = new DateTime(2002, 3, 4);

            _calendar.Setup(x => x.Now()).Returns(now);

            _healthService
                .Setup(x => x.GetLatestSleepSummaryDate(It.IsAny<DateTime>()))
                .Returns(latestDate);

            IEnumerable<SleepSummary> sleeps = new List<SleepSummary>
            {
                new SleepSummary{ DateOfSleep = new DateTime(2010, 12, 1), MinutesAsleep = 111 },
                new SleepSummary{ DateOfSleep = new DateTime(2010, 12, 2), MinutesAsleep = 222 }
            };

            _fitbitService
                .Setup(x => x.GetSleepSummaries(latestDate.AddDays(-10), now))
                .Returns(Task.FromResult(sleeps));

            await _fitbitWork.ImportSleepSummaries();

            _healthService.Verify(x => x.UpsertAsync(sleeps), Times.Once);
        }


    }
}