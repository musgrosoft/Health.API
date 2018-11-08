using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Nokia.Importer;
using Nokia.Services;
using Repositories.Health.Models;
using Services.Health;
using Utils;
using Xunit;

namespace Nokia.Unit.Tests
{
    public class NokiaMigratorTests
    {
        private Mock<IHealthService> _healthService;
        private Mock<INokiaService> _nokiaService;
        private Mock<ILogger> _logger;
        private NokiaMigrator _nokiaMigrator;

        private const int SEARCH_DAYS_PREVIOUS = 10;

        public NokiaMigratorTests()
        {
            _healthService = new Mock<IHealthService>();
            _nokiaService = new Mock<INokiaService>();
            _logger = new Mock<ILogger>();

            _nokiaMigrator = new NokiaMigrator(_healthService.Object, _logger.Object, _nokiaService.Object);
        }

        [Fact]
        public async Task ShouldMigrateWeights()
        {
            var latestWeightDate = new DateTime(2010,12,1);
            
            _healthService.Setup(x => x.GetLatestWeightDate(It.IsAny<DateTime>())).Returns(latestWeightDate);

            var weights = new List<Weight>()
            {
                new Weight { CreatedDate = new DateTime(2014,1,1), Kg = 123},
                new Weight { CreatedDate = new DateTime(2015,1,1), Kg = 111},
                new Weight { CreatedDate = new DateTime(2016,1,1), Kg = 234}
            };
            
            _nokiaService.Setup(x => x.GetWeights(latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS))).Returns(Task.FromResult((IEnumerable<Weight>)weights));

           // _targetService.Setup(x => x.SetTargets(weights)).Returns(weights);

            await _nokiaMigrator.MigrateWeights();


            _healthService.Verify(x=>x.UpsertWeights(weights));
        }

        [Fact]
        public async Task ShouldMigrateBloodPressures()
        {
            var latestBloodPressureDate = new DateTime(2011, 2, 3);

            _healthService.Setup(x => x.GetLatestBloodPressureDate(It.IsAny<DateTime>())).Returns(latestBloodPressureDate);

            var bloodPressures = new List<BloodPressure>()
            {
                new BloodPressure {CreatedDate = new DateTime(2014,1,1), Systolic = 123, Diastolic = 234},
                new BloodPressure {CreatedDate = new DateTime(2015,1,1), Systolic = 111, Diastolic = 234},
                new BloodPressure {CreatedDate = new DateTime(2016,1,1), Systolic = 222, Diastolic = 234}
            };

            _nokiaService.Setup(x => x.GetBloodPressures(latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS))).Returns(Task.FromResult((IEnumerable<BloodPressure>)bloodPressures));

            await _nokiaMigrator.MigrateBloodPressures();

            _healthService.Verify(x => x.UpsertBloodPressures(bloodPressures));

        }


    }
}