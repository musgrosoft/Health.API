using Repositories.Models;
using System;
using System.Collections.Generic;
using HealthAPI.Controllers.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.MyHealth;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.OData
{
    public class RestingHeartRatesControllerTests
    {
        private Mock<IHealthService> _healthService;
        private RestingHeartRatesController _controller;

        public RestingHeartRatesControllerTests()
        {
            _healthService = new Mock<IHealthService>();

            _controller = new RestingHeartRatesController(_healthService.Object);
        }


        [Fact]
        public void ShouldGetRestingHeartRatess()
        {

            var someRestingHeartRatess = new List<RestingHeartRate>
            {
                new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 1), Beats = 1 },
                new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 2), Beats = 2 },
                new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 3), Beats = 3 },
                new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 4), Beats = 4 },
                new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 5), Beats = 5 }
            };

            _healthService.Setup(x => x.GetAllRestingHeartRates()).Returns(someRestingHeartRatess);

            var result = (JsonResult)_controller.Get();

            List<RestingHeartRate> restingHeartRates = (List<RestingHeartRate>)result.Value;

            Assert.Equal(5, restingHeartRates.Count);
            Assert.Equal(1, restingHeartRates[0].Beats);
            Assert.Equal(2, restingHeartRates[1].Beats);
            Assert.Equal(3, restingHeartRates[2].Beats);
            Assert.Equal(4, restingHeartRates[3].Beats);
            Assert.Equal(5, restingHeartRates[4].Beats);
        }

    }
}
