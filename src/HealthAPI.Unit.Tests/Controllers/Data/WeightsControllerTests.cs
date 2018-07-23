using System;
using System.Collections.Generic;
using HealthAPI.Controllers.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.Models;
using Services.MyHealth;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.Data
{
    public class WeightsControllerTests
    {
        private Mock<IHealthService> _healthService;
        private WeightsController _controller;

        public WeightsControllerTests()
        {
            _healthService = new Mock<IHealthService>();

            _controller = new WeightsController(_healthService.Object);
        }


        [Fact]
        public void ShouldGetWeights()
        {

            var someWeights = new List<Weight>
            {
                new Weight { CreatedDate = new DateTime(2018, 6, 1), Kg = 1 },
                new Weight { CreatedDate = new DateTime(2018, 6, 2), Kg = 2 },
                new Weight { CreatedDate = new DateTime(2018, 6, 3), Kg = 3 },
                new Weight { CreatedDate = new DateTime(2018, 6, 4), Kg = 4 },
                new Weight { CreatedDate = new DateTime(2018, 6, 5), Kg = 5 }
            };

            _healthService.Setup(x => x.GetAllWeights()).Returns(someWeights);

            var result = (JsonResult)_controller.Get();

            List<Weight> weights = (List<Weight>)result.Value;

            Assert.Equal(5, weights.Count);
            Assert.Equal(1, weights[0].Kg);
            Assert.Equal(2, weights[1].Kg);
            Assert.Equal(3, weights[2].Kg);
            Assert.Equal(4, weights[3].Kg);
            Assert.Equal(5, weights[4].Kg);
        }

    }
}
