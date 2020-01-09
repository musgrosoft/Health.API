using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthAPI.Controllers;
using Moq;
using Repositories.Health.Models;
using Services.Health;
using Utils;
using Withings;
using Xunit;

namespace HealthAPI.Tests.Unit.Controllers.Migration
{
    public class WithingsControllerTests
    {
        private readonly WithingsController _withingsController;
        private readonly Mock<IWithingsService> _withingsService;
        
        private readonly Mock<ILogger> _logger;
        private Mock<IHealthService> _healthService;

        public WithingsControllerTests()
        {
            _logger = new Mock<ILogger>();
            _withingsService = new Mock<IWithingsService>();
            _healthService = new Mock<IHealthService>();
        

            _withingsController = new WithingsController(_logger.Object, _withingsService.Object, _healthService.Object);
        }

        
        [Fact]
        public async Task ShouldSetTokens()
        {
            await _withingsController.OAuth("abc123");

            _withingsService.Verify(x=>x.SetTokens("abc123"));

        }

        [Fact]
        public async Task ShouldMigrateBloodPressures()
        {
            //Given
            var minBloodPressureDate = new DateTime(2012, 1, 1);
            var searchDaysPrevious = 10;
            var latestBloodPRessureDate = new DateTime(2019, 1, 1);
            IEnumerable<BloodPressure> someBloodpressures = new List<BloodPressure>();

            _healthService.Setup(x => x.GetLatestBloodPressureDate(minBloodPressureDate)).Returns(latestBloodPRessureDate);
            _withingsService.Setup(x => x.GetBloodPressures(latestBloodPRessureDate.AddDays(-searchDaysPrevious))).Returns(Task.FromResult(someBloodpressures));

            //When
            await _withingsController.MigrateBloodPressures();

            //Then
            _healthService.Verify(x => x.UpsertAsync(someBloodpressures), Times.Once);


        }

        [Fact]
        public async Task ShouldMigrateWeights()
        {
            //Given
            var minWeightDate = new DateTime(2012, 1, 1);
            var searchDaysPrevious = 10;
            var latestWeightDate = new DateTime(2019, 1, 1);
            IEnumerable<Weight> someWeights = new List<Weight>();

            _healthService.Setup(x => x.GetLatestWeightDate(minWeightDate)).Returns(latestWeightDate);
            _withingsService.Setup(x => x.GetWeights(latestWeightDate.AddDays(-searchDaysPrevious))).Returns(Task.FromResult(someWeights));

            //When
            await _withingsController.MigrateWeights();

            //Then
            _healthService.Verify(x => x.UpsertAsync(someWeights), Times.Once);


        }

    }
}