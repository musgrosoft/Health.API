using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Repositories.Models;
using Services.MyHealth;
using Services.Nokia;
using Utils;
using Xunit;

namespace Migrators.Unit.Tests
{
    public class NokiaMigratorTests
    {
        private Mock<IHealthService> _healthService;
        private Mock<INokiaClient> _nokiaClient;
        private Mock<ILogger> _logger;
        private NokiaMigrator _nokiaMigrator;

        private const int SEARCH_DAYS_PREVIOUS = 10;

        public NokiaMigratorTests()
        {
            _healthService = new Mock<IHealthService>();
            _nokiaClient = new Mock<INokiaClient>();
            _logger = new Mock<ILogger>();

            _nokiaMigrator = new NokiaMigrator(_healthService.Object, _logger.Object, _nokiaClient.Object);
        }

        [Fact]
        public async Task ShouldMigrateWeights()
        {
            var latestWeightDate = new DateTime(2010,12,1);
            
            _healthService.Setup(x => x.GetLatestWeightDate(It.IsAny<DateTime>())).Returns(latestWeightDate);

            var weights = new List<Weight>()
            {
                new Weight { DateTime = new DateTime(2014,1,1), Kg = 123},
                new Weight { DateTime = new DateTime(2015,1,1), Kg = 111},
                new Weight { DateTime = new DateTime(2016,1,1), Kg = 234}
            };
            
            _nokiaClient.Setup(x => x.GetWeights(latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS))).Returns(Task.FromResult((IEnumerable<Weight>)weights));

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
                new BloodPressure {DateTime = new DateTime(2014,1,1), Systolic = 123, Diastolic = 234},
                new BloodPressure {DateTime = new DateTime(2015,1,1), Systolic = 111, Diastolic = 234},
                new BloodPressure {DateTime = new DateTime(2016,1,1), Systolic = 222, Diastolic = 234}
            };

            _nokiaClient.Setup(x => x.GetBloodPressures(latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS))).Returns(Task.FromResult((IEnumerable<BloodPressure>)bloodPressures));

            await _nokiaMigrator.MigrateBloodPressures();

            _healthService.Verify(x => x.UpsertBloodPressures(bloodPressures));

        }


    }
}