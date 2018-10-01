using System;
using System.Collections.Generic;
using System.Text;
using Google;
using Moq;
using Repositories.Models;
using Services.Google;
using Services.Health;
using Xunit;

namespace Migrators.Unit.Tests
{
    public class GoogleMigratorTests
    {
        private Mock<IGoogleClient> _googleClient;
        private Mock<IHealthService> _healthService;
        private GoogleMigrator _googleMigrator;

        public GoogleMigratorTests()
        {
            _googleClient = new Mock<IGoogleClient>();
            _healthService = new Mock<IHealthService>();
            _googleMigrator = new GoogleMigrator(_googleClient.Object, _healthService.Object);
        }

        [Fact]
        public void ShouldMigrateRuns()
        {
            //given
            var someRuns = new List<Run>
            {
                new Run{CreatedDate = new DateTime(2018,1,1), Time = new TimeSpan(1,2,3), Metres = 123},
                new Run{CreatedDate = new DateTime(2018,1,2), Time = new TimeSpan(1,2,4), Metres = 124}
            };
            _googleClient.Setup(x => x.GetRuns()).Returns(someRuns);

            _googleMigrator.MigrateRuns();

            _healthService.Verify(x=>x.UpsertRuns(someRuns), Times.Once);
        }

        [Fact]
        public void ShouldMigrateErgos()
        {
            //given
            var someErgos = new List<Ergo>
            {
                new Ergo{CreatedDate = new DateTime(2018,1,1), Time = new TimeSpan(1,2,3), Metres = 123},
                new Ergo{CreatedDate = new DateTime(2018,1,2), Time = new TimeSpan(1,2,4), Metres = 124}
            };
            _googleClient.Setup(x => x.GetErgos()).Returns(someErgos);

            _googleMigrator.MigrateErgos();

            _healthService.Verify(x => x.UpsertErgos(someErgos), Times.Once);
        }

        [Fact]
        public void ShouldMigrateAlcoholIntakes()
        {
            //given
            var someAlcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake{CreatedDate = new DateTime(2018,1,1), Units = 123},
                new AlcoholIntake{CreatedDate = new DateTime(2018,1,2), Units = 124}
            };
            _googleClient.Setup(x => x.GetAlcoholIntakes()).Returns(someAlcoholIntakes);

            _googleMigrator.MigrateAlcoholIntakes();

            _healthService.Verify(x => x.UpsertAlcoholIntakes(someAlcoholIntakes), Times.Once);
        }
    }
}
