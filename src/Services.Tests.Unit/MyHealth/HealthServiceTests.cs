using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Repositories.Health;
using Repositories.Health.Models;
using Services.Health;
using Utils;
using Xunit;

namespace Services.Tests.Unit.MyHealth
{
    public class HealthServiceTests
    {
        private Mock<IHealthRepository> _healthRepository;
        private HealthService _healthService;
        private Mock<ILogger> _logger;

        public HealthServiceTests()
        {
            _healthRepository = new Mock<IHealthRepository>();
            _logger = new Mock<ILogger>();

            _healthService = new HealthService(
                _logger.Object, 
                _healthRepository.Object
                );
        }

        [Fact]
        public void ShouldGetLatestWeightDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 1);
            _healthRepository.Setup(x => x.GetLatestWeightDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestWeightDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);
        }

        [Fact]
        public void ShouldGetLatestTargetDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 2);
            _healthRepository.Setup(x => x.GetLatestTargetDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestTargetDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);
        }

        [Fact]
        public void ShouldGetLatestDrinkDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 3);
            _healthRepository.Setup(x => x.GetLatestDrinkDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestDrinkDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);
        }

        [Fact]
        public void ShouldGetLatestBloodPressureDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 4);
            _healthRepository.Setup(x => x.GetLatestBloodPressureDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestBloodPressureDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);

        }
        
        [Fact]
        public void ShouldGetLatestRestingHeartRateDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 5);
            _healthRepository.Setup(x => x.GetLatestRestingHeartRateDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestRestingHeartRateDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);

        }

        [Fact]
        public void ShouldGetLatestExerciseDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 6);
            _healthRepository.Setup(x => x.GetLatestExerciseDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestExerciseDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);
        }

        [Fact]
        public void ShouldGetLatestSleepStateDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 6);
            _healthRepository.Setup(x => x.GetLatestSleepStateDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestSleepStateDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);
        }

        [Fact]
        public void ShouldGetLatestSleepSummaryDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 6);
            _healthRepository.Setup(x => x.GetLatestSleepSummaryDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestSleepSummaryDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);
        }

        [Fact]
        public async Task ShouldUpsertNewWeights()
        {
            //Given
            var newWeights = new List<Weight>
            {
                new Weight { CreatedDate = new DateTime(2010,10,10) },
                new Weight { CreatedDate = new DateTime(2010,10,11) },
                new Weight { CreatedDate = new DateTime(2010,10,12) }
            };

            //When
            await _healthService.UpsertAsync(newWeights);

            //Then
            _healthRepository.Verify(x => x.UpsertAsync(newWeights), Times.Once);
        }


        [Fact]
        public async Task ShouldUpsertNewExercises()
        {
            //Given
            var newExercises = new List<Exercise>
            {
                new Exercise { CreatedDate = new DateTime(2010,10,10) },
                new Exercise { CreatedDate = new DateTime(2010,10,11) },
                new Exercise { CreatedDate = new DateTime(2010,10,12) }

            };

            //When
            await _healthService.UpsertAsync(newExercises);

            //Then
            _healthRepository.Verify(x => x.UpsertAsync(newExercises), Times.Once);
        }

        [Fact]
        public async Task ShouldUpsertNewAlcoholIntakes()
        {
            //Given
            var newDrinks = new List<Drink>
            {
                new Drink { CreatedDate = new DateTime(2010,10,10) },
                new Drink { CreatedDate = new DateTime(2010,10,11) },
                new Drink { CreatedDate = new DateTime(2010,10,12) }

            };

            //When
            await _healthService.UpsertAsync(newDrinks);

            //Then
            _healthRepository.Verify(x => x.UpsertAsync(newDrinks), Times.Once);
        }

        [Fact]
        public async Task ShouldUpsertNewBloodPressures()
        {
            //Given
            var newBloodPressures = new List<BloodPressure>
            {
                new BloodPressure { CreatedDate = new DateTime(2010,10,10) },
                new BloodPressure { CreatedDate = new DateTime(2010,10,11) },
                new BloodPressure { CreatedDate = new DateTime(2010,10,12) }
            };

            //When
            await _healthService.UpsertAsync(newBloodPressures);

            //Then
            _healthRepository.Verify(x => x.UpsertAsync(newBloodPressures), Times.Once);
        }
        
        [Fact]
        public async Task ShouldUpsertNewRestingHeartRates()
        {
            //Given
            var newHeartSummaries = new List<RestingHeartRate>
            {
                new RestingHeartRate { CreatedDate = new DateTime(2010,10,10) },
                new RestingHeartRate { CreatedDate = new DateTime(2010,10,11) },
                new RestingHeartRate { CreatedDate = new DateTime(2010,10,12) }
            };

            //When
            await _healthService.UpsertAsync(newHeartSummaries);

            //Then
            _healthRepository.Verify(x => x.UpsertAsync(newHeartSummaries), Times.Once);
        }

        [Fact]
        public async Task ShouldUpsertNewSleepSummaries()
        {
            //Given
            var newSleepSummaries = new List<SleepSummary>
            {
                new SleepSummary { DateOfSleep = new DateTime(2010,10,10) },
                new SleepSummary { DateOfSleep = new DateTime(2010,10,11) },
                new SleepSummary { DateOfSleep = new DateTime(2010,10,12) }
            };

            //When
            await _healthService.UpsertAsync(newSleepSummaries);

            //Then
            _healthRepository.Verify(x => x.UpsertAsync(newSleepSummaries), Times.Once);
        }

        [Fact]
        public async Task ShouldUpsertNewSleepStates()
        {
            //Given
            var newSleepStates = new List<SleepState>
            {
                new SleepState { CreatedDate = new DateTime(2010,10,10) },
                new SleepState { CreatedDate = new DateTime(2010,10,11) },
                new SleepState { CreatedDate = new DateTime(2010,10,12) }
            };

            //When
            await _healthService.UpsertAsync(newSleepStates);

            //Then
            _healthRepository.Verify(x => x.UpsertAsync(newSleepStates), Times.Once);
        }

        [Fact]
        public async Task ShouldUpsertNewTargets()
        {
            //Given
            var newTargets = new List<Target>
            {
                new Target { Date = new DateTime(2010,10,10) },
                new Target { Date = new DateTime(2010,10,11) },
                new Target { Date = new DateTime(2010,10,12) }
            };

            //When
            await _healthService.UpsertAsync(newTargets);

            //Then
            _healthRepository.Verify(x => x.UpsertAsync(newTargets), Times.Once);
        }


    }
}