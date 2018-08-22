using System.Threading.Tasks;
using HealthAPI.Controllers.Migration;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Migrators.Nokia;
using Moq;
using Services.Nokia;
using Utils;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.Migration
{
    public class NokiaControllerTests
    {
        private readonly NokiaController _nokiaController;
        private readonly Mock<INokiaService> _nokiaService;
        private readonly Mock<INokiaMigrator> _nokiaMigrator;
        private readonly Mock<ILogger> _logger;

        public NokiaControllerTests()
        {
            _logger = new Mock<ILogger>();
            _nokiaService = new Mock<INokiaService>();
            _nokiaMigrator = new Mock<INokiaMigrator>();

            _nokiaController = new NokiaController(_logger.Object, _nokiaMigrator.Object, _nokiaService.Object);
        }

        //[Fact]
        //public async Task ShouldMigrateNokiaData()
        //{
        //    var _logger = new Mock<ILogger>();
        //    var _nokiaMigrator = new Mock<INokiaMigrator>();

        //    var _fitbitController = new NokiaController(_logger.Object, _nokiaMigrator.Object,null,null);

        //    //When
        //    await _fitbitController.Migrate();

        //    //Then
        //    _nokiaMigrator.Verify(x => x.MigrateWeights(), Times.Once);
        //    _nokiaMigrator.Verify(x => x.MigrateBloodPressures(), Times.Once);
        //}

        [Fact]
        public async Task ShouldListSubscriptions()
        {
            _nokiaService.Setup(x => x.GetSubscriptions()).Returns(Task.FromResult("All the subscriptions"));

            var response = (OkObjectResult) (await _nokiaController.ListSubscriptions());

            Assert.Equal("All the subscriptions", response.Value);
        }

        [Fact]
        public async Task ShouldSubscribe()
        {
            await _nokiaController.Subscribe();

            _nokiaService.Verify(x=>x.Subscribe(), Times.Once);

        }

        [Fact]
        public void ShouldReturnOAuthUrl()
        {
            var response = (JsonResult) _nokiaController.OAuthUrl();

            Assert.Equal("https://account.health.nokia.com/oauth2_user/authorize2?response_type=code&redirect_uri=http://musgrosoft-health-api.azurewebsites.net/api/nokia/oauth/&client_id=09d4e17f36ee237455246942602624feaad12ac51598859bc79ddbd821147942&scope=user.info,user.metrics,user.activity&state=768uyFys", response.Value);

        }
    }
}