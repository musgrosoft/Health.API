using System;
using System.Threading.Tasks;
using HealthAPI.Hangfire;
using Importer.Fitbit.Importer;
using Moq;
using Utils;
using Xunit;

namespace HealthAPI.Tests.Unit
{
    public class HangifireWorkTests
    {
        private Mock<IFitbitImporter> _fitbitMigrator;
        private Mock<ILogger> _logger;
        private HangfireWork _hangfireWork;

        public HangifireWorkTests()
        {
            _fitbitMigrator = new Mock<IFitbitImporter>();
            _logger = new Mock<ILogger>();
            _hangfireWork = new HangfireWork(_fitbitMigrator.Object, _logger.Object);
        }

        [Fact]
        public async Task ShouldMigrateAllFitbitData()
        {
            //when
            await _hangfireWork.MigrateAllFitbitData();

            //then
            _fitbitMigrator.Verify(x => x.MigrateRestingHeartRates(), Times.Once);
        }

        [Fact]
        public async Task ShouldLogErrorInMigrateAllFitbitData()
        {
            //given
            var ex = new Exception("I'm a little teapot.");
            _fitbitMigrator.Setup(x => x.MigrateRestingHeartRates()).Throws(ex);

            //when
            await _hangfireWork.MigrateAllFitbitData();

            //then
            _logger.Verify(x => x.LogErrorAsync(ex), Times.Once);
        }


    }
}