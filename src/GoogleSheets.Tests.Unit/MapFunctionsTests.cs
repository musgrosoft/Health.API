using System;
using System.Collections.Generic;
using GoogleSheets;
using Xunit;

namespace GoogleSheets.Tests.Unit
{
    public class MapFunctionsTests
    {
        private MapFunctions _mapFunctions;

        public MapFunctionsTests()
        {
            _mapFunctions = new MapFunctions();
        }

        [Fact]
        public void ShouldMapRowToDrink()
        {
            var row = new List<object>
            {
                "12 Jan 2001",
                "",
                "",
                "",
                "10"
            };

            var drink = _mapFunctions.MapRowToDrink(row);

            Assert.Equal(new DateTime(2001,1,12), drink.CreatedDate );
            Assert.Equal(10, drink.Units);

        }

        [Fact]
        public void ShouldMapRowToExercise()
        {
            var row = new List<object>
            {
                "10 Jan 2001",
                "1234",
                "",
                "treadmill",
                "4567"
            };

            var exercise = _mapFunctions.MapRowToExercise(row);

            Assert.Equal(new DateTime(2001, 1, 10), exercise.CreatedDate);
            Assert.Equal(1234, exercise.Metres);
            Assert.Equal(4567, exercise.TotalSeconds);
            Assert.Equal("treadmill", exercise.Description);

        }

    }
}
