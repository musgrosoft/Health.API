using System;
using System.Linq;
using HealthAPI.Controllers.OData;
using Repositories.Models;
using Xunit;

namespace HealthAPI.Unit.Tests.Controllers.OData
{
    public class DailyActivitiesControllerTests : IDisposable
    {
        private ActivitySummariesController _controller;
        private FakeLocalContext _fakeLocalContext;

        public DailyActivitiesControllerTests()
        {
            _fakeLocalContext = new FakeLocalContext();
            _controller = new ActivitySummariesController(_fakeLocalContext);
        }

        public void Dispose()
        {
            _fakeLocalContext.Database.EnsureDeleted();
        }

        [Fact]
        public void ShouldGroupByMonth()
        {
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 1, 1), VeryActiveMinutes = 1 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 1, 2), VeryActiveMinutes = 2 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 1, 3), VeryActiveMinutes = 3 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 2, 1), VeryActiveMinutes = 4 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 2, 2), VeryActiveMinutes = 5 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 2, 3), VeryActiveMinutes = 6 });
            _fakeLocalContext.SaveChanges();

            var activitySummaries = _controller.GetByMonth();

            // var alcoholIntakes = response as AlcoholIntake[] ?? response.ToArray();
            Assert.Equal(2, activitySummaries.Count());
            Assert.Equal(2, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 1, 1)).VeryActiveMinutes);
            Assert.Equal(5, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 2, 1)).VeryActiveMinutes);

        }

        [Fact]
        public void ShouldGroupByWeek()
        {

            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 11), VeryActiveMinutes = 1 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 12), VeryActiveMinutes = 2 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 13), VeryActiveMinutes = 3 });

            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 18), VeryActiveMinutes = 4 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 19), VeryActiveMinutes = 5 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 20), VeryActiveMinutes = 6 });

            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 25), VeryActiveMinutes = 7 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 26), VeryActiveMinutes = 8 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 27), VeryActiveMinutes = 9 });
            _fakeLocalContext.SaveChanges();
            
            var activitySummaries = _controller.GetByWeek();

            Assert.Equal(3, activitySummaries.Count());
            Assert.Equal(6, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 11)).VeryActiveMinutes);
            Assert.Equal(15, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 18)).VeryActiveMinutes);
            Assert.Equal(24, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 25)).VeryActiveMinutes);

        }

        [Fact]
        public void ShouldGetAlcoholIntakes()
        {

            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 1), VeryActiveMinutes = 1 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 2), VeryActiveMinutes = 2 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 3), VeryActiveMinutes = 3 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 4), VeryActiveMinutes = 4 });
            _fakeLocalContext.Add(new ActivitySummary { CreatedDate = new DateTime(2018, 6, 5), VeryActiveMinutes = 5 });
            _fakeLocalContext.SaveChanges();

            var activitySummaries = _controller.Get();

            Assert.Equal(5, activitySummaries.Count());
            Assert.Equal(1, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 1)).VeryActiveMinutes);
            Assert.Equal(2, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 2)).VeryActiveMinutes);
            Assert.Equal(3, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 3)).VeryActiveMinutes);
            Assert.Equal(4, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 4)).VeryActiveMinutes);
            Assert.Equal(5, activitySummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 5)).VeryActiveMinutes);

        }

    }
}