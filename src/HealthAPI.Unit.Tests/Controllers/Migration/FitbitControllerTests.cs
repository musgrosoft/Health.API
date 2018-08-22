using System.Threading.Tasks;
using HealthAPI.Controllers.Migration;
using Migrators;
using Migrators.Hangfire;
using Moq;
using Services.Fitbit;
using Services.OAuth;
using Utils;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.Migration
{
    
    public class FitbitControllerTests
    {
        private readonly FitbitController _fitbitController;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IConfig> _config;
        private readonly Mock<IFitbitService> _fitbitService;
        private readonly Mock<IHangfireUtility> _hangfireUtility;
        private readonly Mock<IHangfireWork> _hangfireWork;

        public FitbitControllerTests()
        {
            _logger = new Mock<ILogger>();
            _config =new Mock<IConfig>();
            _fitbitService = new Mock<IFitbitService>();
            _hangfireUtility = new Mock<IHangfireUtility>();
            _hangfireWork = new Mock<IHangfireWork>();


            _fitbitController = new FitbitController(_logger.Object, _config.Object, _fitbitService.Object, _hangfireUtility.Object, _hangfireWork.Object);
        }

        [Fact]
        public async Task ShouldSubscribe()
        {
            await _fitbitController.Subscribe();

            _fitbitService.Verify(x=>x.Subscribe());
        }

        //[Fact]
        //public async Task ShouldMigrateFitbitData()
        //{
        //    var _logger = new Mock<ILogger>();
        //    var _oAuthService = new Mock<IOAuthService>();
        //    var _fitbitMigrator = new Mock<IFitbitMigrator>();

        //    var _fitbitController = new FitbitController(_logger.Object, _oAuthService.Object, _fitbitMigrator.Object);

        //    //When
        //    await _fitbitController.Migrate();

        //    //Then
        //    _fitbitMigrator.Verify(x => x.MigrateActivitySummaries(), Times.Once);
        //    _fitbitMigrator.Verify(x => x.MigrateHeartSummaries(), Times.Once);
        //    _fitbitMigrator.Verify(x => x.MigrateRestingHeartRates(), Times.Once);
        //    _fitbitMigrator.Verify(x => x.MigrateStepCounts(), Times.Once);
        //}
    }
}