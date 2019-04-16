using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Importer.Fitbit;
using Importer.Fitbit.Domain;
using Moq;
using Utils;
using Xunit;

namespace Services.Tests.Fitbit
{
    public class FitbitClientQueryAdapterTests
    {
        private Mock<IFitbitClient> _fitbitClient;
        private Mock<ILogger> _logger;
        private FitbitClientQueryAdapter _fitbitClientClientQueryAdapter;

        public FitbitClientQueryAdapterTests()
        {
            _fitbitClient = new Mock<IFitbitClient>();
            _logger = new Mock<ILogger>();
            _fitbitClientClientQueryAdapter = new FitbitClientQueryAdapter(_fitbitClient.Object, _logger.Object);
        }

 


        [Fact]
        public async Task ShouldGetFitbitHeartActivities()
        {
            DateTime fromDate = new DateTime(2018, 1, 15);
            DateTime toDate = new DateTime(2018, 3, 15);

            //Given 
            _fitbitClient.Setup(x => x.GetMonthOfFitbitActivities(fromDate.AddMonths(1))).Returns(Task.FromResult(
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

            _fitbitClient.Setup(x => x.GetMonthOfFitbitActivities(fromDate.AddMonths(2))).Returns(Task.FromResult(
                new FitBitActivity
                {
                    activitiesHeart = new List<ActivitiesHeart>
                    {
                        new ActivitiesHeart
                        {
                            dateTime = new DateTime(2018, 2, 20),
                            value = new Value {restingHeartRate = 21}
                        },
                        new ActivitiesHeart
                        {
                            dateTime = new DateTime(2018, 2, 21),
                            value = new Value {restingHeartRate = 22}
                        },
                    }
                }));

            _fitbitClient.Setup(x => x.GetMonthOfFitbitActivities(fromDate.AddMonths(3))).Returns(Task.FromResult(
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
            var heartActivities = await _fitbitClientClientQueryAdapter.GetFitbitHeartActivities(fromDate, toDate);

            Assert.Equal(5, heartActivities.Count());
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 11);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 12);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 21);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 22);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 31);

        }

        [Fact]
        public async Task ShouldReturnFitbitHeartActivitiesAfterReceivingTooManyRequestsException()
        {

            DateTime fromDate = new DateTime(2018, 1, 15);
            DateTime toDate = new DateTime(2018, 3, 15);

            var tooManyRequests = new TooManyRequestsException("I'm a little teapot.");

            //Given 
            _fitbitClient.Setup(x => x.GetMonthOfFitbitActivities(fromDate.AddMonths(1))).Returns(Task.FromResult(
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

            _fitbitClient.Setup(x => x.GetMonthOfFitbitActivities(fromDate.AddMonths(2))).Throws(tooManyRequests);

            _fitbitClient.Setup(x => x.GetMonthOfFitbitActivities(fromDate.AddMonths(3))).Returns(Task.FromResult(
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
            var heartActivities = await _fitbitClientClientQueryAdapter.GetFitbitHeartActivities(fromDate, toDate);

            Assert.Equal(2, heartActivities.Count());
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 11);
            Assert.Contains(heartActivities, x => x.value.restingHeartRate == 12);
            _logger.Verify(x => x.LogErrorAsync(tooManyRequests), Times.Once);

        }

    }
}