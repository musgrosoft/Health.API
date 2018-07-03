using System;
using System.Linq;
using HealthAPI.Controllers.OData;
using Repositories.Models;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.OData
{
    public class RestingHeartRatesControllerTests
    {
        private RestingHeartRatesController _controller;
        private FakeLocalContext _fakeLocalContext;

        public RestingHeartRatesControllerTests()
        {
            _fakeLocalContext = new FakeLocalContext();
            _controller = new RestingHeartRatesController(_fakeLocalContext);
        }

        public void Dispose()
        {
            _fakeLocalContext.Database.EnsureDeleted();
        }

        [Fact]
        public void ShouldGetBloodPressures()
        {

            _fakeLocalContext.Add(new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 1), Beats = 1 });
            _fakeLocalContext.Add(new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 2), Beats = 2 });
            _fakeLocalContext.Add(new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 3), Beats = 3 });
            _fakeLocalContext.Add(new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 4), Beats = 4 });
            _fakeLocalContext.Add(new RestingHeartRate { CreatedDate = new DateTime(2018, 6, 5), Beats = 5 });
            _fakeLocalContext.SaveChanges();

            var restingHeartRates = _controller.Get();

            Assert.Equal(5, restingHeartRates.Count());
            Assert.Equal(1, restingHeartRates.First(x => x.CreatedDate == new DateTime(2018, 6, 1)).Beats);
            Assert.Equal(2, restingHeartRates.First(x => x.CreatedDate == new DateTime(2018, 6, 2)).Beats);
            Assert.Equal(3, restingHeartRates.First(x => x.CreatedDate == new DateTime(2018, 6, 3)).Beats);
            Assert.Equal(4, restingHeartRates.First(x => x.CreatedDate == new DateTime(2018, 6, 4)).Beats);
            Assert.Equal(5, restingHeartRates.First(x => x.CreatedDate == new DateTime(2018, 6, 5)).Beats);
        }
    }
}