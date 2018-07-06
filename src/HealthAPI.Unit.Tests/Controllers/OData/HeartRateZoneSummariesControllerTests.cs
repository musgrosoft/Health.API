//using System;
//using System.Linq;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.TestHost;
//using Repositories.Models;
//using Xunit;



//namespace HealthAPI.Unit.Tests.Controllers.OData
//{
//    public class HeartRateSummariesControllerTests : IDisposable
//    {
//        private HeartRateSummariesController _controller;
//        private FakeLocalContext _fakeLocalContext;

//        public HeartRateSummariesControllerTests()
//        {
//            _fakeLocalContext = new FakeLocalContext();
//            _controller = new HeartRateSummariesController(_fakeLocalContext);
//        }

//        public void Dispose()
//        {
//            _fakeLocalContext.Database.EnsureDeleted();
//        }

//        [Fact]
//        public void ShouldGroupByMonth()
//        {
//            _fakeLocalContext.Database.EnsureDeleted();
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 1, 1), CardioMinutes = 1 , FatBurnMinutes = 0, PeakMinutes = 0 });
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 1, 2), CardioMinutes = 2 , FatBurnMinutes = 0, PeakMinutes = 0 });
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 1, 3), CardioMinutes = 3 , FatBurnMinutes = 0, PeakMinutes = 0 });
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 2, 1), CardioMinutes = 4 , FatBurnMinutes = 0, PeakMinutes = 0 });
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 2, 2), CardioMinutes = 5 , FatBurnMinutes = 0, PeakMinutes = 0 });
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 2, 3), CardioMinutes = 6, FatBurnMinutes = 0, PeakMinutes = 0 });
//            _fakeLocalContext.SaveChanges();

//            var heartSummaries = _controller.GetByMonth();

//            // var alcoholIntakes = response as AlcoholIntake[] ?? response.ToArray();
//            Assert.Equal(2, heartSummaries.Count());
//            Assert.Equal(2, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 1, 1)).CardioMinutes);
//            Assert.Equal(5, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 2, 1)).CardioMinutes);

//        }

//        [Fact]
//        public void ShouldGroupByWeek()
//        {
//            _fakeLocalContext.Database.EnsureDeleted();
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 11), CardioMinutes = 1  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 12), CardioMinutes = 2  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 13), CardioMinutes = 3, FatBurnMinutes = 0, PeakMinutes = 0 });

//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 18), CardioMinutes = 4  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 19), CardioMinutes = 5  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 20), CardioMinutes = 6, FatBurnMinutes = 0, PeakMinutes = 0 });

//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 25), CardioMinutes = 7  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 26), CardioMinutes = 8  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 27), CardioMinutes = 9, FatBurnMinutes = 0, PeakMinutes = 0 });
//            _fakeLocalContext.SaveChanges();
            
//            var heartSummaries = _controller.GetByWeek();

//            Assert.Equal(3, heartSummaries.Count());
//            Assert.Equal(6, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 11)).CardioMinutes);
//            Assert.Equal(15, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 18)).CardioMinutes);
//            Assert.Equal(24, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 25)).CardioMinutes);

//        }

//        [Fact]
//        public void ShouldGetAlcoholIntakes()
//        {
//            _fakeLocalContext.Database.EnsureDeleted();
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 1), CardioMinutes = 1  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 2), CardioMinutes = 2  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 3), CardioMinutes = 3  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 4), CardioMinutes = 4  , FatBurnMinutes = 0, PeakMinutes = 0});
//            _fakeLocalContext.Add(new HeartRateSummary { CreatedDate = new DateTime(2018, 6, 5), CardioMinutes = 5, FatBurnMinutes = 0, PeakMinutes = 0 });
//            _fakeLocalContext.SaveChanges();

//            var heartSummaries = _controller.Get();

//            Assert.Equal(5, heartSummaries.Count());
//            Assert.Equal(1, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 1)).CardioMinutes);
//            Assert.Equal(2, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 2)).CardioMinutes);
//            Assert.Equal(3, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 3)).CardioMinutes);
//            Assert.Equal(4, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 4)).CardioMinutes);
//            Assert.Equal(5, heartSummaries.First(x => x.CreatedDate == new DateTime(2018, 6, 5)).CardioMinutes);

//        }

//    }
//}