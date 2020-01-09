using System;
using System.Threading.Tasks;
using Fitbit;
using HealthAPI.Hangfire;
using Moq;
using Services.Health;
using Utils;
using Xunit;

namespace HealthAPI.Tests.Unit
{
    public class HangifireWorkTests
    {
        
        private Mock<ILogger> _logger;
        private Mock<IFitbitWork> _fitbitWork;
        private HangfireWork _hangfireWork;

        public HangifireWorkTests()
        { 

            _logger = new Mock<ILogger>();
            _fitbitWork = new Mock<IFitbitWork>();
            _hangfireWork = new HangfireWork(_logger.Object, _fitbitWork.Object);
        }

        [Fact]
        public async Task ShouldImportAllFitbitData()
        {
            //when
            await _hangfireWork.MigrateAllFitbitData();

            //then
            _fitbitWork.Verify(x => x.ImportRestingHeartRates(), Times.Once);
            _fitbitWork.Verify(x => x.ImportSleepSummaries(), Times.Once);

        }

        [Fact]
        public async Task ShouldLogErrorInMigrateAllFitbitData()
        {
            //given
            var ex = new Exception("I'm a little teapot.");
            _fitbitWork.Setup(x => x.ImportRestingHeartRates()).Throws(ex);

            //when
            await _hangfireWork.MigrateAllFitbitData();

            //then
            _logger.Verify(x => x.LogErrorAsync(ex), Times.Once);
        }


    }
}