using System.Threading.Tasks;
using HealthAPI.Controllers.Migration;
using Migrators;
using Moq;
using Services.OAuth;
using Utils;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.Migration
{
    public class FitbitControllerTests
    {
        [Fact]
        public async Task ShouldMigrateFitbitData()
        {
            var _logger = new Mock<ILogger>();
            var _oAuthService = new Mock<IOAuthService>();
            var _fitbitMigrator = new Mock<IFitbitMigrator>();

            var _fitbitController = new FitbitController(_logger.Object, _oAuthService.Object, _fitbitMigrator.Object);

            //When
            await _fitbitController.Migrate();

            //Then
            _fitbitMigrator.Verify(x => x.MigrateActivitySummaries(), Times.Once);
            _fitbitMigrator.Verify(x => x.MigrateHeartSummaries(), Times.Once);
            _fitbitMigrator.Verify(x => x.MigrateRestingHeartRates(), Times.Once);
            _fitbitMigrator.Verify(x => x.MigrateStepCounts(), Times.Once);
        }
    }
}