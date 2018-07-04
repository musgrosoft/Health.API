//using System;
//using System.Linq;
//using HealthAPI.Controllers.OData;
//using Repositories.Models;
//using Xunit;

//namespace HealthAPI.Unit.Tests.Controllers.OData
//{
//    public class BloodPressuresControllerTests
//    {
//        private BloodPressuresController _controller;
//        private FakeLocalContext _fakeLocalContext;

//        public BloodPressuresControllerTests()
//        {
//            _fakeLocalContext = new FakeLocalContext();
//            _controller = new BloodPressuresController(_fakeLocalContext);
//        }

//        public void Dispose()
//        {
//            _fakeLocalContext.Database.EnsureDeleted();
//        }

//        [Fact]
//        public void ShouldGetBloodPressures()
//        {

//            _fakeLocalContext.Add(new BloodPressure { CreatedDate = new DateTime(2018, 6, 1), Systolic = 1 });
//            _fakeLocalContext.Add(new BloodPressure { CreatedDate = new DateTime(2018, 6, 2), Systolic = 2 });
//            _fakeLocalContext.Add(new BloodPressure { CreatedDate = new DateTime(2018, 6, 3), Systolic = 3 });
//            _fakeLocalContext.Add(new BloodPressure { CreatedDate = new DateTime(2018, 6, 4), Systolic = 4 });
//            _fakeLocalContext.Add(new BloodPressure { CreatedDate = new DateTime(2018, 6, 5), Systolic = 5 });
//            _fakeLocalContext.SaveChanges();

//            var bloodPressures = _controller.Get();

//            Assert.Equal(5, bloodPressures.Count());
//            Assert.Equal(1, bloodPressures.First(x => x.CreatedDate == new DateTime(2018, 6, 1)).Systolic);
//            Assert.Equal(2, bloodPressures.First(x => x.CreatedDate == new DateTime(2018, 6, 2)).Systolic);
//            Assert.Equal(3, bloodPressures.First(x => x.CreatedDate == new DateTime(2018, 6, 3)).Systolic);
//            Assert.Equal(4, bloodPressures.First(x => x.CreatedDate == new DateTime(2018, 6, 4)).Systolic);
//            Assert.Equal(5, bloodPressures.First(x => x.CreatedDate == new DateTime(2018, 6, 5)).Systolic);
//        }
//    }
//}