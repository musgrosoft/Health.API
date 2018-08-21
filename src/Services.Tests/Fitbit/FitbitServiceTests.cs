using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy.Contributors;
using Moq;
using Repositories.Models;
using Services.Fitbit;
using Services.Fitbit.Domain;
using Utils;
using Xunit;

namespace Services.Tests.Fitbit
{
    public class FitbitServiceTests
    {
        private FitbitService _fitbitService;
        private Mock<ILogger> _logger;
        private Mock<IFitbitClientQueryAdapter> _fitbitClientQueryAdapter;

        private DateTime fromDate = new DateTime(2017,1,1);
        private DateTime toDate = new DateTime(2018, 2, 2);
        private Mock<IFitbitMapper> _fitbitMapper;
        private Mock<IFitbitClient> _fitbitClient;
        private Mock<IFitbitAuthenticator> _fitbitAuthenticator;

        public FitbitServiceTests() 
        {
            _logger = new Mock<ILogger>();
            _fitbitClientQueryAdapter = new Mock<IFitbitClientQueryAdapter>();
            _fitbitMapper = new Mock<IFitbitMapper>();
            _fitbitClient = new Mock<IFitbitClient>();
            _fitbitAuthenticator = new Mock<IFitbitAuthenticator>();

            _fitbitService = new FitbitService(
                _logger.Object, 
                _fitbitClientQueryAdapter.Object, 
                _fitbitClient.Object, 
                _fitbitAuthenticator.Object, 
                _fitbitMapper.Object);
        }

        [Fact]
        public async Task ShouldSubscribe()
        {
            //When
            await _fitbitService.Subscribe();

            //Then
            _fitbitClient.Verify(x=>x.Subscribe(), Times.Once);
        }

        [Fact]
        public async Task ShouldSetTokens()
        {
            //When
            await _fitbitService.SetTokens("I am an authorization code");

            //Then
            _fitbitAuthenticator.Verify(x => x.SetTokens("I am an authorization code"), Times.Once);
        }

