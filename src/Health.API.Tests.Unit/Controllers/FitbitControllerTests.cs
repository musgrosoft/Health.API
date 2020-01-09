using System.Threading.Tasks;
using Fitbit;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using HealthAPI.Controllers;
using HealthAPI.Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Health;
using Utils;
using Xunit;

namespace HealthAPI.Tests.Unit.Controllers.Migration
{
    public class FitbitControllerTests
    {
        private readonly FitbitController _fitbitController;
        private readonly Mock<ILogger> _logger;
        private readonly Mock<IConfig> _config;
        private readonly Mock<IFitbitService> _fitbitService;
        private readonly Mock<IBackgroundJobClient> _backgroundJobClient;
        private readonly IHangfireWork _hangfireWork;

        private Mock<IHealthService> _healthService;

        private Mock<ICalendar> _calendar;

        private Mock<IFitbitWork> _fitbitWork;
        //private Mock<IFitbitImporter> _fitbitMigrator;

        public FitbitControllerTests()
        {
            _logger = new Mock<ILogger>();
            _config =new Mock<IConfig>();
            _fitbitService = new Mock<IFitbitService>();
            _backgroundJobClient = new Mock<IBackgroundJobClient>();

            _fitbitWork = new Mock<IFitbitWork>();

            _healthService = new Mock<IHealthService>();
            _calendar = new Mock<ICalendar>();

            //_fitbitMigrator = new Mock<IFitbitImporter>();

            //todo do we need real HangfirWrok to test?
            _hangfireWork = new HangfireWork(_logger.Object, _fitbitWork.Object);
            _fitbitController = new FitbitController(_logger.Object, _config.Object, _fitbitService.Object, _backgroundJobClient.Object, _hangfireWork);
        }
        
        [Fact]
        public async Task ShouldSetTokens()
        {
            await _fitbitController.OAuth("qwerty111");

            _fitbitService.Verify(x=>x.SetTokens("qwerty111"), Times.Once);
        }
        
        [Fact]
        public void ShouldVerifyFitbitCode()
        {
            _config.Setup(x => x.FitbitVerificationCode).Returns("ABC123");
            var response = (NoContentResult)_fitbitController.Verify("ABC123");

            Assert.Equal(StatusCodes.Status204NoContent, response.StatusCode);
        }

        [Fact]
        public void ShouldFailVerificationOfFitbitCode()
        {
            _config.Setup(x => x.FitbitVerificationCode).Returns("ABC123");
            var response = (NotFoundResult)_fitbitController.Verify("WRONG CODE");

            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public void ShouldMigrateFitbitDataByStartginBackgroundTask()
        {
            //When
            _fitbitController.Notify();

            //Then
            _backgroundJobClient.Verify(x => x.Create(
                                                        It.Is<Job>(job =>  
                                                                job.Method.Name == "MigrateAllFitbitData" && 
                                                                job.Type == typeof(HangfireWork)), 
                                                        It.IsAny<EnqueuedState>()
                                                    ));
        }

    }
}