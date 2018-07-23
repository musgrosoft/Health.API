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
    public class StepCountsControllerTests
    {
        private Mock<IHealthService> _healthService;
        private StepCountsController _controller;

        public StepCountsControllerTests()
        {
            _healthService = new Mock<IHealthService>();

            _controller = new StepCountsController(_healthService.Object);
        }


        [Fact]
        public void ShouldGetStepCounts()
        {

            var someStepCounts = new List<StepCount>
            {
                new StepCount { CreatedDate = new DateTime(2018, 6, 1), Count = 1 },
                new StepCount { CreatedDate = new DateTime(2018, 6, 2), Count = 2 },
                new StepCount { CreatedDate = new DateTime(2018, 6, 3), Count = 3 },
                new StepCount { CreatedDate = new DateTime(2018, 6, 4), Count = 4 },
                new StepCount { CreatedDate = new DateTime(2018, 6, 5), Count = 5 }
            };

            _healthService.Setup(x => x.GetAllStepCounts()).Returns(someStepCounts);

            var result = (JsonResult)_controller.Get();

            List<StepCount> stepCounts = (List<StepCount>)result.Value;

            Assert.Equal(5, stepCounts.Count);
            Assert.Equal(1, stepCounts[0].Count);
            Assert.Equal(2, stepCounts[1].Count);
            Assert.Equal(3, stepCounts[2].Count);
            Assert.Equal(4, stepCounts[3].Count);
            Assert.Equal(5, stepCounts[4].Count);
        }

        [Fact]
        public void ShouldGetStepCountsByWeek()
        {
            var someStepCounts = new List<StepCount>
            {
                new StepCount { CreatedDate = new DateTime(2018, 6, 1), Count = 6 },
                new StepCount { CreatedDate = new DateTime(2018, 6, 2), Count = 7 },
                new StepCount { CreatedDate = new DateTime(2018, 6, 3), Count = 8 },
                new StepCount { CreatedDate = new DateTime(2018, 6, 4), Count = 9 }
            };

            _healthService.Setup(x => x.GetAllStepCountsByWeek()).Returns(someStepCounts);

            var result = (JsonResult)_controller.GetByWeek();

            List<StepCount> alcoholIntakes = (List<StepCount>)result.Value;

            Assert.Equal(4, alcoholIntakes.Count);
            Assert.Equal(6, alcoholIntakes[0].Count);
            Assert.Equal(7, alcoholIntakes[1].Count);
            Assert.Equal(8, alcoholIntakes[2].Count);
            Assert.Equal(9, alcoholIntakes[3].Count);

        }

        [Fact]
        public void ShouldGetStepCountsByMonth()
        {

            var someStepCounts = new List<StepCount>
            {
                new StepCount { CreatedDate = new DateTime(2018, 6, 1), Count = 10 },
                new StepCount { CreatedDate = new DateTime(2018, 6, 2), Count = 11 },
                new StepCount { CreatedDate = new DateTime(2018, 6, 3), Count = 12 }
            };

            _healthService.Setup(x => x.GetAllStepCountsByMonth()).Returns(someStepCounts);

            var result = (JsonResult)_controller.GetByMonth();

            List<StepCount> alcoholIntakes = (List<StepCount>)result.Value;

            Assert.Equal(3, alcoholIntakes.Count);
            Assert.Equal(10, alcoholIntakes[0].Count);
            Assert.Equal(11, alcoholIntakes[1].Count);
            Assert.Equal(12, alcoholIntakes[2].Count);
        }
    }
}
