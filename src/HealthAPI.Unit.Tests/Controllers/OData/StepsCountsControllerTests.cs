using System;
using System.Linq;
using HealthAPI.Controllers.OData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Repositories.Models;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.OData
{
    public class StepCountsControllerTests : IDisposable
    {
        private StepCountsController _controller;
        private FakeLocalContext _fakeLocalContext;

        public StepCountsControllerTests()
        {
            _fakeLocalContext = new FakeLocalContext();
            _controller = new StepCountsController(_fakeLocalContext);
        }

        public void Dispose()
        {
            _fakeLocalContext.Database.EnsureDeleted();
        }

        [Fact]
        public void ShouldGroupByMonth()
        {
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 1, 1), Count = 1 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 1, 2), Count = 2 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 1, 3), Count = 3 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 2, 1), Count = 4 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 2, 2), Count = 5 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 2, 3), Count = 6 });
            _fakeLocalContext.SaveChanges();

            var stepCounts = _controller.GetByMonth();

            // var alcoholIntakes = response as AlcoholIntake[] ?? response.ToArray();
            Assert.Equal(2, stepCounts.Count());
            Assert.Equal(2, stepCounts.First(x => x.DateTime == new DateTime(2018, 1, 1)).Count);
            Assert.Equal(5, stepCounts.First(x => x.DateTime == new DateTime(2018, 2, 1)).Count);

        }

        [Fact]
        public void ShouldGroupByWeek()
        {

            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 11), Count = 1 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 12), Count = 2 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 13), Count = 3 });

            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 18), Count = 4 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 19), Count = 5 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 20), Count = 6 });

            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 25), Count = 7 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 26), Count = 8 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 27), Count = 9 });
            _fakeLocalContext.SaveChanges();
            
            var stepCounts = _controller.GetByWeek();

            Assert.Equal(3, stepCounts.Count());
            Assert.Equal(6, stepCounts.First(x => x.DateTime == new DateTime(2018, 6, 11)).Count);
            Assert.Equal(15, stepCounts.First(x => x.DateTime == new DateTime(2018, 6, 18)).Count);
            Assert.Equal(24, stepCounts.First(x => x.DateTime == new DateTime(2018, 6, 25)).Count);

        }

        [Fact]
        public void ShouldGetStepCounts()
        {

            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 1), Count = 1 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 2), Count = 2 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 3), Count = 3 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 4), Count = 4 });
            _fakeLocalContext.Add(new StepCount { DateTime = new DateTime(2018, 6, 5), Count = 5 });
            _fakeLocalContext.SaveChanges();

            var stepCounts = _controller.Get();

            Assert.Equal(5, stepCounts.Count());
            Assert.Equal(1, stepCounts.First(x => x.DateTime == new DateTime(2018, 6, 1)).Count);
            Assert.Equal(2, stepCounts.First(x => x.DateTime == new DateTime(2018, 6, 2)).Count);
            Assert.Equal(3, stepCounts.First(x => x.DateTime == new DateTime(2018, 6, 3)).Count);
            Assert.Equal(4, stepCounts.First(x => x.DateTime == new DateTime(2018, 6, 4)).Count);
            Assert.Equal(5, stepCounts.First(x => x.DateTime == new DateTime(2018, 6, 5)).Count);

        }

    }
}