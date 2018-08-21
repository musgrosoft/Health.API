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
    public class ErgosControllerTests
    {
        private Mock<IHealthService> _healthService;
        private ErgosController _controller;

        public ErgosControllerTests()
        {
            _healthService = new Mock<IHealthService>();

            _controller = new ErgosController(_healthService.Object);
        }
        
        [Fact]
        public void ShouldGetErgos()
        {

            var someErgos = new List<Ergo>
            {
                new Ergo { CreatedDate = new DateTime(2018, 6, 1), Metres = 1 },
                new Ergo { CreatedDate = new DateTime(2018, 6, 2), Metres = 2 },
                new Ergo { CreatedDate = new DateTime(2018, 6, 3), Metres = 3 },
                new Ergo { CreatedDate = new DateTime(2018, 6, 4), Metres = 4 },
                new Ergo { CreatedDate = new DateTime(2018, 6, 5), Metres = 5 }
            };

            _healthService.Setup(x => x.GetAllErgos()).Returns(someErgos);

            var result = (JsonResult)_controller.Get();

            List<Ergo> ergos = (List<Ergo>)result.Value;

            Assert.Equal(5, ergos.Count);
            Assert.Equal(1, ergos[0].Metres);
            Assert.Equal(2, ergos[1].Metres);
            Assert.Equal(3, ergos[2].Metres);
            Assert.Equal(4, ergos[3].Metres);
            Assert.Equal(5, ergos[4].Metres);
        }
    }
}