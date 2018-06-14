using System.Collections.Generic;
using Repositories.Models;
using Services.MyHealth;
using Xunit;

namespace Services.Tests.MyHealth
{
    public class AggregationCalculatorTests
    {
        private AggregationCalculator _aggregationCalculator;

        public AggregationCalculatorTests()
        {
            _aggregationCalculator = new AggregationCalculator();
        }

        [Fact]
        public void ShouldDoMovingAverages()
        {
            var seedWeights = new List<Weight>
            {
                new Weight {Kg = 10},
                new Weight {Kg = 20},
                new Weight {Kg = 30},
                new Weight {Kg = 40},
                new Weight {Kg = 50},
                new Weight {Kg = 60},
                new Weight {Kg = 70},
                new Weight {Kg = 80},
                new Weight {Kg = 90},
            };


            var orderedWeights = new List<Weight>
            {
                new Weight {Kg = 100},
                new Weight {Kg = 110},
                new Weight {Kg = 120},
                new Weight {Kg = 130},
                new Weight {Kg = 140},
                new Weight {Kg = 150},
                new Weight {Kg = 160},
                new Weight {Kg = 170},
                new Weight {Kg = 180},
                new Weight {Kg = 190},
                new Weight {Kg = 200},
                new Weight {Kg = 210},
                new Weight {Kg = 220},
                new Weight {Kg = 230},
                new Weight {Kg = 240},
            };

            _aggregationCalculator.SetMovingAveragesOnWeights(seedWeights, orderedWeights);

            Assert.Equal(15, orderedWeights.Count);

            Assert.Equal(100, orderedWeights[0].Kg);
            Assert.Equal(110, orderedWeights[1].Kg);
            Assert.Equal(120, orderedWeights[2].Kg);
            Assert.Equal(130, orderedWeights[3].Kg);
            Assert.Equal(140, orderedWeights[4].Kg);
            Assert.Equal(150, orderedWeights[5].Kg);
            Assert.Equal(160, orderedWeights[6].Kg);
            Assert.Equal(170, orderedWeights[7].Kg);
            Assert.Equal(180, orderedWeights[8].Kg);
            Assert.Equal(190, orderedWeights[9].Kg);
            Assert.Equal(200, orderedWeights[10].Kg);
            Assert.Equal(210, orderedWeights[11].Kg);
            Assert.Equal(220, orderedWeights[12].Kg);
            Assert.Equal(230, orderedWeights[13].Kg);
            Assert.Equal(240, orderedWeights[14].Kg);

            Assert.Equal(55, orderedWeights[0].MovingAverageKg);
            Assert.Equal(65, orderedWeights[1].MovingAverageKg);
            Assert.Equal(75, orderedWeights[2].MovingAverageKg);
            Assert.Equal(85, orderedWeights[3].MovingAverageKg);
            Assert.Equal(95, orderedWeights[4].MovingAverageKg);
            Assert.Equal(105, orderedWeights[5].MovingAverageKg);
            Assert.Equal(115, orderedWeights[6].MovingAverageKg);
            Assert.Equal(125, orderedWeights[7].MovingAverageKg);
            Assert.Equal(135, orderedWeights[8].MovingAverageKg);
            Assert.Equal(145, orderedWeights[9].MovingAverageKg);
            Assert.Equal(155, orderedWeights[10].MovingAverageKg);
            Assert.Equal(165, orderedWeights[11].MovingAverageKg);
            Assert.Equal(175, orderedWeights[12].MovingAverageKg);
            Assert.Equal(185, orderedWeights[13].MovingAverageKg);
            Assert.Equal(195, orderedWeights[14].MovingAverageKg);

        }


        [Fact]
        public void ShouldDoMovingAverages2()
        {
            var seedWeights = new List<Weight>
            {
                new Weight {Kg = 40},
                new Weight {Kg = 50},
                new Weight {Kg = 60},
                new Weight {Kg = 70},
                new Weight {Kg = 80},
                new Weight {Kg = 90},

            };

            var orderedWeights = new List<Weight>
            {
                new Weight {Kg = 100},
                new Weight {Kg = 110},
                new Weight {Kg = 120},
                new Weight {Kg = 130},
                new Weight {Kg = 140},
                new Weight {Kg = 150},
                new Weight {Kg = 160},
                new Weight {Kg = 170},
                new Weight {Kg = 180},
                new Weight {Kg = 190},
                new Weight {Kg = 200},
                new Weight {Kg = 210},
                new Weight {Kg = 220},
                new Weight {Kg = 230},
                new Weight {Kg = 240},
            };

            _aggregationCalculator.SetMovingAveragesOnWeights(seedWeights, orderedWeights);

            Assert.Equal(15, orderedWeights.Count);

            Assert.Equal(100, orderedWeights[0].Kg);
            Assert.Equal(110, orderedWeights[1].Kg);
            Assert.Equal(120, orderedWeights[2].Kg);
            Assert.Equal(130, orderedWeights[3].Kg);
            Assert.Equal(140, orderedWeights[4].Kg);
            Assert.Equal(150, orderedWeights[5].Kg);
            Assert.Equal(160, orderedWeights[6].Kg);
            Assert.Equal(170, orderedWeights[7].Kg);
            Assert.Equal(180, orderedWeights[8].Kg);
            Assert.Equal(190, orderedWeights[9].Kg);
            Assert.Equal(200, orderedWeights[10].Kg);
            Assert.Equal(210, orderedWeights[11].Kg);
            Assert.Equal(220, orderedWeights[12].Kg);
            Assert.Equal(230, orderedWeights[13].Kg);
            Assert.Equal(240, orderedWeights[14].Kg);

            Assert.Null(orderedWeights[0].MovingAverageKg);
            Assert.Null(orderedWeights[1].MovingAverageKg);
            Assert.Null(orderedWeights[2].MovingAverageKg);
            Assert.Equal(85, orderedWeights[3].MovingAverageKg);
            Assert.Equal(95, orderedWeights[4].MovingAverageKg);
            Assert.Equal(105, orderedWeights[5].MovingAverageKg);
            Assert.Equal(115, orderedWeights[6].MovingAverageKg);
            Assert.Equal(125, orderedWeights[7].MovingAverageKg);
            Assert.Equal(135, orderedWeights[8].MovingAverageKg);
            Assert.Equal(145, orderedWeights[9].MovingAverageKg);
            Assert.Equal(155, orderedWeights[10].MovingAverageKg);
            Assert.Equal(165, orderedWeights[11].MovingAverageKg);
            Assert.Equal(175, orderedWeights[12].MovingAverageKg);
            Assert.Equal(185, orderedWeights[13].MovingAverageKg);
            Assert.Equal(195, orderedWeights[14].MovingAverageKg);

        }
    }
}