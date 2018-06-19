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
        public void ShouldSetMovingAveragesOnWeightsWithFullSeedList()
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

            _aggregationCalculator.SetMovingAveragesOnWeights(seedWeights, orderedWeights,10);

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
        public void ShouldSetMovingAveragesOnWeightsWithPartialSeedList()
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

            _aggregationCalculator.SetMovingAveragesOnWeights(seedWeights, orderedWeights, 10);

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

        [Fact]
        public void ShouldSetMovingAveragesOnWeightsWithEmptySeedList()
        {
            var seedWeights = new List<Weight>
            {

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

            _aggregationCalculator.SetMovingAveragesOnWeights(seedWeights, orderedWeights, 10);

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
            Assert.Null(orderedWeights[3].MovingAverageKg);
            Assert.Null(orderedWeights[4].MovingAverageKg);
            Assert.Null(orderedWeights[5].MovingAverageKg);
            Assert.Null(orderedWeights[6].MovingAverageKg);
            Assert.Null(orderedWeights[7].MovingAverageKg);
            Assert.Null(orderedWeights[8].MovingAverageKg);
            Assert.Equal(145, orderedWeights[9].MovingAverageKg);
            Assert.Equal(155, orderedWeights[10].MovingAverageKg);
            Assert.Equal(165, orderedWeights[11].MovingAverageKg);
            Assert.Equal(175, orderedWeights[12].MovingAverageKg);
            Assert.Equal(185, orderedWeights[13].MovingAverageKg);
            Assert.Equal(195, orderedWeights[14].MovingAverageKg);

        }

        [Fact]
        public void ShouldSetMovingAveragesOnRestingHeartRatesWithFullSeedList()
        {
            var seedRestingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate {Beats = 10},
                new RestingHeartRate {Beats = 20},
                new RestingHeartRate {Beats = 30},
                new RestingHeartRate {Beats = 40},
                new RestingHeartRate {Beats = 50},
                new RestingHeartRate {Beats = 60},
                new RestingHeartRate {Beats = 70},
                new RestingHeartRate {Beats = 80},
                new RestingHeartRate {Beats = 90},
            };


            var orderedRestingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate {Beats = 100},
                new RestingHeartRate {Beats = 110},
                new RestingHeartRate {Beats = 120},
                new RestingHeartRate {Beats = 130},
                new RestingHeartRate {Beats = 140},
                new RestingHeartRate {Beats = 150},
                new RestingHeartRate {Beats = 160},
                new RestingHeartRate {Beats = 170},
                new RestingHeartRate {Beats = 180},
                new RestingHeartRate {Beats = 190},
                new RestingHeartRate {Beats = 200},
                new RestingHeartRate {Beats = 210},
                new RestingHeartRate {Beats = 220},
                new RestingHeartRate {Beats = 230},
                new RestingHeartRate {Beats = 240},
            };

            _aggregationCalculator.SetMovingAveragesOnRestingHeartRates(seedRestingHeartRates, orderedRestingHeartRates, 10);

            Assert.Equal(15, orderedRestingHeartRates.Count);

            Assert.Equal(100, orderedRestingHeartRates[0].Beats);
            Assert.Equal(110, orderedRestingHeartRates[1].Beats);
            Assert.Equal(120, orderedRestingHeartRates[2].Beats);
            Assert.Equal(130, orderedRestingHeartRates[3].Beats);
            Assert.Equal(140, orderedRestingHeartRates[4].Beats);
            Assert.Equal(150, orderedRestingHeartRates[5].Beats);
            Assert.Equal(160, orderedRestingHeartRates[6].Beats);
            Assert.Equal(170, orderedRestingHeartRates[7].Beats);
            Assert.Equal(180, orderedRestingHeartRates[8].Beats);
            Assert.Equal(190, orderedRestingHeartRates[9].Beats);
            Assert.Equal(200, orderedRestingHeartRates[10].Beats);
            Assert.Equal(210, orderedRestingHeartRates[11].Beats);
            Assert.Equal(220, orderedRestingHeartRates[12].Beats);
            Assert.Equal(230, orderedRestingHeartRates[13].Beats);
            Assert.Equal(240, orderedRestingHeartRates[14].Beats);

            Assert.Equal(55, orderedRestingHeartRates[0].MovingAverageBeats);
            Assert.Equal(65, orderedRestingHeartRates[1].MovingAverageBeats);
            Assert.Equal(75, orderedRestingHeartRates[2].MovingAverageBeats);
            Assert.Equal(85, orderedRestingHeartRates[3].MovingAverageBeats);
            Assert.Equal(95, orderedRestingHeartRates[4].MovingAverageBeats);
            Assert.Equal(105, orderedRestingHeartRates[5].MovingAverageBeats);
            Assert.Equal(115, orderedRestingHeartRates[6].MovingAverageBeats);
            Assert.Equal(125, orderedRestingHeartRates[7].MovingAverageBeats);
            Assert.Equal(135, orderedRestingHeartRates[8].MovingAverageBeats);
            Assert.Equal(145, orderedRestingHeartRates[9].MovingAverageBeats);
            Assert.Equal(155, orderedRestingHeartRates[10].MovingAverageBeats);
            Assert.Equal(165, orderedRestingHeartRates[11].MovingAverageBeats);
            Assert.Equal(175, orderedRestingHeartRates[12].MovingAverageBeats);
            Assert.Equal(185, orderedRestingHeartRates[13].MovingAverageBeats);
            Assert.Equal(195, orderedRestingHeartRates[14].MovingAverageBeats);

        }


        [Fact]
        public void ShouldSetMovingAveragesOnRestingHeartRatesWithPartialSeedList()
        {
            var seedRestingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate {Beats = 40},
                new RestingHeartRate {Beats = 50},
                new RestingHeartRate {Beats = 60},
                new RestingHeartRate {Beats = 70},
                new RestingHeartRate {Beats = 80},
                new RestingHeartRate {Beats = 90},

            };

            var orderedRestingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate {Beats = 100},
                new RestingHeartRate {Beats = 110},
                new RestingHeartRate {Beats = 120},
                new RestingHeartRate {Beats = 130},
                new RestingHeartRate {Beats = 140},
                new RestingHeartRate {Beats = 150},
                new RestingHeartRate {Beats = 160},
                new RestingHeartRate {Beats = 170},
                new RestingHeartRate {Beats = 180},
                new RestingHeartRate {Beats = 190},
                new RestingHeartRate {Beats = 200},
                new RestingHeartRate {Beats = 210},
                new RestingHeartRate {Beats = 220},
                new RestingHeartRate {Beats = 230},
                new RestingHeartRate {Beats = 240},
            };

            _aggregationCalculator.SetMovingAveragesOnRestingHeartRates(seedRestingHeartRates, orderedRestingHeartRates, 10);

            Assert.Equal(15, orderedRestingHeartRates.Count);

            Assert.Equal(100, orderedRestingHeartRates[0].Beats);
            Assert.Equal(110, orderedRestingHeartRates[1].Beats);
            Assert.Equal(120, orderedRestingHeartRates[2].Beats);
            Assert.Equal(130, orderedRestingHeartRates[3].Beats);
            Assert.Equal(140, orderedRestingHeartRates[4].Beats);
            Assert.Equal(150, orderedRestingHeartRates[5].Beats);
            Assert.Equal(160, orderedRestingHeartRates[6].Beats);
            Assert.Equal(170, orderedRestingHeartRates[7].Beats);
            Assert.Equal(180, orderedRestingHeartRates[8].Beats);
            Assert.Equal(190, orderedRestingHeartRates[9].Beats);
            Assert.Equal(200, orderedRestingHeartRates[10].Beats);
            Assert.Equal(210, orderedRestingHeartRates[11].Beats);
            Assert.Equal(220, orderedRestingHeartRates[12].Beats);
            Assert.Equal(230, orderedRestingHeartRates[13].Beats);
            Assert.Equal(240, orderedRestingHeartRates[14].Beats);

            Assert.Null(orderedRestingHeartRates[0].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[1].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[2].MovingAverageBeats);
            Assert.Equal(85, orderedRestingHeartRates[3].MovingAverageBeats);
            Assert.Equal(95, orderedRestingHeartRates[4].MovingAverageBeats);
            Assert.Equal(105, orderedRestingHeartRates[5].MovingAverageBeats);
            Assert.Equal(115, orderedRestingHeartRates[6].MovingAverageBeats);
            Assert.Equal(125, orderedRestingHeartRates[7].MovingAverageBeats);
            Assert.Equal(135, orderedRestingHeartRates[8].MovingAverageBeats);
            Assert.Equal(145, orderedRestingHeartRates[9].MovingAverageBeats);
            Assert.Equal(155, orderedRestingHeartRates[10].MovingAverageBeats);
            Assert.Equal(165, orderedRestingHeartRates[11].MovingAverageBeats);
            Assert.Equal(175, orderedRestingHeartRates[12].MovingAverageBeats);
            Assert.Equal(185, orderedRestingHeartRates[13].MovingAverageBeats);
            Assert.Equal(195, orderedRestingHeartRates[14].MovingAverageBeats);

        }

        [Fact]
        public void ShouldSetMovingAveragesOnRestingHeartRatesWithEmptySeedList()
        {
            var seedRestingHeartRates = new List<RestingHeartRate>
            {

            };

            var orderedRestingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate {Beats = 100},
                new RestingHeartRate {Beats = 110},
                new RestingHeartRate {Beats = 120},
                new RestingHeartRate {Beats = 130},
                new RestingHeartRate {Beats = 140},
                new RestingHeartRate {Beats = 150},
                new RestingHeartRate {Beats = 160},
                new RestingHeartRate {Beats = 170},
                new RestingHeartRate {Beats = 180},
                new RestingHeartRate {Beats = 190},
                new RestingHeartRate {Beats = 200},
                new RestingHeartRate {Beats = 210},
                new RestingHeartRate {Beats = 220},
                new RestingHeartRate {Beats = 230},
                new RestingHeartRate {Beats = 240},
            };

            _aggregationCalculator.SetMovingAveragesOnRestingHeartRates(seedRestingHeartRates, orderedRestingHeartRates, 10);

            Assert.Equal(15, orderedRestingHeartRates.Count);

            Assert.Equal(100, orderedRestingHeartRates[0].Beats);
            Assert.Equal(110, orderedRestingHeartRates[1].Beats);
            Assert.Equal(120, orderedRestingHeartRates[2].Beats);
            Assert.Equal(130, orderedRestingHeartRates[3].Beats);
            Assert.Equal(140, orderedRestingHeartRates[4].Beats);
            Assert.Equal(150, orderedRestingHeartRates[5].Beats);
            Assert.Equal(160, orderedRestingHeartRates[6].Beats);
            Assert.Equal(170, orderedRestingHeartRates[7].Beats);
            Assert.Equal(180, orderedRestingHeartRates[8].Beats);
            Assert.Equal(190, orderedRestingHeartRates[9].Beats);
            Assert.Equal(200, orderedRestingHeartRates[10].Beats);
            Assert.Equal(210, orderedRestingHeartRates[11].Beats);
            Assert.Equal(220, orderedRestingHeartRates[12].Beats);
            Assert.Equal(230, orderedRestingHeartRates[13].Beats);
            Assert.Equal(240, orderedRestingHeartRates[14].Beats);

            Assert.Null(orderedRestingHeartRates[0].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[1].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[2].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[3].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[4].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[5].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[6].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[7].MovingAverageBeats);
            Assert.Null(orderedRestingHeartRates[8].MovingAverageBeats);
            Assert.Equal(145, orderedRestingHeartRates[9].MovingAverageBeats);
            Assert.Equal(155, orderedRestingHeartRates[10].MovingAverageBeats);
            Assert.Equal(165, orderedRestingHeartRates[11].MovingAverageBeats);
            Assert.Equal(175, orderedRestingHeartRates[12].MovingAverageBeats);
            Assert.Equal(185, orderedRestingHeartRates[13].MovingAverageBeats);
            Assert.Equal(195, orderedRestingHeartRates[14].MovingAverageBeats);

        }










        [Fact]
        public void ShouldSetMovingAveragesOnBloodPressureWithFullSeedList()
        {
            var seedBloodPressures = new List<BloodPressure>
            {
                new BloodPressure {Systolic = 10, Diastolic = 15},
                new BloodPressure {Systolic = 20, Diastolic = 25},
                new BloodPressure {Systolic = 30, Diastolic = 35},
                new BloodPressure {Systolic = 40, Diastolic = 45},
                new BloodPressure {Systolic = 50, Diastolic = 55},
                new BloodPressure {Systolic = 60, Diastolic = 65},
                new BloodPressure {Systolic = 70, Diastolic = 75},
                new BloodPressure {Systolic = 80, Diastolic = 85},
                new BloodPressure {Systolic = 90, Diastolic = 95},
            };


            var orderedBloodPressures = new List<BloodPressure>
            {
                new BloodPressure {Systolic = 100, Diastolic = 105},
                new BloodPressure {Systolic = 110, Diastolic = 115},
                new BloodPressure {Systolic = 120, Diastolic = 125},
                new BloodPressure {Systolic = 130, Diastolic = 135},
                new BloodPressure {Systolic = 140, Diastolic = 145},
                new BloodPressure {Systolic = 150, Diastolic = 155},
                new BloodPressure {Systolic = 160, Diastolic = 165},
                new BloodPressure {Systolic = 170, Diastolic = 175},
                new BloodPressure {Systolic = 180, Diastolic = 185},
                new BloodPressure {Systolic = 190, Diastolic = 195},
                new BloodPressure {Systolic = 200, Diastolic = 205},
                new BloodPressure {Systolic = 210, Diastolic = 215},
                new BloodPressure {Systolic = 220, Diastolic = 225},
                new BloodPressure {Systolic = 230, Diastolic = 235},
                new BloodPressure {Systolic = 240, Diastolic = 245},
            };

            _aggregationCalculator.SetMovingAveragesOnBloodPressures(seedBloodPressures, orderedBloodPressures, 10);

            Assert.Equal(15, orderedBloodPressures.Count);

            Assert.Equal(100,  orderedBloodPressures[0].Systolic);
            Assert.Equal(110,  orderedBloodPressures[1].Systolic);
            Assert.Equal(120,  orderedBloodPressures[2].Systolic);
            Assert.Equal(130,  orderedBloodPressures[3].Systolic);
            Assert.Equal(140,  orderedBloodPressures[4].Systolic);
            Assert.Equal(150,  orderedBloodPressures[5].Systolic);
            Assert.Equal(160,  orderedBloodPressures[6].Systolic);
            Assert.Equal(170,  orderedBloodPressures[7].Systolic);
            Assert.Equal(180,  orderedBloodPressures[8].Systolic);
            Assert.Equal(190,  orderedBloodPressures[9].Systolic);
            Assert.Equal(200, orderedBloodPressures[10].Systolic);
            Assert.Equal(210, orderedBloodPressures[11].Systolic);
            Assert.Equal(220, orderedBloodPressures[12].Systolic);
            Assert.Equal(230, orderedBloodPressures[13].Systolic);
            Assert.Equal(240, orderedBloodPressures[14].Systolic);

            Assert.Equal(105,  orderedBloodPressures[0].Diastolic);
            Assert.Equal(115,  orderedBloodPressures[1].Diastolic);
            Assert.Equal(125,  orderedBloodPressures[2].Diastolic);
            Assert.Equal(135,  orderedBloodPressures[3].Diastolic);
            Assert.Equal(145,  orderedBloodPressures[4].Diastolic);
            Assert.Equal(155,  orderedBloodPressures[5].Diastolic);
            Assert.Equal(165,  orderedBloodPressures[6].Diastolic);
            Assert.Equal(175,  orderedBloodPressures[7].Diastolic);
            Assert.Equal(185,  orderedBloodPressures[8].Diastolic);
            Assert.Equal(195,  orderedBloodPressures[9].Diastolic);
            Assert.Equal(205, orderedBloodPressures[10].Diastolic);
            Assert.Equal(215, orderedBloodPressures[11].Diastolic);
            Assert.Equal(225, orderedBloodPressures[12].Diastolic);
            Assert.Equal(235, orderedBloodPressures[13].Diastolic);
            Assert.Equal(245, orderedBloodPressures[14].Diastolic);


            Assert.Equal(55,   orderedBloodPressures[0].MovingAverageSystolic);
            Assert.Equal(65,   orderedBloodPressures[1].MovingAverageSystolic);
            Assert.Equal(75,   orderedBloodPressures[2].MovingAverageSystolic);
            Assert.Equal(85,   orderedBloodPressures[3].MovingAverageSystolic);
            Assert.Equal(95,   orderedBloodPressures[4].MovingAverageSystolic);
            Assert.Equal(105,  orderedBloodPressures[5].MovingAverageSystolic);
            Assert.Equal(115,  orderedBloodPressures[6].MovingAverageSystolic);
            Assert.Equal(125,  orderedBloodPressures[7].MovingAverageSystolic);
            Assert.Equal(135,  orderedBloodPressures[8].MovingAverageSystolic);
            Assert.Equal(145,  orderedBloodPressures[9].MovingAverageSystolic);
            Assert.Equal(155, orderedBloodPressures[10].MovingAverageSystolic);
            Assert.Equal(165, orderedBloodPressures[11].MovingAverageSystolic);
            Assert.Equal(175, orderedBloodPressures[12].MovingAverageSystolic);
            Assert.Equal(185, orderedBloodPressures[13].MovingAverageSystolic);
            Assert.Equal(195, orderedBloodPressures[14].MovingAverageSystolic);

            Assert.Equal(60,   orderedBloodPressures[0].MovingAverageDiastolic);
            Assert.Equal(70,   orderedBloodPressures[1].MovingAverageDiastolic);
            Assert.Equal(80,   orderedBloodPressures[2].MovingAverageDiastolic);
            Assert.Equal(90,   orderedBloodPressures[3].MovingAverageDiastolic);
            Assert.Equal(100,   orderedBloodPressures[4].MovingAverageDiastolic);
            Assert.Equal(110,  orderedBloodPressures[5].MovingAverageDiastolic);
            Assert.Equal(120,  orderedBloodPressures[6].MovingAverageDiastolic);
            Assert.Equal(130,  orderedBloodPressures[7].MovingAverageDiastolic);
            Assert.Equal(140,  orderedBloodPressures[8].MovingAverageDiastolic);
            Assert.Equal(150,  orderedBloodPressures[9].MovingAverageDiastolic);
            Assert.Equal(160, orderedBloodPressures[10].MovingAverageDiastolic);
            Assert.Equal(170, orderedBloodPressures[11].MovingAverageDiastolic);
            Assert.Equal(180, orderedBloodPressures[12].MovingAverageDiastolic);
            Assert.Equal(190, orderedBloodPressures[13].MovingAverageDiastolic);
            Assert.Equal(200, orderedBloodPressures[14].MovingAverageDiastolic);


        }


        [Fact]
        public void ShouldSetMovingAveragesOnBloodPressuresWithPartialSeedList()
        {
            var seedBloodPressures = new List<BloodPressure>
            {
                new BloodPressure {Systolic = 40, Diastolic = 45},
                new BloodPressure {Systolic = 50, Diastolic = 55},
                new BloodPressure {Systolic = 60, Diastolic = 65},
                new BloodPressure {Systolic = 70, Diastolic = 75},
                new BloodPressure {Systolic = 80, Diastolic = 85},
                new BloodPressure {Systolic = 90, Diastolic = 95},
            };


            var orderedBloodPressures = new List<BloodPressure>
            {
                new BloodPressure {Systolic = 100, Diastolic = 105},
                new BloodPressure {Systolic = 110, Diastolic = 115},
                new BloodPressure {Systolic = 120, Diastolic = 125},
                new BloodPressure {Systolic = 130, Diastolic = 135},
                new BloodPressure {Systolic = 140, Diastolic = 145},
                new BloodPressure {Systolic = 150, Diastolic = 155},
                new BloodPressure {Systolic = 160, Diastolic = 165},
                new BloodPressure {Systolic = 170, Diastolic = 175},
                new BloodPressure {Systolic = 180, Diastolic = 185},
                new BloodPressure {Systolic = 190, Diastolic = 195},
                new BloodPressure {Systolic = 200, Diastolic = 205},
                new BloodPressure {Systolic = 210, Diastolic = 215},
                new BloodPressure {Systolic = 220, Diastolic = 225},
                new BloodPressure {Systolic = 230, Diastolic = 235},
                new BloodPressure {Systolic = 240, Diastolic = 245},
            };

            _aggregationCalculator.SetMovingAveragesOnBloodPressures(seedBloodPressures, orderedBloodPressures, 10);

            Assert.Equal(15, orderedBloodPressures.Count);

            Assert.Equal(100, orderedBloodPressures[0].Systolic);
            Assert.Equal(110, orderedBloodPressures[1].Systolic);
            Assert.Equal(120, orderedBloodPressures[2].Systolic);
            Assert.Equal(130, orderedBloodPressures[3].Systolic);
            Assert.Equal(140, orderedBloodPressures[4].Systolic);
            Assert.Equal(150, orderedBloodPressures[5].Systolic);
            Assert.Equal(160, orderedBloodPressures[6].Systolic);
            Assert.Equal(170, orderedBloodPressures[7].Systolic);
            Assert.Equal(180, orderedBloodPressures[8].Systolic);
            Assert.Equal(190, orderedBloodPressures[9].Systolic);
            Assert.Equal(200, orderedBloodPressures[10].Systolic);
            Assert.Equal(210, orderedBloodPressures[11].Systolic);
            Assert.Equal(220, orderedBloodPressures[12].Systolic);
            Assert.Equal(230, orderedBloodPressures[13].Systolic);
            Assert.Equal(240, orderedBloodPressures[14].Systolic);

            Assert.Equal(105, orderedBloodPressures[0].Diastolic);
            Assert.Equal(115, orderedBloodPressures[1].Diastolic);
            Assert.Equal(125, orderedBloodPressures[2].Diastolic);
            Assert.Equal(135, orderedBloodPressures[3].Diastolic);
            Assert.Equal(145, orderedBloodPressures[4].Diastolic);
            Assert.Equal(155, orderedBloodPressures[5].Diastolic);
            Assert.Equal(165, orderedBloodPressures[6].Diastolic);
            Assert.Equal(175, orderedBloodPressures[7].Diastolic);
            Assert.Equal(185, orderedBloodPressures[8].Diastolic);
            Assert.Equal(195, orderedBloodPressures[9].Diastolic);
            Assert.Equal(205, orderedBloodPressures[10].Diastolic);
            Assert.Equal(215, orderedBloodPressures[11].Diastolic);
            Assert.Equal(225, orderedBloodPressures[12].Diastolic);
            Assert.Equal(235, orderedBloodPressures[13].Diastolic);
            Assert.Equal(245, orderedBloodPressures[14].Diastolic);


            Assert.Null(orderedBloodPressures[0].MovingAverageSystolic);
            Assert.Null(orderedBloodPressures[1].MovingAverageSystolic);
            Assert.Null(orderedBloodPressures[2].MovingAverageSystolic);
            Assert.Equal(85, orderedBloodPressures[3].MovingAverageSystolic);
            Assert.Equal(95, orderedBloodPressures[4].MovingAverageSystolic);
            Assert.Equal(105, orderedBloodPressures[5].MovingAverageSystolic);
            Assert.Equal(115, orderedBloodPressures[6].MovingAverageSystolic);
            Assert.Equal(125, orderedBloodPressures[7].MovingAverageSystolic);
            Assert.Equal(135, orderedBloodPressures[8].MovingAverageSystolic);
            Assert.Equal(145, orderedBloodPressures[9].MovingAverageSystolic);
            Assert.Equal(155, orderedBloodPressures[10].MovingAverageSystolic);
            Assert.Equal(165, orderedBloodPressures[11].MovingAverageSystolic);
            Assert.Equal(175, orderedBloodPressures[12].MovingAverageSystolic);
            Assert.Equal(185, orderedBloodPressures[13].MovingAverageSystolic);
            Assert.Equal(195, orderedBloodPressures[14].MovingAverageSystolic);

            Assert.Null(orderedBloodPressures[0].MovingAverageDiastolic);
            Assert.Null(orderedBloodPressures[1].MovingAverageDiastolic);
            Assert.Null(orderedBloodPressures[2].MovingAverageDiastolic);
            Assert.Equal(90, orderedBloodPressures[3].MovingAverageDiastolic);
            Assert.Equal(100, orderedBloodPressures[4].MovingAverageDiastolic);
            Assert.Equal(110, orderedBloodPressures[5].MovingAverageDiastolic);
            Assert.Equal(120, orderedBloodPressures[6].MovingAverageDiastolic);
            Assert.Equal(130, orderedBloodPressures[7].MovingAverageDiastolic);
            Assert.Equal(140, orderedBloodPressures[8].MovingAverageDiastolic);
            Assert.Equal(150, orderedBloodPressures[9].MovingAverageDiastolic);
            Assert.Equal(160, orderedBloodPressures[10].MovingAverageDiastolic);
            Assert.Equal(170, orderedBloodPressures[11].MovingAverageDiastolic);
            Assert.Equal(180, orderedBloodPressures[12].MovingAverageDiastolic);
            Assert.Equal(190, orderedBloodPressures[13].MovingAverageDiastolic);
            Assert.Equal(200, orderedBloodPressures[14].MovingAverageDiastolic);

        }

        [Fact]
        public void ShouldSetMovingAveragesOnBloodPressuresWithEmptySeedList()
        {
            var seedBloodPressures = new List<BloodPressure>
            {
            };


            var orderedBloodPressures = new List<BloodPressure>
            {
                new BloodPressure {Systolic = 100, Diastolic = 105},
                new BloodPressure {Systolic = 110, Diastolic = 115},
                new BloodPressure {Systolic = 120, Diastolic = 125},
                new BloodPressure {Systolic = 130, Diastolic = 135},
                new BloodPressure {Systolic = 140, Diastolic = 145},
                new BloodPressure {Systolic = 150, Diastolic = 155},
                new BloodPressure {Systolic = 160, Diastolic = 165},
                new BloodPressure {Systolic = 170, Diastolic = 175},
                new BloodPressure {Systolic = 180, Diastolic = 185},
                new BloodPressure {Systolic = 190, Diastolic = 195},
                new BloodPressure {Systolic = 200, Diastolic = 205},
                new BloodPressure {Systolic = 210, Diastolic = 215},
                new BloodPressure {Systolic = 220, Diastolic = 225},
                new BloodPressure {Systolic = 230, Diastolic = 235},
                new BloodPressure {Systolic = 240, Diastolic = 245},
            };

            _aggregationCalculator.SetMovingAveragesOnBloodPressures(seedBloodPressures, orderedBloodPressures, 10);

            Assert.Equal(15, orderedBloodPressures.Count);

            Assert.Equal(100, orderedBloodPressures[0].Systolic);
            Assert.Equal(110, orderedBloodPressures[1].Systolic);
            Assert.Equal(120, orderedBloodPressures[2].Systolic);
            Assert.Equal(130, orderedBloodPressures[3].Systolic);
            Assert.Equal(140, orderedBloodPressures[4].Systolic);
            Assert.Equal(150, orderedBloodPressures[5].Systolic);
            Assert.Equal(160, orderedBloodPressures[6].Systolic);
            Assert.Equal(170, orderedBloodPressures[7].Systolic);
            Assert.Equal(180, orderedBloodPressures[8].Systolic);
            Assert.Equal(190, orderedBloodPressures[9].Systolic);
            Assert.Equal(200, orderedBloodPressures[10].Systolic);
            Assert.Equal(210, orderedBloodPressures[11].Systolic);
            Assert.Equal(220, orderedBloodPressures[12].Systolic);
            Assert.Equal(230, orderedBloodPressures[13].Systolic);
            Assert.Equal(240, orderedBloodPressures[14].Systolic);

            Assert.Equal(105, orderedBloodPressures[0].Diastolic);
            Assert.Equal(115, orderedBloodPressures[1].Diastolic);
            Assert.Equal(125, orderedBloodPressures[2].Diastolic);
            Assert.Equal(135, orderedBloodPressures[3].Diastolic);
            Assert.Equal(145, orderedBloodPressures[4].Diastolic);
            Assert.Equal(155, orderedBloodPressures[5].Diastolic);
            Assert.Equal(165, orderedBloodPressures[6].Diastolic);
            Assert.Equal(175, orderedBloodPressures[7].Diastolic);
            Assert.Equal(185, orderedBloodPressures[8].Diastolic);
            Assert.Equal(195, orderedBloodPressures[9].Diastolic);
            Assert.Equal(205, orderedBloodPressures[10].Diastolic);
            Assert.Equal(215, orderedBloodPressures[11].Diastolic);
            Assert.Equal(225, orderedBloodPressures[12].Diastolic);
            Assert.Equal(235, orderedBloodPressures[13].Diastolic);
            Assert.Equal(245, orderedBloodPressures[14].Diastolic);


            Assert.Null(orderedBloodPressures[0].MovingAverageSystolic);
            Assert.Null(orderedBloodPressures[1].MovingAverageSystolic);
            Assert.Null(orderedBloodPressures[2].MovingAverageSystolic);
            Assert.Null(orderedBloodPressures[3].MovingAverageSystolic);
            Assert.Null(orderedBloodPressures[4].MovingAverageSystolic);
            Assert.Null( orderedBloodPressures[5].MovingAverageSystolic);
            Assert.Null( orderedBloodPressures[6].MovingAverageSystolic);
            Assert.Null( orderedBloodPressures[7].MovingAverageSystolic);
            Assert.Null( orderedBloodPressures[8].MovingAverageSystolic);
            Assert.Equal(145, orderedBloodPressures[9].MovingAverageSystolic);
            Assert.Equal(155, orderedBloodPressures[10].MovingAverageSystolic);
            Assert.Equal(165, orderedBloodPressures[11].MovingAverageSystolic);
            Assert.Equal(175, orderedBloodPressures[12].MovingAverageSystolic);
            Assert.Equal(185, orderedBloodPressures[13].MovingAverageSystolic);
            Assert.Equal(195, orderedBloodPressures[14].MovingAverageSystolic);

            Assert.Null(orderedBloodPressures[0].MovingAverageDiastolic);
            Assert.Null(orderedBloodPressures[1].MovingAverageDiastolic);
            Assert.Null(orderedBloodPressures[2].MovingAverageDiastolic);
            Assert.Null(orderedBloodPressures[3].MovingAverageDiastolic);
            Assert.Null( orderedBloodPressures[4].MovingAverageDiastolic);
            Assert.Null( orderedBloodPressures[5].MovingAverageDiastolic);
            Assert.Null( orderedBloodPressures[6].MovingAverageDiastolic);
            Assert.Null( orderedBloodPressures[7].MovingAverageDiastolic);
            Assert.Null( orderedBloodPressures[8].MovingAverageDiastolic);
            Assert.Equal(150, orderedBloodPressures[9].MovingAverageDiastolic);
            Assert.Equal(160, orderedBloodPressures[10].MovingAverageDiastolic);
            Assert.Equal(170, orderedBloodPressures[11].MovingAverageDiastolic);
            Assert.Equal(180, orderedBloodPressures[12].MovingAverageDiastolic);
            Assert.Equal(190, orderedBloodPressures[13].MovingAverageDiastolic);
            Assert.Equal(200, orderedBloodPressures[14].MovingAverageDiastolic);

        }
    }
}