        [Fact]
        public async Task ShouldGetStepCounts()
        {
            //Given
            var fitbitDailyActivities = new List<FitbitDailyActivity>
            {
                new FitbitDailyActivity {DateTime = new DateTime(2017, 1, 1)}
            };

            var stepCounts = new List<StepCount>
            {
                new StepCount{CreatedDate = new DateTime(2017, 1, 1), Count = 111},
                new StepCount{CreatedDate = new DateTime(2017, 1, 2), Count = 222},
                new StepCount{CreatedDate = new DateTime(2017, 1, 3), Count = 333}
            };

            _fitbitClientQueryAdapter.Setup(x => x.GetFitbitDailyActivities(fromDate, toDate)).Returns(Task.FromResult((IEnumerable<FitbitDailyActivity>)fitbitDailyActivities));
            _fitbitMapper.Setup(x => x.MapToStepCounts(fitbitDailyActivities)).Returns(stepCounts);

            //When
            var result = await _fitbitService.GetStepCounts(fromDate, toDate);

            //Then
            Assert.Equal(3,result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 1) && x.Count == 111);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 2) && x.Count == 222);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 3) && x.Count == 333);

        }

        [Fact]
        public async Task ShouldGetDailyActivities()
        {
            //Given
            var fitbitDailyActivities = new List<FitbitDailyActivity>
            {
                new FitbitDailyActivity{DateTime = new DateTime(2017,1,1)}
            };

            var activitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2017,1,1), FairlyActiveMinutes = 101, LightlyActiveMinutes = 201, SedentaryMinutes = 301, VeryActiveMinutes = 401},
                new ActivitySummary {CreatedDate = new DateTime(2017,1,2), FairlyActiveMinutes = 102, LightlyActiveMinutes = 202, SedentaryMinutes = 302, VeryActiveMinutes = 402},
                new ActivitySummary {CreatedDate = new DateTime(2017,1,3), FairlyActiveMinutes = 103, LightlyActiveMinutes = 203, SedentaryMinutes = 303, VeryActiveMinutes = 403},
            };

            _fitbitClientQueryAdapter.Setup(x => x.GetFitbitDailyActivities(fromDate, toDate)).Returns(Task.FromResult((IEnumerable<FitbitDailyActivity>)fitbitDailyActivities));
            _fitbitMapper.Setup(x => x.MapFitbitDailyActivitiesToActivitySummaries(fitbitDailyActivities)).Returns(activitySummaries);

            //When
            var result = await _fitbitService.GetActivitySummaries(fromDate, toDate);

            //Then
            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 1) && x.FairlyActiveMinutes == 101 && x.LightlyActiveMinutes == 201 && x.SedentaryMinutes == 301 && x.VeryActiveMinutes == 401);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 2) && x.FairlyActiveMinutes == 102 && x.LightlyActiveMinutes == 202 && x.SedentaryMinutes == 302 && x.VeryActiveMinutes == 402);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 3) && x.FairlyActiveMinutes == 103 && x.LightlyActiveMinutes == 203 && x.SedentaryMinutes == 303 && x.VeryActiveMinutes == 403);

        }

        [Fact]
        public async Task ShouldGetRestingHeartRates()
        {
            //Given
            var activitiesHearts = new List<ActivitiesHeart>
            {
                new ActivitiesHeart{dateTime = new DateTime(2017,1,1)}
            };

            _fitbitClientQueryAdapter.Setup(x=>x.GetFitbitHeartActivities(fromDate, toDate)).Returns(Task.FromResult((IEnumerable<ActivitiesHeart>)activitiesHearts));

            var restingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate{CreatedDate = new DateTime(2017,1,1), Beats = 111},
                new RestingHeartRate{CreatedDate = new DateTime(2017,1,2), Beats = 222},
                new RestingHeartRate{CreatedDate = new DateTime(2017,1,3), Beats = 333},
            };

            _fitbitMapper.Setup(x => x.MapActivitiesHeartToRestingHeartRates(activitiesHearts)).Returns(restingHeartRates);

            //When
            var result = await _fitbitService.GetRestingHeartRates(fromDate, toDate);

            //Then
            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 1) && x.Beats == 111);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 2) && x.Beats == 222);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 3) && x.Beats == 333);

        }

        [Fact]
        public async Task ShouldGetHeartSummaries()
        {
            //Given
            var activitiesHearts = new List<ActivitiesHeart>
            {
                new ActivitiesHeart{dateTime = new DateTime(2017,1,1)}
            };

            _fitbitClientQueryAdapter.Setup(x => x.GetFitbitHeartActivities(fromDate, toDate)).Returns(Task.FromResult((IEnumerable<ActivitiesHeart>)activitiesHearts));

            var heartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary{CreatedDate = new DateTime(2017,1,1), OutOfRangeMinutes = 1, FatBurnMinutes = 2, CardioMinutes = 3, PeakMinutes = 4},
                new HeartRateSummary{CreatedDate = new DateTime(2017,1,2), OutOfRangeMinutes = 5, FatBurnMinutes = 6, CardioMinutes = 7, PeakMinutes = 8},
                new HeartRateSummary{CreatedDate = new DateTime(2017,1,3), OutOfRangeMinutes = 9, FatBurnMinutes = 10, CardioMinutes = 11, PeakMinutes = 12}
            };

            _fitbitMapper.Setup(x => x.MapActivitiesHeartToHeartRateSummaries(activitiesHearts))
                .Returns(heartRateSummaries);

            //When
            var result = await _fitbitService.GetHeartSummaries(fromDate, toDate);

            //Then
            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 1) && x.OutOfRangeMinutes == 1 && x.FatBurnMinutes == 2 && x.CardioMinutes == 3 && x.PeakMinutes == 4);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 2) && x.OutOfRangeMinutes == 5 && x.FatBurnMinutes == 6 && x.CardioMinutes == 7 && x.PeakMinutes == 8);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 3) && x.OutOfRangeMinutes == 9 && x.FatBurnMinutes == 10 && x.CardioMinutes == 11 && x.PeakMinutes == 12);


        }
    }
}