using System;
using Xunit;

namespace Utils.Unit.Tests
{
    public class ExtensionMethodTests
    {
        [Theory]
        [InlineData(0, 1970, 1, 1, 0, 0, 0)]
        [InlineData(1, 1970, 1, 1, 0, 0, 1)]
        [InlineData(1234546789, 2009, 2, 13, 17, 39, 49)]
        public void ShouldConvertToDateFromUnixTimeLong(long input, int year, int month, int day, int hour, int minute, int second)
        {
            var expectedDate = new DateTime(year, month, day, hour, minute, second);
            var date = input.ToDateFromUnixTime();

            Assert.Equal(expectedDate, date);
        }

        [Theory]
        [InlineData(0, 1970, 1, 1, 0, 0, 0)]
        [InlineData(1, 1970, 1, 1, 0, 0, 1)]
        [InlineData(1234546789, 2009, 2, 13, 17, 39, 49)]
        public void ShouldConvertToDateFromUnixTimeInt(int input, int year, int month, int day, int hour, int minute, int second)
        {
            var expectedDate = new DateTime(year, month, day, hour, minute, second);
            var date = input.ToDateFromUnixTime();

            Assert.Equal(expectedDate, date);
        }
        

        [Theory]
        [InlineData(2020, 1, 20, 2020, 1, 25, 2020, 1, 22, true)]
        [InlineData(2020, 1, 20, 2020, 1, 25, 2020, 1, 26, false)]
        [InlineData(2020, 1, 20, 2020, 1, 25, 2020, 1, 19, false)]
        [InlineData(2020, 1, 20, 2020, 1, 25, 2020, 1, 25, true)]
        [InlineData(2020, 1, 20, 2020, 1, 25, 2020, 1, 20, true)]
        public void ShouldTellIfDateIsBetweenOthers(int yearA, int monthA, int dayA, int yearB, int monthB, int dayB, int testYear, int testMonth, int testDay, bool expected)
        {
            var dateA = new DateTime(yearA, monthA, dayA);
            var dateB = new DateTime(yearB, monthB, dayB);
            var testDate = new DateTime(testYear, testMonth, testDay);

            var result = testDate.Between(dateA, dateB);

            Assert.Equal(expected, result);


        }

    }
}