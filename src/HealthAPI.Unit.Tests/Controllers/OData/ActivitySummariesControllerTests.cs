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
    public class ActivitySummariesControllerTests
    {
        private Mock<IHealthService> _healthService;
        private ActivitySummariesController _controller;

        public ActivitySummariesControllerTests()
        {
            _healthService = new Mock<IHealthService>();

            _controller = new ActivitySummariesController(_healthService.Object);
        }


        [Fact]
        public void ShouldGetBloodPressures()
        {

            var someActivitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 1), VeryActiveMinutes = 1 },
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 2), VeryActiveMinutes = 2 },
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 3), VeryActiveMinutes = 3 },
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 4), VeryActiveMinutes = 4 },
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 5), VeryActiveMinutes = 5 }
            };

            _healthService.Setup(x => x.GetAllActivitySummaries()).Returns(someActivitySummaries);

            var result = (JsonResult)_controller.Get();

            List<ActivitySummary> activitySummaries = (List<ActivitySummary>)result.Value;

            Assert.Equal(5, activitySummaries.Count);
            Assert.Equal(1, activitySummaries[0].VeryActiveMinutes);
            Assert.Equal(2, activitySummaries[1].VeryActiveMinutes);
            Assert.Equal(3, activitySummaries[2].VeryActiveMinutes);
            Assert.Equal(4, activitySummaries[3].VeryActiveMinutes);
            Assert.Equal(5, activitySummaries[4].VeryActiveMinutes);
        }

        [Fact]
        public void ShouldGetBloodPressuresByWeek()
        {
            var someActivitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 1), VeryActiveMinutes = 6 },
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 2), VeryActiveMinutes = 7 },
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 3), VeryActiveMinutes = 8 },
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 4), VeryActiveMinutes = 9 }
            };

            _healthService.Setup(x => x.GetAllActivitySummariesByWeek()).Returns(someActivitySummaries);

            var result = (JsonResult)_controller.GetByWeek();

            List<ActivitySummary> activitySummaries = (List<ActivitySummary>)result.Value;

            Assert.Equal(4, activitySummaries.Count);
            Assert.Equal(6, activitySummaries[0].VeryActiveMinutes);
            Assert.Equal(7, activitySummaries[1].VeryActiveMinutes);
            Assert.Equal(8, activitySummaries[2].VeryActiveMinutes);
            Assert.Equal(9, activitySummaries[3].VeryActiveMinutes);

        }

        [Fact]
        public void ShouldGetBloodPressuresByMonth()
        {

            var someActivitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 1), VeryActiveMinutes = 10 },
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 2), VeryActiveMinutes = 11 },
                new ActivitySummary { CreatedDate = new DateTime(2018, 6, 3), VeryActiveMinutes = 12 }
            };

            _healthService.Setup(x => x.GetAllActivitySummariesByMonth()).Returns(someActivitySummaries);

            var result = (JsonResult)_controller.GetByMonth();

            List<ActivitySummary> activitySummaries = (List<ActivitySummary>)result.Value;

            Assert.Equal(3, activitySummaries.Count);
            Assert.Equal(10, activitySummaries[0].VeryActiveMinutes);
            Assert.Equal(11, activitySummaries[1].VeryActiveMinutes);
            Assert.Equal(12, activitySummaries[2].VeryActiveMinutes);
        }
    }
}
