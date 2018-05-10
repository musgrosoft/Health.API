using System;
using System.Collections.Generic;
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
        

        private const int SEARCH_DAYS_PREVIOUS = 10;

        public FitbitMigratorTests()
        {
            _fitbitClient = new Mock<IFitbitClient>();
            _logger = new Mock<ILogger>();
            _healthService = new Mock<IHealthService>();
        

            _fitbitMigrator = new FitbitMigrator(_healthService.Object, _logger.Object, _fitbitClient.Object);
        }

        [Fact]
        public async Task ShouldMigrateSteps()
        {
            var latestStepDate = new DateTime(2010, 12, 1);
            
            _healthService.Setup(x => x.GetLatestStepCountDate()).Returns(latestStepDate);
            _healthService.Setup(x => x.UpsertStepCounts(It.IsAny<IEnumerable<StepCount>>())).Returns(Task.CompletedTask);


            var stepCounts = new List<StepCount>
            {
                new StepCount{
                    DateTime = new DateTime(2010, 12, 1),
                    Count = 111
                    },
                new StepCount{
                    DateTime = new DateTime(2022, 12, 22),
                    Count = 222
                },

            };

            _fitbitClient.Setup(x => x.GetStepCounts(latestStepDate.AddDays(-SEARCH_DAYS_PREVIOUS))).Returns(Task.FromResult((IEnumerable<StepCount>)stepCounts));

            await _fitbitMigrator.MigrateStepData();

            //       _healthService.Verify(x => x.UpsertStepCount(stepCount1), Times.Once);
            _healthService.Verify(x => x.UpsertStepCounts(stepCounts), Times.Once);




        }
    }
}