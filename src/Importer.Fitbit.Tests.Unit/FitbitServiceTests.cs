using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Importer.Fitbit.Domain;
using Moq;
using Repositories.Health.Models;
using Xunit;

namespace Importer.Fitbit.Tests.Unit
{
    public class FitbitServiceTests
    {
        private FitbitService _fitbitService;
        private Mock<IFitbitClientQueryAdapter> _fitbitClientQueryAdapter;

        private DateTime fromDate = new DateTime(2017,1,1);
        private DateTime toDate = new DateTime(2018, 2, 2);
        
        
        private Mock<IFitbitAuthenticator> _fitbitAuthenticator;

        public FitbitServiceTests() 
        {
            _fitbitClientQueryAdapter = new Mock<IFitbitClientQueryAdapter>();
            _fitbitAuthenticator = new Mock<IFitbitAuthenticator>();

            _fitbitService = new FitbitService(
                _fitbitClientQueryAdapter.Object, 
                _fitbitAuthenticator.Object);
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
        public async Task ShouldGetRestingHeartRates()
        {
            //Given

            var activitiesHearts = new List<ActivitiesHeart>
            {
                new ActivitiesHeart {dateTime = new DateTime(2018,1,1),value = new Value {restingHeartRate = 51}},
                new ActivitiesHeart {dateTime = new DateTime(2018,1,2),value = new Value {restingHeartRate = 52}},
                new ActivitiesHeart {dateTime = new DateTime(2018,1,3),value = new Value {restingHeartRate = 53}},
                new ActivitiesHeart { dateTime = new DateTime(2018, 1, 4), value = new Value { restingHeartRate = 0 } } /// don't map if 0 restingheartrate
            };

            _fitbitClientQueryAdapter.Setup(x=>x.GetFitbitHeartActivities(fromDate, toDate, It.IsAny<string>())).Returns(Task.FromResult((IEnumerable<ActivitiesHeart>)activitiesHearts));

//            var restingHeartRates = new List<RestingHeartRate>
//            {
//                new RestingHeartRate{CreatedDate = new DateTime(2017,1,1), Beats = 111},
//                new RestingHeartRate{CreatedDate = new DateTime(2017,1,2), Beats = 222},
//                new RestingHeartRate{CreatedDate = new DateTime(2017,1,3), Beats = 333},
//            };

//            _fitbitMapper.Setup(x => x.MapActivitiesHeartsToRestingHeartRates(activitiesHearts)).Returns(restingHeartRates);

            //When
            var result = await _fitbitService.GetRestingHeartRates(fromDate, toDate);

            //Then
            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Beats == 51);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Beats == 52);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Beats == 53);

        }

    }
}