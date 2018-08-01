using System;
using System.Collections.Generic;
using HealthAPI.Controllers.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.Models;
using Services.Health;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.Data
{
    public class AlcoholIntakesControllerTests
    {
        private Mock<IHealthService> _healthService;
        private AlcoholIntakesController _controller;

        public AlcoholIntakesControllerTests()
        {
            _healthService = new Mock<IHealthService>();

            _controller = new AlcoholIntakesController(_healthService.Object);
        }


        [Fact]
        public void ShouldGetAlcoholIntakes()
        {

            var someAlcoholIntake = new List<AlcoholIntake>
            {
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 1), Units = 1 },
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 2), Units = 2 },
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 3), Units = 3 },
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 4), Units = 4 },
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 5), Units = 5 }
            };

            _healthService.Setup(x => x.GetAllAlcoholIntakes()).Returns(someAlcoholIntake);

            var result = (JsonResult)_controller.Get();

            List<AlcoholIntake> alcoholIntakes = (List<AlcoholIntake>)result.Value;

            Assert.Equal(5, alcoholIntakes.Count);
            Assert.Equal(1, alcoholIntakes[0].Units);
            Assert.Equal(2, alcoholIntakes[1].Units);
            Assert.Equal(3, alcoholIntakes[2].Units);
            Assert.Equal(4, alcoholIntakes[3].Units);
            Assert.Equal(5, alcoholIntakes[4].Units);
        }

        [Fact]
        public void ShouldGetAlcoholIntakesByWeek()
        {
            var someAlcoholIntake = new List<AlcoholIntake>
            {
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 1), Units = 6 },
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 2), Units = 7 },
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 3), Units = 8 },
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 4), Units = 9 }
            };

            _healthService.Setup(x => x.GetAllAlcoholIntakesByWeek()).Returns(someAlcoholIntake);

            var result = (JsonResult)_controller.GetByWeek();

            List<AlcoholIntake> alcoholIntakes = (List<AlcoholIntake>)result.Value;

            Assert.Equal(4, alcoholIntakes.Count);
            Assert.Equal(6, alcoholIntakes[0].Units);
            Assert.Equal(7, alcoholIntakes[1].Units);
            Assert.Equal(8, alcoholIntakes[2].Units);
            Assert.Equal(9, alcoholIntakes[3].Units);

        }

        [Fact]
        public void ShouldGetAlcoholIntakesByMonth()
        {

            var someAlcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 1), Units = 10 },
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 2), Units = 11 },
                new AlcoholIntake { CreatedDate = new DateTime(2018, 6, 3), Units = 12 }
            };

            _healthService.Setup(x => x.GetAllAlcoholIntakesByMonth()).Returns(someAlcoholIntakes);

            var result = (JsonResult)_controller.GetByMonth();

            List<AlcoholIntake> alcoholIntakes = (List<AlcoholIntake>)result.Value;

            Assert.Equal(3, alcoholIntakes.Count);
            Assert.Equal(10, alcoholIntakes[0].Units);
            Assert.Equal(11, alcoholIntakes[1].Units);
            Assert.Equal(12, alcoholIntakes[2].Units);
        }
    }
}
