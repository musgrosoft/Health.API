using System.Threading.Tasks;
using Google;
using HealthAPI.Controllers.Migration;
using Moq;
using Services.Migrator;
using Utils;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.Migration
{
    public class GoogleSheetsControllerTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly GoogleSheetsController _googleSheetsController;
        private readonly Mock<IGoogleMigrator> _googleMigrator;

        public GoogleSheetsControllerTests()
        {
            _logger = new Mock<ILogger>();
            _googleMigrator = new Mock<IGoogleMigrator>();
     
            _googleSheetsController = new GoogleSheetsController(_logger.Object,_googleMigrator.Object);
        }

        [Fact]
        public void ShouldMigrateRuns()
        {
            _googleSheetsController.MigrateRuns();

            _googleMigrator.Verify(x => x.MigrateRuns(), Times.Once);
            _logger.Verify(x => x.LogMessageAsync("GOOGLE SHEETS : Migrate Runs"));

        }

        [Fact]
        public void ShouldMigrateErgos()
        {
            _googleSheetsController.MigrateErgos();

            _googleMigrator.Verify(x => x.MigrateErgos(), Times.Once);
            _logger.Verify(x => x.LogMessageAsync("GOOGLE SHEETS : Migrate Ergos"));

        }

        [Fact]
        public void ShouldMigrateUnits()
        {
            _googleSheetsController.MigrateUnits();

            _googleMigrator.Verify(x=>x.MigrateAlcoholIntakes(), Times.Once);
            _logger.Verify(x=>x.LogMessageAsync("GOOGLE SHEETS : Migrate Units"));

        }
    }
}