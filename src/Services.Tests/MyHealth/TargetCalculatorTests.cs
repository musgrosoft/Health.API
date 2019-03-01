using System;
using Services.Health;
using Xunit;

namespace Services.Tests.MyHealth
{
    public class TargetCalculatorTests
    {
        private TargetCalculator _targetCalculator;

        public TargetCalculatorTests()
        {
            _targetCalculator = new TargetCalculator();
        }




        [Theory]
        [InlineData(2017, 1, 1, 0)]
        [InlineData(2018, 5, 27, 0)]
        [InlineData(2018, 5, 28, 11)]
        [InlineData(2019, 1, 1, 11)]
        [InlineData(2019, 1, 2, 11)]
        [InlineData(2019, 1, 3, 11)]
        public void ShouldGetCardioAndAboveTarget(int year, int month, int day, double? expectedTarget)
        {
            var dateTime = new DateTime(year, month, day);

            var targetUnits = _targetCalculator.GetCardioAndAboveTarget(dateTime);

            Assert.Equal(expectedTarget, targetUnits);
        }

        [Theory]
        [InlineData(2017, 1, 1, null)]
        [InlineData(2019, 1, 1, 4)]
        [InlineData(2019, 1, 2, 4)]
        [InlineData(2019, 1, 3, 4)]
        public void ShouldGetAlcoholIntakeTarget(int year, int month, int day, double? expectedTarget)
        {
            var dateTime = new DateTime(year, month, day);

            var targetUnits = _targetCalculator.GetAlcoholIntakeTarget(dateTime);

            Assert.Equal(expectedTarget, targetUnits);
        }


        [Theory]
        [InlineData(2018, 8, 1, 89.2066666666663)]
        [InlineData(2019, 1, 1, 87.6733333333333)]
        [InlineData(2019, 1, 2, 87.665)]
        [InlineData(2019, 1, 3, 87.6566666666667)]
        public void ShouldGetTargetWeight(int year, int month, int day, double? expectedTarget)
        {
            var dateTime = new DateTime(year, month, day);

            var targetUnits = _targetCalculator.GetTargetWeight(dateTime);

            Assert.True(Math.Abs(expectedTarget.Value - targetUnits.Value) < 0.0001 );
        }

        [Theory]
        [InlineData(2017, 1, 1)]
        public void ShouldGetTargetWeightBeNullIfOutsideDateRange(int year, int month, int day)
        {
            var dateTime = new DateTime(year, month, day);

            var targetUnits = _targetCalculator.GetTargetWeight(dateTime);

            Assert.Null(targetUnits);
        }

    }
}