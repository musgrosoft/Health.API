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
    public class BloodPressuresControllerTests
    {
        private Mock<IHealthService> _healthService;
        private BloodPressuresController _controller;

        public BloodPressuresControllerTests()
        {
            _healthService = new Mock<IHealthService>();

            _controller = new BloodPressuresController(_healthService.Object);
        }


        [Fact]
        public void ShouldGetBloodPressures()
        {

            var someBloodPressures = new List<BloodPressure>
            {
                new BloodPressure { CreatedDate = new DateTime(2018, 6, 1), Systolic = 1 },
                new BloodPressure { CreatedDate = new DateTime(2018, 6, 2), Systolic = 2 },
                new BloodPressure { CreatedDate = new DateTime(2018, 6, 3), Systolic = 3 },
                new BloodPressure { CreatedDate = new DateTime(2018, 6, 4), Systolic = 4 },
                new BloodPressure { CreatedDate = new DateTime(2018, 6, 5), Systolic = 5 }
            };

            _healthService.Setup(x => x.GetAllBloodPressures()).Returns(someBloodPressures);

            var result = (JsonResult)_controller.Get();

            List<BloodPressure> bloodPressures = (List<BloodPressure>)result.Value;

            Assert.Equal(5, bloodPressures.Count);
            Assert.Equal(1, bloodPressures[0].Systolic);
            Assert.Equal(2, bloodPressures[1].Systolic);
            Assert.Equal(3, bloodPressures[2].Systolic);
            Assert.Equal(4, bloodPressures[3].Systolic);
            Assert.Equal(5, bloodPressures[4].Systolic);
        }

    }
}
