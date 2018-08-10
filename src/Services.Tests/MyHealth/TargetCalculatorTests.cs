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
        [InlineData(2017, 1, 1, null)]
        [InlineData(2018, 1, 1, 2440000)]
        [InlineData(2018, 1, 2, 2450000)]
        [InlineData(2018, 1, 3, 2460000)]
        public void ShoulCalculateTargetStepCounts(int year, int month, int day, double? expectedTargetStepCounts)
        {
            var dateTime = new DateTime(year, month, day);

            var targetStepCounts = _targetCalculator.GetTargetStepCountCumSum(dateTime);

            Assert.Equal(expectedTargetStepCounts,targetStepCounts);
        }

        [Theory]
        [InlineData(2017, 1, 1, null)]
        [InlineData(2019, 1, 1, 6016)]
        [InlineData(2019, 1, 2, 6020)]
        [InlineData(2019, 1, 3, 6024)]
        public void ShoulCalculateTargetAlcoholIntakes(int year, int month, int day, double? expectedTargetUnits)
        {
            var dateTime = new DateTime(year, month, day);

            var targetUnits = _targetCalculator.GetAlcoholIntakeTarget(dateTime);

            Assert.Equal(expectedTargetUnits, targetUnits);
        }


    }
}