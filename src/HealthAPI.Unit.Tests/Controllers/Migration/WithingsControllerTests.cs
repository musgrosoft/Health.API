using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HealthAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Health;
using Services.Withings;
using Utils;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.Migration
{
    public class WithingsControllerTests
    {
        private readonly WithingsController _withingsController;
        private readonly Mock<IWithingsService> _nokiaService;
        
        private readonly Mock<ILogger> _logger;
        private Mock<IHealthService> _healthService;

        public WithingsControllerTests()
        {
            _logger = new Mock<ILogger>();
            _nokiaService = new Mock<IWithingsService>();
            _healthService = new Mock<IHealthService>();
        

            _withingsController = new WithingsController(_logger.Object, _nokiaService.Object, _healthService.Object);
        }

        //[Fact]
        //public async Task ShouldMigrateWeights()
        //{
        //   var response = (OkResult)(await _withingsController.MigrateWeights());

        //    _nokiaMigrator.Verify(x=>x.MigrateWeights(), Times.Once);

        //    Assert.Equal( 200, response.StatusCode);
        //}

        //[Fact]
        //public async Task ShouldMigrateBloodPressures()
        //{
        //    var response = (OkResult)(await _withingsController.MigrateBloodPressures());

        //    _nokiaMigrator.Verify(x => x.MigrateBloodPressures(), Times.Once);

        //    Assert.Equal(200, response.StatusCode);
        //}
        
        [Fact]
        public async Task ShouldListSubscriptions()
        {
            var subscriptions = new List<string>
            {
                "subscript one",
                "subscription two"
            };

            _nokiaService.Setup(x => x.GetSubscriptions()).Returns(Task.FromResult(subscriptions));

            var response = (OkObjectResult) (await _withingsController.ListSubscriptions());

            Assert.Equal(subscriptions, response.Value);
        }

        [Fact]
        public async Task ShouldSubscribe()
        {
            await _withingsController.Subscribe();

            _nokiaService.Verify(x=>x.Subscribe(), Times.Once);

        }

        [Fact]
        public void ShouldReturnOAuthUrl()
        {
            var response = (JsonResult) _withingsController.OAuthUrl();

            Assert.Equal("https://account.health.nokia.com/oauth2_user/authorize2?response_type=code&redirect_uri=http://musgrosoft-health-api.azurewebsites.net/api/nokia/oauth/&client_id=09d4e17f36ee237455246942602624feaad12ac51598859bc79ddbd821147942&scope=user.info,user.metrics,user.activity&state=768uyFys", response.Value);

        }

        [Fact]
        public async Task ShouldSetTokens()
        {
            await _withingsController.OAuth("abc123");

            _nokiaService.Verify(x=>x.SetTokens("abc123"));

        }
    }
}