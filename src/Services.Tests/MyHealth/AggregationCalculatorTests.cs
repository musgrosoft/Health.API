using System.Collections.Generic;
using Repositories.Models;
using Services.MyHealth;
using Xunit;
using System.Linq;

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
        public void ShouldSetMovingAveragesOnWeights()
        {


            var orderedWeights = new List<Weight>
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

            var resultWeights = _aggregationCalculator.GetMovingAverages( orderedWeights,10).ToList();

            Assert.Equal(24, resultWeights.Count());

            Assert.Equal(10, resultWeights[0].Kg);
            Assert.Equal(20, resultWeights[1].Kg);
            Assert.Equal(30, resultWeights[2].Kg);
            Assert.Equal(40, resultWeights[3].Kg);
            Assert.Equal(50, resultWeights[4].Kg);
            Assert.Equal(60, resultWeights[5].Kg);
            Assert.Equal(70, resultWeights[6].Kg);
            Assert.Equal(80, resultWeights[7].Kg);
            Assert.Equal(90, resultWeights[8].Kg);

            Assert.Equal(100, resultWeights[9].Kg);
            Assert.Equal(110, resultWeights[10].Kg);
            Assert.Equal(120, resultWeights[11].Kg);
            Assert.Equal(130, resultWeights[12].Kg);
            Assert.Equal(140, resultWeights[13].Kg);
            Assert.Equal(150, resultWeights[14].Kg);
            Assert.Equal(160, resultWeights[15].Kg);
            Assert.Equal(170, resultWeights[16].Kg);
            Assert.Equal(180, resultWeights[17].Kg);
            Assert.Equal(190, resultWeights[18].Kg);
            Assert.Equal(200, resultWeights[19].Kg);
            Assert.Equal(210, resultWeights[20].Kg);
            Assert.Equal(220, resultWeights[21].Kg);
            Assert.Equal(230, resultWeights[22].Kg);
            Assert.Equal(240, resultWeights[23].Kg);

            Assert.Equal(null, resultWeights[0].MovingAverageKg);
            Assert.Equal(null, resultWeights[1].MovingAverageKg);
            Assert.Equal(null, resultWeights[2].MovingAverageKg);
            Assert.Equal(null, resultWeights[3].MovingAverageKg);
            Assert.Equal(null, resultWeights[4].MovingAverageKg);
            Assert.Equal(null, resultWeights[5].MovingAverageKg);
            Assert.Equal(null, resultWeights[6].MovingAverageKg);
            Assert.Equal(null, resultWeights[7].MovingAverageKg);
            Assert.Equal(null, resultWeights[8].MovingAverageKg);

            Assert.Equal(55, resultWeights[9].MovingAverageKg);
            Assert.Equal(65, resultWeights[10].MovingAverageKg);
            Assert.Equal(75, resultWeights[11].MovingAverageKg);
            Assert.Equal(85, resultWeights[12].MovingAverageKg);
            Assert.Equal(95, resultWeights[13].MovingAverageKg);
            Assert.Equal(105, resultWeights[14].MovingAverageKg);
            Assert.Equal(115, resultWeights[15].MovingAverageKg);
            Assert.Equal(125, resultWeights[16].MovingAverageKg);
            Assert.Equal(135, resultWeights[17].MovingAverageKg);
            Assert.Equal(145, resultWeights[18].MovingAverageKg);
            Assert.Equal(155, resultWeights[19].MovingAverageKg);
            Assert.Equal(165, resultWeights[20].MovingAverageKg);
            Assert.Equal(175, resultWeights[21].MovingAverageKg);
            Assert.Equal(185, resultWeights[22].MovingAverageKg);
            Assert.Equal(195, resultWeights[23].MovingAverageKg);

        }


        //[Fact]
        //public void ShouldSetMovingAveragesOnWeightsWithPartialSeedList()
        //{
        //    var seedWeights = new List<Weight>
        //    {
        //        new Weight {Kg = 40},
        //        new Weight {Kg = 50},
        //        new Weight {Kg = 60},
        //        new Weight {Kg = 70},
        //        new Weight {Kg = 80},
        //        new Weight {Kg = 90},

        //    };

        //    var orderedWeights = new List<Weight>
        //    {
        //        new Weight {Kg = 100},
        //        new Weight {Kg = 110},
        //        new Weight {Kg = 120},
        //        new Weight {Kg = 130},
        //        new Weight {Kg = 140},
        //        new Weight {Kg = 150},
        //        new Weight {Kg = 160},
        //        new Weight {Kg = 170},
        //        new Weight {Kg = 180},
        //        new Weight {Kg = 190},
        //        new Weight {Kg = 200},
        //        new Weight {Kg = 210},
        //        new Weight {Kg = 220},
        //        new Weight {Kg = 230},
        //        new Weight {Kg = 240},
        //    };

        //    var resultWeights = _aggregationCalculator.GetMovingAverages(seedWeights, orderedWeights, 10).ToList();

        //    Assert.Equal(15, resultWeights.Count());

        //    Assert.Equal(100, resultWeights[0].Kg);
        //    Assert.Equal(110, resultWeights[1].Kg);
        //    Assert.Equal(120, resultWeights[2].Kg);
        //    Assert.Equal(130, resultWeights[3].Kg);
        //    Assert.Equal(140, resultWeights[4].Kg);
        //    Assert.Equal(150, resultWeights[5].Kg);
        //    Assert.Equal(160, resultWeights[6].Kg);
        //    Assert.Equal(170, resultWeights[7].Kg);
        //    Assert.Equal(180, resultWeights[8].Kg);
        //    Assert.Equal(190, resultWeights[9].Kg);
        //    Assert.Equal(200, resultWeights[10].Kg);
        //    Assert.Equal(210, resultWeights[11].Kg);
        //    Assert.Equal(220, resultWeights[12].Kg);
        //    Assert.Equal(230, resultWeights[13].Kg);
        //    Assert.Equal(240, resultWeights[14].Kg);

        //    Assert.Null(resultWeights[0].MovingAverageKg);
        //    Assert.Null(resultWeights[1].MovingAverageKg);
        //    Assert.Null(resultWeights[2].MovingAverageKg);
        //    Assert.Equal(85, resultWeights[3].MovingAverageKg);
        //    Assert.Equal(95, resultWeights[4].MovingAverageKg);
        //    Assert.Equal(105, resultWeights[5].MovingAverageKg);
        //    Assert.Equal(115, resultWeights[6].MovingAverageKg);
        //    Assert.Equal(125, resultWeights[7].MovingAverageKg);
        //    Assert.Equal(135, resultWeights[8].MovingAverageKg);
        //    Assert.Equal(145, resultWeights[9].MovingAverageKg);
        //    Assert.Equal(155, resultWeights[10].MovingAverageKg);
        //    Assert.Equal(165, resultWeights[11].MovingAverageKg);
        //    Assert.Equal(175, resultWeights[12].MovingAverageKg);
        //    Assert.Equal(185, resultWeights[13].MovingAverageKg);
        //    Assert.Equal(195, resultWeights[14].MovingAverageKg);

        //}

        //[Fact]
        //public void ShouldSetMovingAveragesOnWeightsWithEmptySeedList()
        //{
        //    var seedWeights = new List<Weight>
        //    {

        //    };

        //    var orderedWeights = new List<Weight>
        //    {
        //        new Weight {Kg = 100},
        //        new Weight {Kg = 110},
        //        new Weight {Kg = 120},
        //        new Weight {Kg = 130},
        //        new Weight {Kg = 140},
        //        new Weight {Kg = 150},
        //        new Weight {Kg = 160},
        //        new Weight {Kg = 170},
        //        new Weight {Kg = 180},
        //        new Weight {Kg = 190},
        //        new Weight {Kg = 200},
        //        new Weight {Kg = 210},
        //        new Weight {Kg = 220},
        //        new Weight {Kg = 230},
        //        new Weight {Kg = 240},
        //    };

        //    var resultWeights = _aggregationCalculator.GetMovingAverages(seedWeights, orderedWeights, 10).ToList();

        //    Assert.Equal(15, resultWeights.Count);

        //    Assert.Equal(100, resultWeights[0].Kg);
        //    Assert.Equal(110, resultWeights[1].Kg);
        //    Assert.Equal(120, resultWeights[2].Kg);
        //    Assert.Equal(130, resultWeights[3].Kg);
        //    Assert.Equal(140, resultWeights[4].Kg);
        //    Assert.Equal(150, resultWeights[5].Kg);
        //    Assert.Equal(160, resultWeights[6].Kg);
        //    Assert.Equal(170, resultWeights[7].Kg);
        //    Assert.Equal(180, resultWeights[8].Kg);
        //    Assert.Equal(190, resultWeights[9].Kg);
        //    Assert.Equal(200, resultWeights[10].Kg);
        //    Assert.Equal(210, resultWeights[11].Kg);
        //    Assert.Equal(220, resultWeights[12].Kg);
        //    Assert.Equal(230, resultWeights[13].Kg);
        //    Assert.Equal(240, resultWeights[14].Kg);

        //    Assert.Null(resultWeights[0].MovingAverageKg);
        //    Assert.Null(resultWeights[1].MovingAverageKg);
        //    Assert.Null(resultWeights[2].MovingAverageKg);
        //    Assert.Null(resultWeights[3].MovingAverageKg);
        //    Assert.Null(resultWeights[4].MovingAverageKg);
        //    Assert.Null(resultWeights[5].MovingAverageKg);
        //    Assert.Null(resultWeights[6].MovingAverageKg);
        //    Assert.Null(resultWeights[7].MovingAverageKg);
        //    Assert.Null(resultWeights[8].MovingAverageKg);
        //    Assert.Equal(145, resultWeights[9].MovingAverageKg);
        //    Assert.Equal(155, resultWeights[10].MovingAverageKg);
        //    Assert.Equal(165, resultWeights[11].MovingAverageKg);
        //    Assert.Equal(175, resultWeights[12].MovingAverageKg);
        //    Assert.Equal(185, resultWeights[13].MovingAverageKg);
        //    Assert.Equal(195, resultWeights[14].MovingAverageKg);

        //}

        //[Fact]
        //public void ShouldSetMovingAveragesOnRestingHeartRatesWithFullSeedList()
        //{
        //    var seedRestingHeartRates = new List<RestingHeartRate>
        //    {
        //        new RestingHeartRate {Beats = 10},
        //        new RestingHeartRate {Beats = 20},
        //        new RestingHeartRate {Beats = 30},
        //        new RestingHeartRate {Beats = 40},
        //        new RestingHeartRate {Beats = 50},
        //        new RestingHeartRate {Beats = 60},
        //        new RestingHeartRate {Beats = 70},
        //        new RestingHeartRate {Beats = 80},
        //        new RestingHeartRate {Beats = 90},
        //    };


        //    var orderedRestingHeartRates = new List<RestingHeartRate>
        //    {
        //        new RestingHeartRate {Beats = 100},
        //        new RestingHeartRate {Beats = 110},
        //        new RestingHeartRate {Beats = 120},
        //        new RestingHeartRate {Beats = 130},
        //        new RestingHeartRate {Beats = 140},
        //        new RestingHeartRate {Beats = 150},
        //        new RestingHeartRate {Beats = 160},
        //        new RestingHeartRate {Beats = 170},
        //        new RestingHeartRate {Beats = 180},
        //        new RestingHeartRate {Beats = 190},
        //        new RestingHeartRate {Beats = 200},
        //        new RestingHeartRate {Beats = 210},
        //        new RestingHeartRate {Beats = 220},
        //        new RestingHeartRate {Beats = 230},
        //        new RestingHeartRate {Beats = 240},
        //    };

        //    var resultRestingHeartRates = _aggregationCalculator.GetMovingAverages(seedRestingHeartRates, orderedRestingHeartRates, 10).ToList();

        //    Assert.Equal(15, resultRestingHeartRates.Count);

        //    Assert.Equal(100, resultRestingHeartRates[0].Beats);
        //    Assert.Equal(110, resultRestingHeartRates[1].Beats);
        //    Assert.Equal(120, resultRestingHeartRates[2].Beats);
        //    Assert.Equal(130, resultRestingHeartRates[3].Beats);
        //    Assert.Equal(140, resultRestingHeartRates[4].Beats);
        //    Assert.Equal(150, resultRestingHeartRates[5].Beats);
        //    Assert.Equal(160, resultRestingHeartRates[6].Beats);
        //    Assert.Equal(170, resultRestingHeartRates[7].Beats);
        //    Assert.Equal(180, resultRestingHeartRates[8].Beats);
        //    Assert.Equal(190, resultRestingHeartRates[9].Beats);
        //    Assert.Equal(200, resultRestingHeartRates[10].Beats);
        //    Assert.Equal(210, resultRestingHeartRates[11].Beats);
        //    Assert.Equal(220, resultRestingHeartRates[12].Beats);
        //    Assert.Equal(230, resultRestingHeartRates[13].Beats);
        //    Assert.Equal(240, resultRestingHeartRates[14].Beats);

        //    Assert.Equal(55, resultRestingHeartRates[0].MovingAverageBeats);
        //    Assert.Equal(65, resultRestingHeartRates[1].MovingAverageBeats);
        //    Assert.Equal(75, resultRestingHeartRates[2].MovingAverageBeats);
        //    Assert.Equal(85, resultRestingHeartRates[3].MovingAverageBeats);
        //    Assert.Equal(95, resultRestingHeartRates[4].MovingAverageBeats);
        //    Assert.Equal(105, resultRestingHeartRates[5].MovingAverageBeats);
        //    Assert.Equal(115, resultRestingHeartRates[6].MovingAverageBeats);
        //    Assert.Equal(125, resultRestingHeartRates[7].MovingAverageBeats);
        //    Assert.Equal(135, resultRestingHeartRates[8].MovingAverageBeats);
        //    Assert.Equal(145, resultRestingHeartRates[9].MovingAverageBeats);
        //    Assert.Equal(155, resultRestingHeartRates[10].MovingAverageBeats);
        //    Assert.Equal(165, resultRestingHeartRates[11].MovingAverageBeats);
        //    Assert.Equal(175, resultRestingHeartRates[12].MovingAverageBeats);
        //    Assert.Equal(185, resultRestingHeartRates[13].MovingAverageBeats);
        //    Assert.Equal(195, resultRestingHeartRates[14].MovingAverageBeats);

        //}


        //[Fact]
        //public void ShouldSetMovingAveragesOnRestingHeartRatesWithPartialSeedList()
        //{
        //    var seedRestingHeartRates = new List<RestingHeartRate>
        //    {
        //        new RestingHeartRate {Beats = 40},
        //        new RestingHeartRate {Beats = 50},
        //        new RestingHeartRate {Beats = 60},
        //        new RestingHeartRate {Beats = 70},
        //        new RestingHeartRate {Beats = 80},
        //        new RestingHeartRate {Beats = 90},

        //    };

        //    var orderedRestingHeartRates = new List<RestingHeartRate>
        //    {
        //        new RestingHeartRate {Beats = 100},
        //        new RestingHeartRate {Beats = 110},
        //        new RestingHeartRate {Beats = 120},
        //        new RestingHeartRate {Beats = 130},
        //        new RestingHeartRate {Beats = 140},
        //        new RestingHeartRate {Beats = 150},
        //        new RestingHeartRate {Beats = 160},
        //        new RestingHeartRate {Beats = 170},
        //        new RestingHeartRate {Beats = 180},
        //        new RestingHeartRate {Beats = 190},
        //        new RestingHeartRate {Beats = 200},
        //        new RestingHeartRate {Beats = 210},
        //        new RestingHeartRate {Beats = 220},
        //        new RestingHeartRate {Beats = 230},
        //        new RestingHeartRate {Beats = 240},
        //    };

        //    var resultRestingHeartRates = _aggregationCalculator.GetMovingAverages(seedRestingHeartRates, orderedRestingHeartRates, 10).ToList();

        //    Assert.Equal(15, resultRestingHeartRates.Count);

        //    Assert.Equal(100, resultRestingHeartRates[0].Beats);
        //    Assert.Equal(110, resultRestingHeartRates[1].Beats);
        //    Assert.Equal(120, resultRestingHeartRates[2].Beats);
        //    Assert.Equal(130, resultRestingHeartRates[3].Beats);
        //    Assert.Equal(140, resultRestingHeartRates[4].Beats);
        //    Assert.Equal(150, resultRestingHeartRates[5].Beats);
        //    Assert.Equal(160, resultRestingHeartRates[6].Beats);
        //    Assert.Equal(170, resultRestingHeartRates[7].Beats);
        //    Assert.Equal(180, resultRestingHeartRates[8].Beats);
        //    Assert.Equal(190, resultRestingHeartRates[9].Beats);
        //    Assert.Equal(200, resultRestingHeartRates[10].Beats);
        //    Assert.Equal(210, resultRestingHeartRates[11].Beats);
        //    Assert.Equal(220, resultRestingHeartRates[12].Beats);
        //    Assert.Equal(230, resultRestingHeartRates[13].Beats);
        //    Assert.Equal(240, resultRestingHeartRates[14].Beats);

        //    Assert.Null(resultRestingHeartRates[0].MovingAverageBeats);
        //    Assert.Null(resultRestingHeartRates[1].MovingAverageBeats);
        //    Assert.Null(resultRestingHeartRates[2].MovingAverageBeats);
        //    Assert.Equal(85, resultRestingHeartRates[3].MovingAverageBeats);
        //    Assert.Equal(95, resultRestingHeartRates[4].MovingAverageBeats);
        //    Assert.Equal(105, resultRestingHeartRates[5].MovingAverageBeats);
        //    Assert.Equal(115, resultRestingHeartRates[6].MovingAverageBeats);
        //    Assert.Equal(125, resultRestingHeartRates[7].MovingAverageBeats);
        //    Assert.Equal(135, resultRestingHeartRates[8].MovingAverageBeats);
        //    Assert.Equal(145, resultRestingHeartRates[9].MovingAverageBeats);
        //    Assert.Equal(155, resultRestingHeartRates[10].MovingAverageBeats);
        //    Assert.Equal(165, resultRestingHeartRates[11].MovingAverageBeats);
        //    Assert.Equal(175, resultRestingHeartRates[12].MovingAverageBeats);
        //    Assert.Equal(185, resultRestingHeartRates[13].MovingAverageBeats);
        //    Assert.Equal(195, resultRestingHeartRates[14].MovingAverageBeats);

        //}

        [Fact]
        public void ShouldSetMovingAveragesOnRestingHeartRatesWithEmptySeedList()
        {

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

            var resultRestingHeartRates = _aggregationCalculator.GetMovingAverages(orderedRestingHeartRates, 10).ToList();

            Assert.Equal(15, resultRestingHeartRates.Count);

            Assert.Equal(100, resultRestingHeartRates[0].Beats);
            Assert.Equal(110, resultRestingHeartRates[1].Beats);
            Assert.Equal(120, resultRestingHeartRates[2].Beats);
            Assert.Equal(130, resultRestingHeartRates[3].Beats);
            Assert.Equal(140, resultRestingHeartRates[4].Beats);
            Assert.Equal(150, resultRestingHeartRates[5].Beats);
            Assert.Equal(160, resultRestingHeartRates[6].Beats);
            Assert.Equal(170, resultRestingHeartRates[7].Beats);
            Assert.Equal(180, resultRestingHeartRates[8].Beats);
            Assert.Equal(190, resultRestingHeartRates[9].Beats);
            Assert.Equal(200, resultRestingHeartRates[10].Beats);
            Assert.Equal(210, resultRestingHeartRates[11].Beats);
            Assert.Equal(220, resultRestingHeartRates[12].Beats);
            Assert.Equal(230, resultRestingHeartRates[13].Beats);
            Assert.Equal(240, resultRestingHeartRates[14].Beats);

            Assert.Null(resultRestingHeartRates[0].MovingAverageBeats);
            Assert.Null(resultRestingHeartRates[1].MovingAverageBeats);
            Assert.Null(resultRestingHeartRates[2].MovingAverageBeats);
            Assert.Null(resultRestingHeartRates[3].MovingAverageBeats);
            Assert.Null(resultRestingHeartRates[4].MovingAverageBeats);
            Assert.Null(resultRestingHeartRates[5].MovingAverageBeats);
            Assert.Null(resultRestingHeartRates[6].MovingAverageBeats);
            Assert.Null(resultRestingHeartRates[7].MovingAverageBeats);
            Assert.Null(resultRestingHeartRates[8].MovingAverageBeats);
            Assert.Equal(145, resultRestingHeartRates[9].MovingAverageBeats);
            Assert.Equal(155, resultRestingHeartRates[10].MovingAverageBeats);
            Assert.Equal(165, resultRestingHeartRates[11].MovingAverageBeats);
            Assert.Equal(175, resultRestingHeartRates[12].MovingAverageBeats);
            Assert.Equal(185, resultRestingHeartRates[13].MovingAverageBeats);
            Assert.Equal(195, resultRestingHeartRates[14].MovingAverageBeats);

        }










        [Fact]
        public void ShouldSetMovingAveragesOnBloodPressure()
        {



            var orderedBloodPressures = new List<BloodPressure>
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

            var resultBloodPressures = _aggregationCalculator.GetMovingAverages(orderedBloodPressures, 10).ToList();

            Assert.Equal(24, resultBloodPressures.Count);

            Assert.Equal(10, resultBloodPressures[0].Systolic);
            Assert.Equal(20, resultBloodPressures[1].Systolic);
            Assert.Equal(30, resultBloodPressures[2].Systolic);
            Assert.Equal(40, resultBloodPressures[3].Systolic);
            Assert.Equal(50, resultBloodPressures[4].Systolic);
            Assert.Equal(60, resultBloodPressures[5].Systolic);
            Assert.Equal(70, resultBloodPressures[6].Systolic);
            Assert.Equal(80, resultBloodPressures[7].Systolic);
            Assert.Equal(90, resultBloodPressures[8].Systolic);
            Assert.Equal(100,  resultBloodPressures[9].Systolic);
            Assert.Equal(110,  resultBloodPressures[10].Systolic);
            Assert.Equal(120,  resultBloodPressures[11].Systolic);
            Assert.Equal(130,  resultBloodPressures[12].Systolic);
            Assert.Equal(140,  resultBloodPressures[13].Systolic);
            Assert.Equal(150,  resultBloodPressures[14].Systolic);
            Assert.Equal(160,  resultBloodPressures[15].Systolic);
            Assert.Equal(170,  resultBloodPressures[16].Systolic);
            Assert.Equal(180,  resultBloodPressures[17].Systolic);
            Assert.Equal(190, resultBloodPressures[18].Systolic);
            Assert.Equal(200, resultBloodPressures[19].Systolic);
            Assert.Equal(210, resultBloodPressures[20].Systolic);
            Assert.Equal(220, resultBloodPressures[21].Systolic);
            Assert.Equal(230, resultBloodPressures[22].Systolic);
            Assert.Equal(240, resultBloodPressures[23].Systolic);

            Assert.Equal(15, resultBloodPressures[0].Diastolic);
            Assert.Equal(25, resultBloodPressures[1].Diastolic);
            Assert.Equal(35, resultBloodPressures[2].Diastolic);
            Assert.Equal(45, resultBloodPressures[3].Diastolic);
            Assert.Equal(55, resultBloodPressures[4].Diastolic);
            Assert.Equal(65, resultBloodPressures[5].Diastolic);
            Assert.Equal(75, resultBloodPressures[6].Diastolic);
            Assert.Equal(85, resultBloodPressures[7].Diastolic);
            Assert.Equal(95, resultBloodPressures[8].Diastolic);

            Assert.Equal(105,  resultBloodPressures[9].Diastolic);
            Assert.Equal(115,  resultBloodPressures[10].Diastolic);
            Assert.Equal(125,  resultBloodPressures[11].Diastolic);
            Assert.Equal(135,  resultBloodPressures[12].Diastolic);
            Assert.Equal(145,  resultBloodPressures[13].Diastolic);
            Assert.Equal(155,  resultBloodPressures[14].Diastolic);
            Assert.Equal(165,  resultBloodPressures[15].Diastolic);
            Assert.Equal(175,  resultBloodPressures[16].Diastolic);
            Assert.Equal(185,  resultBloodPressures[17].Diastolic);
            Assert.Equal(195, resultBloodPressures[18].Diastolic);
            Assert.Equal(205, resultBloodPressures[19].Diastolic);
            Assert.Equal(215, resultBloodPressures[20].Diastolic);
            Assert.Equal(225, resultBloodPressures[21].Diastolic);
            Assert.Equal(235, resultBloodPressures[22].Diastolic);
            Assert.Equal(245, resultBloodPressures[23].Diastolic);



            Assert.Equal(null, resultBloodPressures[0].MovingAverageSystolic);
            Assert.Equal(null, resultBloodPressures[1].MovingAverageSystolic);
            Assert.Equal(null, resultBloodPressures[2].MovingAverageSystolic);
            Assert.Equal(null, resultBloodPressures[3].MovingAverageSystolic);
            Assert.Equal(null, resultBloodPressures[4].MovingAverageSystolic);
            Assert.Equal(null, resultBloodPressures[5].MovingAverageSystolic);
            Assert.Equal(null, resultBloodPressures[6].MovingAverageSystolic);
            Assert.Equal(null, resultBloodPressures[7].MovingAverageSystolic);
            Assert.Equal(null, resultBloodPressures[8].MovingAverageSystolic);

            Assert.Equal(55,   resultBloodPressures[9].MovingAverageSystolic);
            Assert.Equal(65,   resultBloodPressures[10].MovingAverageSystolic);
            Assert.Equal(75,   resultBloodPressures[11].MovingAverageSystolic);
            Assert.Equal(85,   resultBloodPressures[12].MovingAverageSystolic);
            Assert.Equal(95,   resultBloodPressures[13].MovingAverageSystolic);
            Assert.Equal(105,  resultBloodPressures[14].MovingAverageSystolic);
            Assert.Equal(115,  resultBloodPressures[15].MovingAverageSystolic);
            Assert.Equal(125,  resultBloodPressures[16].MovingAverageSystolic);
            Assert.Equal(135,  resultBloodPressures[17].MovingAverageSystolic);
            Assert.Equal(145, resultBloodPressures[18].MovingAverageSystolic);
            Assert.Equal(155, resultBloodPressures[19].MovingAverageSystolic);
            Assert.Equal(165, resultBloodPressures[20].MovingAverageSystolic);
            Assert.Equal(175, resultBloodPressures[21].MovingAverageSystolic);
            Assert.Equal(185, resultBloodPressures[22].MovingAverageSystolic);
            Assert.Equal(195, resultBloodPressures[23].MovingAverageSystolic);


            Assert.Equal(null, resultBloodPressures[0].MovingAverageDiastolic);
            Assert.Equal(null, resultBloodPressures[1].MovingAverageDiastolic);
            Assert.Equal(null, resultBloodPressures[2].MovingAverageDiastolic);
            Assert.Equal(null, resultBloodPressures[3].MovingAverageDiastolic);
            Assert.Equal(null, resultBloodPressures[4].MovingAverageDiastolic);
            Assert.Equal(null, resultBloodPressures[5].MovingAverageDiastolic);
            Assert.Equal(null, resultBloodPressures[6].MovingAverageDiastolic);
            Assert.Equal(null, resultBloodPressures[7].MovingAverageDiastolic);
            Assert.Equal(null, resultBloodPressures[8].MovingAverageDiastolic);

            Assert.Equal(60,   resultBloodPressures[9].MovingAverageDiastolic);
            Assert.Equal(70,   resultBloodPressures[10].MovingAverageDiastolic);
            Assert.Equal(80,   resultBloodPressures[11].MovingAverageDiastolic);
            Assert.Equal(90, resultBloodPressures[12].MovingAverageDiastolic);
            Assert.Equal(100, resultBloodPressures[13].MovingAverageDiastolic);
            Assert.Equal(110,  resultBloodPressures[14].MovingAverageDiastolic);
            Assert.Equal(120,  resultBloodPressures[15].MovingAverageDiastolic);
            Assert.Equal(130,  resultBloodPressures[16].MovingAverageDiastolic);
            Assert.Equal(140,  resultBloodPressures[17].MovingAverageDiastolic);
            Assert.Equal(150, resultBloodPressures[18].MovingAverageDiastolic);
            Assert.Equal(160, resultBloodPressures[19].MovingAverageDiastolic);
            Assert.Equal(170, resultBloodPressures[20].MovingAverageDiastolic);
            Assert.Equal(180, resultBloodPressures[21].MovingAverageDiastolic);
            Assert.Equal(190, resultBloodPressures[22].MovingAverageDiastolic);
            Assert.Equal(200, resultBloodPressures[23].MovingAverageDiastolic);


        }


        //[Fact]
        //public void ShouldSetMovingAveragesOnBloodPressuresWithPartialSeedList()
        //{
        //    var seedBloodPressures = new List<BloodPressure>
        //    {
        //        new BloodPressure {Systolic = 40, Diastolic = 45},
        //        new BloodPressure {Systolic = 50, Diastolic = 55},
        //        new BloodPressure {Systolic = 60, Diastolic = 65},
        //        new BloodPressure {Systolic = 70, Diastolic = 75},
        //        new BloodPressure {Systolic = 80, Diastolic = 85},
        //        new BloodPressure {Systolic = 90, Diastolic = 95},
        //    };


        //    var orderedBloodPressures = new List<BloodPressure>
        //    {
        //        new BloodPressure {Systolic = 100, Diastolic = 105},
        //        new BloodPressure {Systolic = 110, Diastolic = 115},
        //        new BloodPressure {Systolic = 120, Diastolic = 125},
        //        new BloodPressure {Systolic = 130, Diastolic = 135},
        //        new BloodPressure {Systolic = 140, Diastolic = 145},
        //        new BloodPressure {Systolic = 150, Diastolic = 155},
        //        new BloodPressure {Systolic = 160, Diastolic = 165},
        //        new BloodPressure {Systolic = 170, Diastolic = 175},
        //        new BloodPressure {Systolic = 180, Diastolic = 185},
        //        new BloodPressure {Systolic = 190, Diastolic = 195},
        //        new BloodPressure {Systolic = 200, Diastolic = 205},
        //        new BloodPressure {Systolic = 210, Diastolic = 215},
        //        new BloodPressure {Systolic = 220, Diastolic = 225},
        //        new BloodPressure {Systolic = 230, Diastolic = 235},
        //        new BloodPressure {Systolic = 240, Diastolic = 245},
        //    };

        //    var resultBloodPressures = _aggregationCalculator.GetMovingAverages(seedBloodPressures, orderedBloodPressures, 10).ToList();

        //    Assert.Equal(15, resultBloodPressures.Count);

        //    Assert.Equal(100, resultBloodPressures[0].Systolic);
        //    Assert.Equal(110, resultBloodPressures[1].Systolic);
        //    Assert.Equal(120, resultBloodPressures[2].Systolic);
        //    Assert.Equal(130, resultBloodPressures[3].Systolic);
        //    Assert.Equal(140, resultBloodPressures[4].Systolic);
        //    Assert.Equal(150, resultBloodPressures[5].Systolic);
        //    Assert.Equal(160, resultBloodPressures[6].Systolic);
        //    Assert.Equal(170, resultBloodPressures[7].Systolic);
        //    Assert.Equal(180, resultBloodPressures[8].Systolic);
        //    Assert.Equal(190, resultBloodPressures[9].Systolic);
        //    Assert.Equal(200, resultBloodPressures[10].Systolic);
        //    Assert.Equal(210, resultBloodPressures[11].Systolic);
        //    Assert.Equal(220, resultBloodPressures[12].Systolic);
        //    Assert.Equal(230, resultBloodPressures[13].Systolic);
        //    Assert.Equal(240, resultBloodPressures[14].Systolic);

        //    Assert.Equal(105, resultBloodPressures[0].Diastolic);
        //    Assert.Equal(115, resultBloodPressures[1].Diastolic);
        //    Assert.Equal(125, resultBloodPressures[2].Diastolic);
        //    Assert.Equal(135, resultBloodPressures[3].Diastolic);
        //    Assert.Equal(145, resultBloodPressures[4].Diastolic);
        //    Assert.Equal(155, resultBloodPressures[5].Diastolic);
        //    Assert.Equal(165, resultBloodPressures[6].Diastolic);
        //    Assert.Equal(175, resultBloodPressures[7].Diastolic);
        //    Assert.Equal(185, resultBloodPressures[8].Diastolic);
        //    Assert.Equal(195, resultBloodPressures[9].Diastolic);
        //    Assert.Equal(205, resultBloodPressures[10].Diastolic);
        //    Assert.Equal(215, resultBloodPressures[11].Diastolic);
        //    Assert.Equal(225, resultBloodPressures[12].Diastolic);
        //    Assert.Equal(235, resultBloodPressures[13].Diastolic);
        //    Assert.Equal(245, resultBloodPressures[14].Diastolic);


        //    Assert.Null(resultBloodPressures[0].MovingAverageSystolic);
        //    Assert.Null(resultBloodPressures[1].MovingAverageSystolic);
        //    Assert.Null(resultBloodPressures[2].MovingAverageSystolic);
        //    Assert.Equal(85, resultBloodPressures[3].MovingAverageSystolic);
        //    Assert.Equal(95, resultBloodPressures[4].MovingAverageSystolic);
        //    Assert.Equal(105, resultBloodPressures[5].MovingAverageSystolic);
        //    Assert.Equal(115, resultBloodPressures[6].MovingAverageSystolic);
        //    Assert.Equal(125, resultBloodPressures[7].MovingAverageSystolic);
        //    Assert.Equal(135, resultBloodPressures[8].MovingAverageSystolic);
        //    Assert.Equal(145, resultBloodPressures[9].MovingAverageSystolic);
        //    Assert.Equal(155, resultBloodPressures[10].MovingAverageSystolic);
        //    Assert.Equal(165, resultBloodPressures[11].MovingAverageSystolic);
        //    Assert.Equal(175, resultBloodPressures[12].MovingAverageSystolic);
        //    Assert.Equal(185, resultBloodPressures[13].MovingAverageSystolic);
        //    Assert.Equal(195, resultBloodPressures[14].MovingAverageSystolic);

        //    Assert.Null(resultBloodPressures[0].MovingAverageDiastolic);
        //    Assert.Null(resultBloodPressures[1].MovingAverageDiastolic);
        //    Assert.Null(resultBloodPressures[2].MovingAverageDiastolic);
        //    Assert.Equal(90, resultBloodPressures[3].MovingAverageDiastolic);
        //    Assert.Equal(100, resultBloodPressures[4].MovingAverageDiastolic);
        //    Assert.Equal(110, resultBloodPressures[5].MovingAverageDiastolic);
        //    Assert.Equal(120, resultBloodPressures[6].MovingAverageDiastolic);
        //    Assert.Equal(130, resultBloodPressures[7].MovingAverageDiastolic);
        //    Assert.Equal(140, resultBloodPressures[8].MovingAverageDiastolic);
        //    Assert.Equal(150, resultBloodPressures[9].MovingAverageDiastolic);
        //    Assert.Equal(160, resultBloodPressures[10].MovingAverageDiastolic);
        //    Assert.Equal(170, resultBloodPressures[11].MovingAverageDiastolic);
        //    Assert.Equal(180, resultBloodPressures[12].MovingAverageDiastolic);
        //    Assert.Equal(190, resultBloodPressures[13].MovingAverageDiastolic);
        //    Assert.Equal(200, resultBloodPressures[14].MovingAverageDiastolic);

        //}

        //[Fact]
        //public void ShouldSetMovingAveragesOnBloodPressuresWithEmptySeedList()
        //{
        //    var seedBloodPressures = new List<BloodPressure>
        //    {
        //    };


        //    var orderedBloodPressures = new List<BloodPressure>
        //    {
        //        new BloodPressure {Systolic = 100, Diastolic = 105},
        //        new BloodPressure {Systolic = 110, Diastolic = 115},
        //        new BloodPressure {Systolic = 120, Diastolic = 125},
        //        new BloodPressure {Systolic = 130, Diastolic = 135},
        //        new BloodPressure {Systolic = 140, Diastolic = 145},
        //        new BloodPressure {Systolic = 150, Diastolic = 155},
        //        new BloodPressure {Systolic = 160, Diastolic = 165},
        //        new BloodPressure {Systolic = 170, Diastolic = 175},
        //        new BloodPressure {Systolic = 180, Diastolic = 185},
        //        new BloodPressure {Systolic = 190, Diastolic = 195},
        //        new BloodPressure {Systolic = 200, Diastolic = 205},
        //        new BloodPressure {Systolic = 210, Diastolic = 215},
        //        new BloodPressure {Systolic = 220, Diastolic = 225},
        //        new BloodPressure {Systolic = 230, Diastolic = 235},
        //        new BloodPressure {Systolic = 240, Diastolic = 245},
        //    };

        //    var resultBloodPressures = _aggregationCalculator.GetMovingAverages(seedBloodPressures, orderedBloodPressures, 10).ToList();

        //    Assert.Equal(15, resultBloodPressures.Count);

        //    Assert.Equal(100, resultBloodPressures[0].Systolic);
        //    Assert.Equal(110, resultBloodPressures[1].Systolic);
        //    Assert.Equal(120, resultBloodPressures[2].Systolic);
        //    Assert.Equal(130, resultBloodPressures[3].Systolic);
        //    Assert.Equal(140, resultBloodPressures[4].Systolic);
        //    Assert.Equal(150, resultBloodPressures[5].Systolic);
        //    Assert.Equal(160, resultBloodPressures[6].Systolic);
        //    Assert.Equal(170, resultBloodPressures[7].Systolic);
        //    Assert.Equal(180, resultBloodPressures[8].Systolic);
        //    Assert.Equal(190, resultBloodPressures[9].Systolic);
        //    Assert.Equal(200, resultBloodPressures[10].Systolic);
        //    Assert.Equal(210, resultBloodPressures[11].Systolic);
        //    Assert.Equal(220, resultBloodPressures[12].Systolic);
        //    Assert.Equal(230, resultBloodPressures[13].Systolic);
        //    Assert.Equal(240, resultBloodPressures[14].Systolic);

        //    Assert.Equal(105, resultBloodPressures[0].Diastolic);
        //    Assert.Equal(115, resultBloodPressures[1].Diastolic);
        //    Assert.Equal(125, resultBloodPressures[2].Diastolic);
        //    Assert.Equal(135, resultBloodPressures[3].Diastolic);
        //    Assert.Equal(145, resultBloodPressures[4].Diastolic);
        //    Assert.Equal(155, resultBloodPressures[5].Diastolic);
        //    Assert.Equal(165, resultBloodPressures[6].Diastolic);
        //    Assert.Equal(175, resultBloodPressures[7].Diastolic);
        //    Assert.Equal(185, resultBloodPressures[8].Diastolic);
        //    Assert.Equal(195, resultBloodPressures[9].Diastolic);
        //    Assert.Equal(205, resultBloodPressures[10].Diastolic);
        //    Assert.Equal(215, resultBloodPressures[11].Diastolic);
        //    Assert.Equal(225, resultBloodPressures[12].Diastolic);
        //    Assert.Equal(235, resultBloodPressures[13].Diastolic);
        //    Assert.Equal(245, resultBloodPressures[14].Diastolic);


        //    Assert.Null(resultBloodPressures[0].MovingAverageSystolic);
        //    Assert.Null(resultBloodPressures[1].MovingAverageSystolic);
        //    Assert.Null(resultBloodPressures[2].MovingAverageSystolic);
        //    Assert.Null(resultBloodPressures[3].MovingAverageSystolic);
        //    Assert.Null(resultBloodPressures[4].MovingAverageSystolic);
        //    Assert.Null( resultBloodPressures[5].MovingAverageSystolic);
        //    Assert.Null( resultBloodPressures[6].MovingAverageSystolic);
        //    Assert.Null( resultBloodPressures[7].MovingAverageSystolic);
        //    Assert.Null(resultBloodPressures[8].MovingAverageSystolic);
        //    Assert.Equal(145, resultBloodPressures[9].MovingAverageSystolic);
        //    Assert.Equal(155, resultBloodPressures[10].MovingAverageSystolic);
        //    Assert.Equal(165, resultBloodPressures[11].MovingAverageSystolic);
        //    Assert.Equal(175, resultBloodPressures[12].MovingAverageSystolic);
        //    Assert.Equal(185, resultBloodPressures[13].MovingAverageSystolic);
        //    Assert.Equal(195, resultBloodPressures[14].MovingAverageSystolic);

        //    Assert.Null(resultBloodPressures[0].MovingAverageDiastolic);
        //    Assert.Null(resultBloodPressures[1].MovingAverageDiastolic);
        //    Assert.Null(resultBloodPressures[2].MovingAverageDiastolic);
        //    Assert.Null(resultBloodPressures[3].MovingAverageDiastolic);
        //    Assert.Null( resultBloodPressures[4].MovingAverageDiastolic);
        //    Assert.Null( resultBloodPressures[5].MovingAverageDiastolic);
        //    Assert.Null( resultBloodPressures[6].MovingAverageDiastolic);
        //    Assert.Null( resultBloodPressures[7].MovingAverageDiastolic);
        //    Assert.Null(resultBloodPressures[8].MovingAverageDiastolic);
        //    Assert.Equal(150, resultBloodPressures[9].MovingAverageDiastolic);
        //    Assert.Equal(160, resultBloodPressures[10].MovingAverageDiastolic);
        //    Assert.Equal(170, resultBloodPressures[11].MovingAverageDiastolic);
        //    Assert.Equal(180, resultBloodPressures[12].MovingAverageDiastolic);
        //    Assert.Equal(190, resultBloodPressures[13].MovingAverageDiastolic);
        //    Assert.Equal(200, resultBloodPressures[14].MovingAverageDiastolic);

        //}
    }
}