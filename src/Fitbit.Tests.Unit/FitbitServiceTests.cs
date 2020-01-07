using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitbit.Domain;
using Moq;
using Repositories.Health.Models;
using Xunit;

namespace Fitbit.Tests.Unit
{
    public class FitbitServiceTests
    {
        private FitbitService _fitbitService;
        private Mock<IFitbitClientQueryAdapter> _fitbitClientQueryAdapter;

        private DateTime fromDate = new DateTime(2017,1,1);
        private DateTime toDate = new DateTime(2018, 2, 2);
        
        
        private Mock<IFitbitAuthenticator> _fitbitAuthenticator;
        private Mock<IFitbitMapper> _fitbitMapper;
        private readonly string _authToken;

        public FitbitServiceTests() 
        {
            _fitbitClientQueryAdapter = new Mock<IFitbitClientQueryAdapter>();
            _fitbitAuthenticator = new Mock<IFitbitAuthenticator>();
            _fitbitMapper = new Mock<IFitbitMapper>();

            _authToken = "qwe987wqe987";
            _fitbitAuthenticator
                .Setup(x => x.GetAccessToken())
                .Returns(Task.FromResult(_authToken));

            _fitbitService = new FitbitService(
                _fitbitClientQueryAdapter.Object, 
                _fitbitAuthenticator.Object,
                _fitbitMapper.Object);
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
        public async Task ShouldGetSleepSummaries()
        {
            IEnumerable<Sleep> sleeps = new List<Sleep>
            {
                new Sleep {dateOfSleep = new DateTime(2001,2,3), minutesAsleep = 345}
            };

            _fitbitClientQueryAdapter
                .Setup(x => x.GetFitbitSleeps(fromDate, toDate, _authToken))
                .Returns(Task.FromResult(sleeps));
            
            IEnumerable<SleepSummary> sleepSummaries = new List<SleepSummary> { new SleepSummary {DateOfSleep = new DateTime(2022, 10, 21)} };
            
            _fitbitMapper
                .Setup(x => x.MapFitbitSleepsToSleepSummaries(sleeps))
                .Returns(sleepSummaries);

            //When
            var result = await _fitbitService.GetSleepSummaries(fromDate, toDate);

            //Then
            Assert.Equal(sleepSummaries, result);
        }

        [Fact]
        public async Task ShouldGetRestingHeartRates()
        {
            //Given
            IEnumerable<ActivitiesHeart> activitiesHearts = new List<ActivitiesHeart> { new ActivitiesHeart {dateTime = new DateTime(2018,1,1),value = new Value {restingHeartRate = 51}} };

            _fitbitClientQueryAdapter
                .Setup(x => x.GetFitbitHeartActivities(fromDate, toDate,_authToken))
                .Returns(Task.FromResult(activitiesHearts));

            var restingHeartRates = new List<RestingHeartRate>{ new RestingHeartRate{CreatedDate = new DateTime(2017,1,1), Beats = 111} };

            _fitbitMapper
                .Setup(x => x.MapActivitiesHeartsToRestingHeartRates(activitiesHearts))
                .Returns(restingHeartRates);

            //When
            var result = await _fitbitService.GetRestingHeartRates(fromDate, toDate);

            //Then
            Assert.Equal(restingHeartRates, result);
//            Assert.Equal(3, result.Count());
//            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 1) && x.Beats == 111);
//            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 2) && x.Beats == 222);
//            Assert.Contains(result, x => x.CreatedDate == new DateTime(2017, 1, 3) && x.Beats == 333);

        }


    }
}