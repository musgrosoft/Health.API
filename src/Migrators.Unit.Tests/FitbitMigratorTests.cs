using System;
using System.Threading.Tasks;
using Moq;
using Services.Fitbit;
using Services.MyHealth;
using Utils;
using Xunit;

namespace Migrators.Unit.Tests
{
    public class FitbitMigratorTests
    {
        private Mock<IFitbitClient> _fitBitClient;
        private Mock<ILogger> _logger;
        private Mock<IHealthService> _healthService;
        private FitbitMigrator _fitbitMigrator;

        public FitbitMigratorTests()
        {
            _fitBitClient = new Mock<IFitbitClient>();
            _logger = new Mock<ILogger>();
            _healthService = new Mock<IHealthService>();

            _fitbitMigrator = new FitbitMigrator(_healthService.Object, _logger.Object, _fitBitClient.Object);
        }

        [Fact]
        public async Task ShouldMigrateSteps()
        {
            var latestStepDate = new DateTime(2010, 12, 1);

            _healthService.Setup(x => x.GetLatestStepCountDate()).Returns(latestStepDate);

            await _fitbitMigrator.MigrateStepData();






        }
    }
}