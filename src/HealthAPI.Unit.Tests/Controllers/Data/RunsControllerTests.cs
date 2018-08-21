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
    public class RunsControllerTests
    {
        private Mock<IHealthService> _healthService;
        private RunsController _controller;

        public RunsControllerTests()
        {
            _healthService = new Mock<IHealthService>();

            _controller = new RunsController(_healthService.Object);
        }


        [Fact]
        public void ShouldGetRuns()
        {

            var someRuns = new List<Run>
            {
                new Run { CreatedDate = new DateTime(2018, 6, 1), Metres = 1 },
                new Run { CreatedDate = new DateTime(2018, 6, 2), Metres = 2 },
                new Run { CreatedDate = new DateTime(2018, 6, 3), Metres = 3 },
                new Run { CreatedDate = new DateTime(2018, 6, 4), Metres = 4 },
                new Run { CreatedDate = new DateTime(2018, 6, 5), Metres = 5 }
            };

            _healthService.Setup(x => x.GetAllRuns()).Returns(someRuns);

            var result = (JsonResult)_controller.Get();

            var runs = (List<Run>)result.Value;

            Assert.Equal(5, runs.Count);
            Assert.Equal(1, runs[0].Metres);
            Assert.Equal(2, runs[1].Metres);
            Assert.Equal(3, runs[2].Metres);
            Assert.Equal(4, runs[3].Metres);
            Assert.Equal(5, runs[4].Metres);
        }
    }
}