using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Repositories.Models;
using Services.Health;
using Utils;
using Xunit;

namespace Services.Tests.MyHealth
{
    public class TargetServiceTests
    {
        private Mock<ITargetCalculator> _targetCalculator;
        private Mock<ICalendar> _calendar;
        private TargetService _targetService;

        public TargetServiceTests()
        {
            _targetCalculator = new Mock<ITargetCalculator>();
            _calendar = new Mock<ICalendar>();
            _targetService = new TargetService(_targetCalculator.Object, _calendar.Object);
        }

        [Fact]
        public void ShouldSetTargetsOnHeartRateSummaries()
        {
            _targetCalculator.Setup(x => x.GetTargetCumSumCardioAndAbove(new DateTime(2018, 1, 1))).Returns(1);
            _targetCalculator.Setup(x => x.GetTargetCumSumCardioAndAbove(new DateTime(2018, 1, 2))).Returns(2);
            _targetCalculator.Setup(x => x.GetTargetCumSumCardioAndAbove(new DateTime(2018, 1, 3))).Returns(3);

            

            var heartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018, 1, 1)},
                new HeartRateSummary {CreatedDate = new DateTime(2018, 1, 2)},
                new HeartRateSummary {CreatedDate = new DateTime(2018, 1, 3)},
            };
            
            //When
            var updatedHeartRateSummaries = _targetService.SetTargets(heartRateSummaries);

            //Then
            Assert.Equal(1, updatedHeartRateSummaries[0].TargetCumSumCardioAndAbove);
            Assert.Equal(2, updatedHeartRateSummaries[1].TargetCumSumCardioAndAbove);
            Assert.Equal(3, updatedHeartRateSummaries[2].TargetCumSumCardioAndAbove);

        }

        [Fact]
        public void ShouldSetTargetsOnStepCounts()
        {
            _targetCalculator.Setup(x => x.GetTargetStepCountCumSum(new DateTime(2018, 1, 1))).Returns(1);
            _targetCalculator.Setup(x => x.GetTargetStepCountCumSum(new DateTime(2018, 1, 2))).Returns(2);
            _targetCalculator.Setup(x => x.GetTargetStepCountCumSum(new DateTime(2018, 1, 3))).Returns(3);

            var stepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2018, 1, 1)},
                new StepCount {CreatedDate = new DateTime(2018, 1, 2)},
                new StepCount {CreatedDate = new DateTime(2018, 1, 3)},
            };
            
            //When
            var updatedStepCounts = _targetService.SetTargets(stepCounts);

            //Then
            Assert.Equal(1, updatedStepCounts[0].TargetCumSumCount);
            Assert.Equal(2, updatedStepCounts[1].TargetCumSumCount);
            Assert.Equal(3, updatedStepCounts[2].TargetCumSumCount);
        }
        
        [Fact]
        public void ShouldSetTargetsOnActivitySummaries()
        {
            _targetCalculator.Setup(x => x.GetTargetActivitySummaryCumSum(new DateTime(2018, 1, 1))).Returns(1);
            _targetCalculator.Setup(x => x.GetTargetActivitySummaryCumSum(new DateTime(2018, 1, 2))).Returns(2);
            _targetCalculator.Setup(x => x.GetTargetActivitySummaryCumSum(new DateTime(2018, 1, 3))).Returns(3);

            var activitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2018, 1, 1)},
                new ActivitySummary {CreatedDate = new DateTime(2018, 1, 2)},
                new ActivitySummary {CreatedDate = new DateTime(2018, 1, 3)},
            };

            //When
            var updatedStepCounts = _targetService.SetTargets(activitySummaries);

            //Then
            Assert.Equal(1, updatedStepCounts[0].TargetCumSumActiveMinutes);
            Assert.Equal(2, updatedStepCounts[1].TargetCumSumActiveMinutes);
            Assert.Equal(3, updatedStepCounts[2].TargetCumSumActiveMinutes);
        }

        [Fact]
        public void ShouldSetTargetsOnAlcoholIntakes()
        {
            _targetCalculator.Setup(x => x.GetAlcoholIntakeTarget(new DateTime(2018, 1, 1))).Returns(1);
            _targetCalculator.Setup(x => x.GetAlcoholIntakeTarget(new DateTime(2018, 1, 2))).Returns(2);
            _targetCalculator.Setup(x => x.GetAlcoholIntakeTarget(new DateTime(2018, 1, 3))).Returns(3);

            var alcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake {CreatedDate = new DateTime(2018, 1, 1)},
                new AlcoholIntake {CreatedDate = new DateTime(2018, 1, 2)},
                new AlcoholIntake {CreatedDate = new DateTime(2018, 1, 3)},
            };
            
            //When
            var updatedAlcoholIntakes = _targetService.SetTargets(alcoholIntakes);

            //Then
            Assert.Equal(1, updatedAlcoholIntakes[0].TargetCumSumUnits);
            Assert.Equal(2, updatedAlcoholIntakes[1].TargetCumSumUnits);
            Assert.Equal(3, updatedAlcoholIntakes[2].TargetCumSumUnits);

        }

        [Fact]
        public void ShouldSetTargetsOnWeights()
        {
            _targetCalculator.Setup(x => x.GetTargetWeight(new DateTime(2018, 1, 1))).Returns(1);
            _targetCalculator.Setup(x => x.GetTargetWeight(new DateTime(2018, 1, 2))).Returns(2);
            _targetCalculator.Setup(x => x.GetTargetWeight(new DateTime(2018, 1, 3))).Returns(3);

            _calendar.Setup(x => x.Now()).Returns(new DateTime(2020, 1, 1));

            var weights = new List<Weight>
            {
                new Weight {CreatedDate = new DateTime(2018, 1, 1)},
                new Weight {CreatedDate = new DateTime(2018, 1, 2)},
                new Weight {CreatedDate = new DateTime(2018, 1, 3)},
            };
            
            //When
            var updatedWeights = _targetService.SetTargets(weights, 123);

            //Then
            Assert.Equal(1, updatedWeights.First(x => x.CreatedDate == new DateTime(2018, 1, 1)).TargetKg);
            Assert.Equal(2, updatedWeights.First(x => x.CreatedDate == new DateTime(2018, 1, 2)).TargetKg);
            Assert.Equal(3, updatedWeights.First(x => x.CreatedDate == new DateTime(2018, 1, 3)).TargetKg);

        }

        [Fact]
        public void ShouldSetFutureTargetsOnWeights()
        {
            _targetCalculator.Setup(x => x.GetTargetWeight(new DateTime(2020, 1, 1))).Returns(1);
            _targetCalculator.Setup(x => x.GetTargetWeight(new DateTime(2020, 1, 2))).Returns(2);
            _targetCalculator.Setup(x => x.GetTargetWeight(new DateTime(2020, 1, 3))).Returns(3);
            _targetCalculator.Setup(x => x.GetTargetWeight(new DateTime(2020, 1, 4))).Returns(4);
            _targetCalculator.Setup(x => x.GetTargetWeight(new DateTime(2020, 1, 5))).Returns(5);
            _targetCalculator.Setup(x => x.GetTargetWeight(new DateTime(2020, 1, 6))).Returns(6);

            _calendar.Setup(x => x.Now()).Returns(new DateTime(2020, 1, 3));

            var weights = new List<Weight>
            {
                new Weight {CreatedDate = new DateTime(2020, 1, 1)},
            };

            //When
            var updatedWeights = _targetService.SetTargets(weights, 3);

            //Then
            Assert.Equal(6, updatedWeights.Count);
            Assert.Equal(1, updatedWeights.First(x => x.CreatedDate == new DateTime(2020, 1, 1)).TargetKg);
            Assert.Equal(2, updatedWeights.First(x => x.CreatedDate == new DateTime(2020, 1, 2)).TargetKg);
            Assert.Equal(3, updatedWeights.First(x => x.CreatedDate == new DateTime(2020, 1, 3)).TargetKg);
            Assert.Equal(4, updatedWeights.First(x => x.CreatedDate == new DateTime(2020, 1, 4)).TargetKg);
            Assert.Equal(5, updatedWeights.First(x => x.CreatedDate == new DateTime(2020, 1, 5)).TargetKg);
            Assert.Equal(6, updatedWeights.First(x => x.CreatedDate == new DateTime(2020, 1, 6)).TargetKg);
        }

    }
}