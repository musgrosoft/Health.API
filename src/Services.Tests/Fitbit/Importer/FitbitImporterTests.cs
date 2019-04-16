using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Importer.Fitbit;
using Importer.Fitbit.Importer;
using Moq;
using Repositories.Health.Models;
using Services.Health;
using Utils;
using Xunit;

namespace Services.Tests.Fitbit.Importer
{
    public class FitbitImporterTests
    {
        private readonly Mock<IFitbitService> _fitbitClient;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IHealthService> _healthService;
        private readonly Mock<ICalendar> _calendar;
        private readonly FitbitImporter _fitbitImporter;
        private readonly DateTime latestDate = new DateTime(2012, 3, 4);

        private const int SEARCH_DAYS_PREVIOUS = 1;

        public FitbitImporterTests()
        {
            _fitbitClient = new Mock<IFitbitService>();
            _logger = new Mock<ILogger>();
            _healthService = new Mock<IHealthService>();
            _calendar = new Mock<ICalendar>();
        
            _fitbitImporter = new FitbitImporter(_healthService.Object, _logger.Object, _fitbitClient.Object, _calendar.Object);
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

            await _fitbitImporter.MigrateRestingHeartRates();

            _healthService.Verify(x => x.UpsertRestingHeartRates(restingHeartRates), Times.Once);
        }

    }
}