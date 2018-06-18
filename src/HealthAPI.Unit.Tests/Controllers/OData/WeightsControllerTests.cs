using HealthAPI.Controllers.OData;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.OData
{
    public class WeightsControllerTests : IDisposable
    {
        private WeightsController _controller;
        private FakeLocalContext _fakeLocalContext;

        public WeightsControllerTests()
        {
            _fakeLocalContext = new FakeLocalContext();
            _controller = new WeightsController(_fakeLocalContext);
        }

        public void Dispose()
        {
            _fakeLocalContext.Database.EnsureDeleted();
        }

        [Fact]
        public void ShouldGetBloodPressures()
        {

            _fakeLocalContext.Add(new Weight { DateTime = new DateTime(2018, 6, 1), Kg = 1 });
            _fakeLocalContext.Add(new Weight { DateTime = new DateTime(2018, 6, 2), Kg = 2 });
            _fakeLocalContext.Add(new Weight { DateTime = new DateTime(2018, 6, 3), Kg = 3 });
            _fakeLocalContext.Add(new Weight { DateTime = new DateTime(2018, 6, 4), Kg = 4 });
            _fakeLocalContext.Add(new Weight { DateTime = new DateTime(2018, 6, 5), Kg = 5 });
            _fakeLocalContext.SaveChanges();

            var weights = _controller.Get();

            Assert.Equal(5, weights.Count());
            Assert.Equal(1, weights.First(x => x.DateTime == new DateTime(2018, 6, 1)).Kg);
            Assert.Equal(2, weights.First(x => x.DateTime == new DateTime(2018, 6, 2)).Kg);
            Assert.Equal(3, weights.First(x => x.DateTime == new DateTime(2018, 6, 3)).Kg);
            Assert.Equal(4, weights.First(x => x.DateTime == new DateTime(2018, 6, 4)).Kg);
            Assert.Equal(5, weights.First(x => x.DateTime == new DateTime(2018, 6, 5)).Kg);
        }

    }
}
