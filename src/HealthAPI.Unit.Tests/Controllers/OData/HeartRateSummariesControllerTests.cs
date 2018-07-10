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
    public class HeartRateSummariesControllerTests
    {
        private Mock<IHealthService> _healthService;
        private HeartRateSummariesController _controller;

        public HeartRateSummariesControllerTests()
        {
            _healthService = new Mock<IHealthService>();

            _controller = new HeartRateSummariesController(_healthService.Object);
        }


        [Fact]
        public void ShouldGetHeartRateSummaries()
        {

            var someHeartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 1), CardioMinutes = 1 },
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 2), CardioMinutes = 2 },
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 3), CardioMinutes = 3 },
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 4), CardioMinutes = 4 },
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 5), CardioMinutes = 5 }
            };

            _healthService.Setup(x => x.GetAllHeartRateSummaries()).Returns(someHeartRateSummaries);

            var result = (JsonResult)_controller.Get();

            List<HeartRateSummary> heartRateSummaries = (List<HeartRateSummary>)result.Value;

            Assert.Equal(5, heartRateSummaries.Count);
            Assert.Equal(1, heartRateSummaries[0].CardioMinutes);
            Assert.Equal(2, heartRateSummaries[1].CardioMinutes);
            Assert.Equal(3, heartRateSummaries[2].CardioMinutes);
            Assert.Equal(4, heartRateSummaries[3].CardioMinutes);
            Assert.Equal(5, heartRateSummaries[4].CardioMinutes);
        }

        [Fact]
        public void ShouldGetHeartRateSummariesByWeek()
        {
            var someAlcoholIntake = new List<HeartRateSummary>
            {
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 1), CardioMinutes = 6 },
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 2), CardioMinutes = 7 },
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 3), CardioMinutes = 8 },
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 4), CardioMinutes = 9 }
            };

            _healthService.Setup(x => x.GetAllHeartRateSummariesByWeek()).Returns(someAlcoholIntake);

            var result = (JsonResult)_controller.GetByWeek();

            List<HeartRateSummary> heartRateSummaries = (List<HeartRateSummary>)result.Value;

            Assert.Equal(4, heartRateSummaries.Count);
            Assert.Equal(6, heartRateSummaries[0].CardioMinutes);
            Assert.Equal(7, heartRateSummaries[1].CardioMinutes);
            Assert.Equal(8, heartRateSummaries[2].CardioMinutes);
            Assert.Equal(9, heartRateSummaries[3].CardioMinutes);

        }

        [Fact]
        public void ShouldGetHeartRateSummariesByMonth()
        {

            var someHeartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 1), CardioMinutes = 10 },
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 2), CardioMinutes = 11 },
                new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 3), CardioMinutes = 12 }
            };

            _healthService.Setup(x => x.GetAllHeartRateSummariesByMonth()).Returns(someHeartRateSummaries);

            var result = (JsonResult)_controller.GetByMonth();

            List<HeartRateSummary> heartRateSummaries = (List<HeartRateSummary>)result.Value;

            Assert.Equal(3, heartRateSummaries.Count);
            Assert.Equal(10, heartRateSummaries[0].CardioMinutes);
            Assert.Equal(11, heartRateSummaries[1].CardioMinutes);
            Assert.Equal(12, heartRateSummaries[2].CardioMinutes);
        }
    }
}
