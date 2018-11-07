using System;
using System.Collections.Generic;
using Google;
using Xunit;

namespace GoogleSheets.Unit.Tests
{
    public class MapperTests
    {
        private Mapper _mapper;

        public MapperTests()
        {
            _mapper = new Mapper();
        }

        [Fact]
        public void ShouldMapFromRowToRun()
        {
            IList<object> row = new List<object>
            {
                "05-Feb-2018",
                "5000",
                "00:24:36"
            };

            var run = _mapper.MapRowToRun(row);

            Assert.Equal(new DateTime(2018, 2, 5), run.CreatedDate);
            Assert.Equal(5000, run.Metres);
            Assert.Equal(new TimeSpan(0, 24, 36), run.Time);
        }

        [Fact]
        public void ShouldMapFromRowToErgo()
        {
            IList<object> row = new List<object>
            {
                "06-Feb-2018",
                "6000",
                "00:12:34"
            };

            var ergo = _mapper.MapRowToErgo(row);

            Assert.Equal(new DateTime(2018, 2, 6), ergo.CreatedDate);
            Assert.Equal(6000, ergo.Metres);
            Assert.Equal(ergo.Time, new TimeSpan(0, 12, 34));
        }

        [Fact]
        public void ShouldMapFromRowToAlcoholIntake()
        {
            IList<object> row = new List<object>
            {
                "06-Mar-2018",
                "12"
            };

            var alcoholIntake = _mapper.MapRowToAlcoholIntake(row);

            Assert.Equal(new DateTime(2018, 3, 6), alcoholIntake.CreatedDate);
            Assert.Equal(12, alcoholIntake.Units);
        }

    }
}