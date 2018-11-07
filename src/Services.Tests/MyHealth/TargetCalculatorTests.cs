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
        [InlineData(2018, 1, 1, 10000)]
        [InlineData(2018, 1, 2, 10000)]
        [InlineData(2018, 1, 3, 10000)]
        public void ShoulCalculateTargetStepCounts(int year, int month, int day, double? expectedTargetStepCounts)
        {
            var dateTime = new DateTime(year, month, day);

            var targetStepCounts = _targetCalculator.GetTargetStepCount(dateTime);

            Assert.Equal(expectedTargetStepCounts, targetStepCounts);
        }

        [Theory]
        [InlineData(2017, 1, 1, 30)]
        [InlineData(2019, 1, 1, 30)]
        [InlineData(2019, 1, 2, 30)]
        [InlineData(2019, 1, 3, 30)]
        public void ShouldGetActivitySummaryTarget(int year, int month, int day, double? expectedTarget)
        {
            var dateTime = new DateTime(year, month, day);

            var targetUnits = _targetCalculator.GetActivitySummaryTarget(dateTime);

            Assert.Equal(expectedTarget, targetUnits);
        }


        [Theory]
        [InlineData(2017, 1, 1, 11)]
        [InlineData(2019, 1, 1, 11)]
        [InlineData(2019, 1, 2, 11)]
        [InlineData(2019, 1, 3, 11)]
        public void ShouldGetCardioAndAboveTarget(int year, int month, int day, double? expectedTarget)
        {
            var dateTime = new DateTime(year, month, day);

            var targetUnits = _targetCalculator.GetCardioAndAboveTarget(dateTime);

            Assert.Equal(expectedTarget, targetUnits);
        }

    }
}