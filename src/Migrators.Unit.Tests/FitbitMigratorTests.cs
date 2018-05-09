using System;
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
        private Mock<ICalendar> _calendar;

        private const int SEARCH_DAYS_PREVIOUS = 10;

        public FitbitMigratorTests()
        {
            _fitbitClient = new Mock<IFitbitClient>();
            _logger = new Mock<ILogger>();
            _healthService = new Mock<IHealthService>();
            _calendar = new Mock<ICalendar>();

            _fitbitMigrator = new FitbitMigrator(_healthService.Object, _logger.Object, _fitbitClient.Object, _calendar.Object);
        }

        [Fact]
        public async Task ShouldMigrateSteps()
        {
            var latestStepDate = new DateTime(2010, 12, 1);
            _healthService.Setup(x => x.GetLatestStepCountDate()).Returns(latestStepDate);
            _healthService.Setup(x => x.UpsertStepCount(It.IsAny<StepCount>())).Returns(Task.CompletedTask);

            _calendar.Setup(x => x.Now()).Returns(new DateTime(2010, 12, 4));

            var stepCount1 = new StepCount
            {
                DateTime = new DateTime(2010, 12, 1),
                Count = 111
            };

            _fitbitClient.Setup(x => x.GetDailySteps(new DateTime(2010, 12, 1))).Returns(Task.FromResult(stepCount1));

            _fitbitClient.Setup(x => x.GetDailySteps(It.IsAny<DateTime>())).Returns(Task.FromResult(new StepCount
            {
                DateTime = new DateTime(2010, 12, 1),
                Count = 222
            }));

            await _fitbitMigrator.MigrateStepData();

            //       _healthService.Verify(x => x.UpsertStepCount(stepCount1), Times.Once);
            _healthService.Verify(x => x.UpsertStepCount(It.IsAny<StepCount>()), Times.AtLeastOnce);




        }
    }
}