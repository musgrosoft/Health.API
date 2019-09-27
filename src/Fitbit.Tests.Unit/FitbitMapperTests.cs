namespace Fitbit.Tests.Unit
{
    public class FitbitMapperTests
    {
//        private FitbitMapper _fitbitMapper;
//
//        public FitbitMapperTests()
//        {
//            _fitbitMapper = new FitbitMapper();
//        }
//
//        [Fact]
//        public void ShouldMapActivitiesHeartsToRestingHeartRates()
//        {
//            var activitiesHeart = new List<ActivitiesHeart>
//            {
//                new ActivitiesHeart {dateTime = new DateTime(2018,1,1),value = new Value {restingHeartRate = 51}},
//                new ActivitiesHeart {dateTime = new DateTime(2018,1,2),value = new Value {restingHeartRate = 52}},
//                new ActivitiesHeart {dateTime = new DateTime(2018,1,3),value = new Value {restingHeartRate = 53}},
//            };
//
//            var result = _fitbitMapper.MapActivitiesHeartsToRestingHeartRates(activitiesHeart);
//
//            //Then
//            Assert.Equal(3, result.Count());
//            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Beats == 51);
//            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Beats == 52);
//            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Beats == 53);
//
//        }
//
//        [Fact]
//        public void ShouldNotMapActivitiesHeartsThatHaveZeroRestingHeartRate()
//        {
//            var activitiesHeart = new List<ActivitiesHeart>
//            {
//                new ActivitiesHeart {dateTime = new DateTime(2018,1,1),value = new Value {restingHeartRate = 0}}
//            };
//
//            var result = _fitbitMapper.MapActivitiesHeartsToRestingHeartRates(activitiesHeart);
//
//            //Then
//            Assert.Empty(result);
//        }

        //[Fact]
        //public void ShouldMapActivitiesHeartToHeartRateSummaries()
        //{
        //    var activitiesHeart = new List<ActivitiesHeart>
        //    {
        //        new ActivitiesHeart {dateTime = new DateTime(2018,1,1),value = new Value {heartRateZones = new List<HeartRateZone>
        //        {
        //            new HeartRateZone {name = "Out of Range", minutes = 100},
        //            new HeartRateZone {name = "Fat Burn", minutes = 101},
        //            new HeartRateZone {name = "Cardio", minutes = 102},
        //            new HeartRateZone {name = "Peak", minutes = 103},
        //        }}},
        //        new ActivitiesHeart {dateTime = new DateTime(2018,1,2),value = new Value {heartRateZones = new List<HeartRateZone>
        //        {
        //            new HeartRateZone {name = "Out of Range", minutes = 200},
        //            new HeartRateZone {name = "Fat Burn", minutes = 201},
        //            new HeartRateZone {name = "Cardio", minutes = 202},
        //            new HeartRateZone {name = "Peak", minutes = 203},
        //        }}},
        //        new ActivitiesHeart {dateTime = new DateTime(2018,1,3),value = new Value {heartRateZones = new List<HeartRateZone>
        //        {
        //            new HeartRateZone {name = "Out of Range", minutes = 300},
        //            new HeartRateZone {name = "Fat Burn", minutes = 301},
        //            new HeartRateZone {name = "Cardio", minutes = 302},
        //            new HeartRateZone {name = "Peak", minutes = 303},
        //        }}},
        //    };

        //    var result = _fitbitMapper.MapActivitiesHeartsToHeartRateSummaries(activitiesHeart);

        //    //Then
        //    Assert.Equal(3, result.Count());
        //    Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.CardioMinutes == 102 && x.PeakMinutes == 103);
        //    Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.CardioMinutes == 202 && x.PeakMinutes == 203);
        //    Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.CardioMinutes == 302 && x.PeakMinutes == 303);
        //}

        //[Fact]
        //public void ShouldMapFitbitDailyActivitiesToActivitySummaries()
        //{
        //    var fitbitDailyActivities = new List<FitbitDailyActivity>
        //    {
        //        new FitbitDailyActivity {DateTime = new DateTime(2018,1,1), summary = new Summary {fairlyActiveMinutes = 100,lightlyActiveMinutes = 101,sedentaryMinutes = 102,veryActiveMinutes = 103}},
        //        new FitbitDailyActivity {DateTime = new DateTime(2018,1,2), summary = new Summary {fairlyActiveMinutes = 200,lightlyActiveMinutes = 201,sedentaryMinutes = 202,veryActiveMinutes = 203}},
        //        new FitbitDailyActivity {DateTime = new DateTime(2018,1,3), summary = new Summary {fairlyActiveMinutes = 300,lightlyActiveMinutes = 301,sedentaryMinutes = 302,veryActiveMinutes = 303}}

        //    };

        //    var result = _fitbitMapper.MapFitbitDailyActivitiesToActivitySummaries(fitbitDailyActivities);


        //    //Then
        //    Assert.Equal(3, result.Count());
        //    Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.FairlyActiveMinutes == 100 && x.LightlyActiveMinutes == 101 && x.SedentaryMinutes == 102 && x.VeryActiveMinutes == 103);
        //    Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.FairlyActiveMinutes == 200 && x.LightlyActiveMinutes == 201 && x.SedentaryMinutes == 202 && x.VeryActiveMinutes == 203);
        //    Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.FairlyActiveMinutes == 300 && x.LightlyActiveMinutes == 301 && x.SedentaryMinutes == 302 && x.VeryActiveMinutes == 303);
        //}

        //[Fact]
        //public void ShouldMapFromFitbitDailyActivitiesToStepCounts()
        //{
        //    //Given
        //    IEnumerable<FitbitDailyActivity> fitbitDailyActivities = new List<FitbitDailyActivity>
        //    {
        //        new FitbitDailyActivity {DateTime = new DateTime(2018, 1, 1), summary = new Summary {steps = 111}},
        //        new FitbitDailyActivity {DateTime = new DateTime(2018, 1, 2), summary = new Summary {steps = 222}},
        //        new FitbitDailyActivity {DateTime = new DateTime(2018, 1, 3), summary = new Summary {steps = 333}}
        //    };

        //    //When
        //    var result = _fitbitMapper.MapFitbitDailyActivitiesToStepCounts(fitbitDailyActivities);

        //    //Then
        //    Assert.Equal(3,result.Count());
        //    Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Count == 111);
        //    Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Count == 222);
        //    Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Count == 333);

        //}

        //public void ShouldMapFromActivitiesHeartToRestingHeartRates()
        //{

        //}

    }
}