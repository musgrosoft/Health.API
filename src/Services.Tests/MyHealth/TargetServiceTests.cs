using System;
using System.Collections.Generic;
using Moq;
using Repositories.Models;
using Services.Health;
using Xunit;

namespace Services.Tests.MyHealth
{
    public class TargetServiceTests
    {
        [Fact]
        public void ShouldSetTargetsOnHeartRateSummaries()
        {
            var targetCalculator = new Mock<ITargetCalculator>();

            targetCalculator.Setup(x => x.GetTargetCumSumCardioAndAbove(new DateTime(2018, 1, 1))).Returns(1);
            targetCalculator.Setup(x => x.GetTargetCumSumCardioAndAbove(new DateTime(2018, 1, 2))).Returns(2);
            targetCalculator.Setup(x => x.GetTargetCumSumCardioAndAbove(new DateTime(2018, 1, 3))).Returns(3);

            var heartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018, 1, 1)},
                new HeartRateSummary {CreatedDate = new DateTime(2018, 1, 2)},
                new HeartRateSummary {CreatedDate = new DateTime(2018, 1, 3)},
            };
            
            var targetService = new TargetService(targetCalculator.Object);

            //When
            var updatedHeartRateSummaries = targetService.SetTargets(heartRateSummaries);

            //Then
            Assert.Equal(1, updatedHeartRateSummaries[0].TargetCumSumCardioAndAbove);
            Assert.Equal(2, updatedHeartRateSummaries[1].TargetCumSumCardioAndAbove);
            Assert.Equal(3, updatedHeartRateSummaries[2].TargetCumSumCardioAndAbove);

        }
    }
}