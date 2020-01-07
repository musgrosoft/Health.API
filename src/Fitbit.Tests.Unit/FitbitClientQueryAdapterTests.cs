using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitbit.Domain;
using Moq;
using Utils;
using Xunit;

namespace Fitbit.Tests.Unit
{
    public class FitbitClientQueryAdapterTests
    {
        private Mock<IFitbitClient> _fitbitClient;
        private Mock<ILogger> _logger;
        private FitbitClientQueryAdapter _fitbitClientClientQueryAdapter;
        
        private string _accessToken;

        public FitbitClientQueryAdapterTests()
        {
            _fitbitClient = new Mock<IFitbitClient>();
            _logger = new Mock<ILogger>();
            _fitbitClientClientQueryAdapter = new FitbitClientQueryAdapter(_fitbitClient.Object, _logger.Object);

            _accessToken = "lksfdh039174kjdfh238947";
        }

        [Fact]
        public async Task ShouldGetFitbitHeartActivities()
        {
            DateTime _fromDate = new DateTime(2018, 1, 15);
            DateTime _toDate = new DateTime(2018, 3, 15);

            //Given 
            _fitbitClient.Setup(x => x.GetMonthOfFitbitHeartRates(_fromDate.AddMonths(1), _accessToken)).Returns(Task.FromResult(
                new FitBitActivity
                {
                    activitiesHeart = new List<ActivitiesHeart>
                    {
                        new ActivitiesHeart { dateTime = new DateTime(2018, 1, 20), value = new Value {restingHeartRate = 11} },
                        new ActivitiesHeart { dateTime = new DateTime(2018, 1, 21), value = new Value {restingHeartRate = 12} }
                    }
                }));

            _fitbitClient.Setup(x => x.GetMonthOfFitbitHeartRates(_fromDate.AddMonths(2), _accessToken)).Returns(Task.FromResult(
                new FitBitActivity
                {
                    activitiesHeart = new List<ActivitiesHeart>
                    {
                        new ActivitiesHeart { dateTime = new DateTime(2018, 2, 20), value = new Value {restingHeartRate = 21} },
                        new ActivitiesHeart { dateTime = new DateTime(2018, 2, 21), value = new Value {restingHeartRate = 22} }
                    }
                }));

            _fitbitClient.Setup(x => x.GetMonthOfFitbitHeartRates(_fromDate.AddMonths(3), _accessToken)).Returns(Task.FromResult(
                new FitBitActivity
                {
                    activitiesHeart = new List<ActivitiesHeart>
                    {
                        new ActivitiesHeart { dateTime = new DateTime(2018, 3, 14), value = new Value {restingHeartRate = 31} },
                        new ActivitiesHeart { dateTime = new DateTime(2018, 3, 16), value = new Value {restingHeartRate = 32} }
                    }
                }));

            //When
            var heartActivities = await _fitbitClientClientQueryAdapter.GetFitbitHeartActivities(_fromDate, _toDate, It.IsAny<string>());

            Assert.Equal(5, heartActivities.Count());
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 11);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 12);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 21);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 22);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 31);

        }


        [Fact]
        public async Task ShouldGetSleeps()
        {
            DateTime _fromDate = new DateTime(2018, 1, 15);
            // more than 300 days DateTime _toDate = new DateTime(2018, 3, 15);

            //Given 
            _fitbitClient.Setup(x => x.Get100DaysOfSleeps(_fromDate, _accessToken)).Returns(Task.FromResult(
                new FitbitSleepsResponse
                {
                    sleep = new List<Sleep>
                    {
                        new Sleep{dateOfSleep = new DateTime(2001,1,1), minutesAsleep = 1},
                        new Sleep{dateOfSleep = new DateTime(2001,1,2), minutesAsleep = 2},
                    }
                }));

            _fitbitClient.Setup(x => x.Get100DaysOfSleeps(_fromDate.AddDays(100), _accessToken)).Returns(Task.FromResult(
                new FitbitSleepsResponse
                {
                    sleep = new List<Sleep>
                    {
                        new Sleep{dateOfSleep = new DateTime(2001,1,3), minutesAsleep = 3},
                        new Sleep{dateOfSleep = new DateTime(2001,1,4), minutesAsleep = 4},
                    }
                }));

            _fitbitClient.Setup(x => x.Get100DaysOfSleeps(_fromDate.AddDays(200), _accessToken)).Returns(Task.FromResult(
                new FitbitSleepsResponse
                {
                    sleep = new List<Sleep>
                    {
                        new Sleep{dateOfSleep = new DateTime(2001,1,5), minutesAsleep = 5},
                        new Sleep{dateOfSleep = new DateTime(2001,1,6), minutesAsleep = 6},
                    }
                }));

            Assert.True(false);
        }


        [Fact]
        public async Task ShouldReturnFitbitHeartActivitiesAfterReceivingTooManyRequestsException()
        {

            DateTime fromDate = new DateTime(2018, 1, 15);
            DateTime toDate = new DateTime(2018, 3, 15);

            var tooManyRequests = new TooManyRequestsException("I'm a little teapot.");

            //Given 
            _fitbitClient.Setup(x => x.GetMonthOfFitbitHeartRates(fromDate.AddMonths(1), It.IsAny<string>())).Returns(Task.FromResult(
                new FitBitActivity
                {
                    activitiesHeart = new List<ActivitiesHeart>
                    {
                        new ActivitiesHeart
                        {
                            dateTime = new DateTime(2018, 1, 20),
                            value = new Value {restingHeartRate = 11}
                        },
                        new ActivitiesHeart
                        {
                            dateTime = new DateTime(2018, 1, 21),
                            value = new Value {restingHeartRate = 12}
                        },
                    }
                }));

            _fitbitClient.Setup(x => x.GetMonthOfFitbitHeartRates(fromDate.AddMonths(2), It.IsAny<string>())).Throws(tooManyRequests);

            _fitbitClient.Setup(x => x.GetMonthOfFitbitHeartRates(fromDate.AddMonths(3), It.IsAny<string>())).Returns(Task.FromResult(
                new FitBitActivity
                {
                    activitiesHeart = new List<ActivitiesHeart>
                    {
                        new ActivitiesHeart
                        {
                            dateTime = new DateTime(2018, 3, 14),
                            value = new Value {restingHeartRate = 31}
                        },
                        new ActivitiesHeart
                        {
                            dateTime = new DateTime(2018, 3, 16),
                            value = new Value {restingHeartRate = 32}
                        },
                    }
                }));

            //When
            var heartActivities = await _fitbitClientClientQueryAdapter.GetFitbitHeartActivities(fromDate, toDate, It.IsAny<string>());

            Assert.Equal(2, heartActivities.Count());
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 11);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 12);
            _logger.Verify(x => x.LogErrorAsync(tooManyRequests), Times.Once);

        }




    }
}