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
    public class AlcoholIntakesControllerTests : IDisposable
    {
        private AlcoholIntakesController _controller;
        private FakeLocalContext _fakeLocalContext;

        public AlcoholIntakesControllerTests()
        {
            _fakeLocalContext = new FakeLocalContext();
            _controller = new AlcoholIntakesController(_fakeLocalContext);
        }

        public void Dispose()
        {
            _fakeLocalContext.Database.EnsureDeleted();
        }

        [Fact]
        public void ShouldGroupByMonth()
        {
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 1, 1), Units = 1 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 1, 2), Units = 2 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 1, 3), Units = 3 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 2, 1), Units = 4 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 2, 2), Units = 5 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 2, 3), Units = 6 });
            _fakeLocalContext.SaveChanges();

            var alcoholIntakes = _controller.GetByMonth();

            // var alcoholIntakes = response as AlcoholIntake[] ?? response.ToArray();
            Assert.Equal(2, alcoholIntakes.Count());
            Assert.Equal(2, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 1, 1)).Units);
            Assert.Equal(5, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 2, 1)).Units);

        }

        [Fact]
        public void ShouldGroupByWeek()
        {

            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 11), Units = 1 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 12), Units = 2 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 13), Units = 3 });

            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 18), Units = 4 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 19), Units = 5 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 20), Units = 6 });

            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 25), Units = 7 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 26), Units = 8 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 27), Units = 9 });
            _fakeLocalContext.SaveChanges();
            
            var alcoholIntakes = _controller.GetByWeek();

            Assert.Equal(3, alcoholIntakes.Count());
            Assert.Equal(6, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 6, 11)).Units);
            Assert.Equal(15, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 6, 18)).Units);
            Assert.Equal(24, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 6, 25)).Units);

        }

        [Fact]
        public void ShouldGetAlcoholIntakes()
        {

            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 1), Units = 1 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 2), Units = 2 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 3), Units = 3 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 4), Units = 4 });
            _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 5), Units = 5 });
            _fakeLocalContext.SaveChanges();

            var alcoholIntakes = _controller.Get();

            Assert.Equal(5, alcoholIntakes.Count());
            Assert.Equal(1, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 6, 1)).Units);
            Assert.Equal(2, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 6, 2)).Units);
            Assert.Equal(3, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 6, 3)).Units);
            Assert.Equal(4, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 6, 4)).Units);
            Assert.Equal(5, alcoholIntakes.First(x => x.DateTime == new DateTime(2018, 6, 5)).Units);

        }

        //[Fact]
        //public void ShouldRunXXXXXXXXXXODataQuery()
        //{
        //    _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2016, 6, 1), Units = 1 });
        //    _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2016, 6, 2), Units = 2 });
        //    _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2017, 6, 3), Units = 3 });
        //    _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2017, 6, 4), Units = 4 });
        //    _fakeLocalContext.Add(new AlcoholIntake { DateTime = new DateTime(2018, 6, 5), Units = 5 });
        //    _fakeLocalContext.SaveChanges();

        //    _controller.ControllerContext = new ControllerContext();
        //    _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        //    _controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
        //    _controller.ControllerContext.HttpContext.Request.QueryString = new QueryString("?$filter=year(DateTime)%20eq%202016");

        //    var alcoholIntakes = _controller.Get();

        //    Assert.Equal(2, alcoholIntakes.Count());
        //    Assert.Equal(1, alcoholIntakes.First(x => x.DateTime == new DateTime(2016, 6, 1)).Units);
        //    Assert.Equal(2, alcoholIntakes.First(x => x.DateTime == new DateTime(2016, 6, 2)).Units);

        //}
    }
}