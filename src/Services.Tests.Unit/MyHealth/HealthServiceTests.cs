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

        //[Fact]
        //public void ShouldUpsertHeartSummaries()
        //{
        //    //Given
        //    var newHeartSummaries = new List<HeartRateSummary>
        //    {
        //        new HeartRateSummary {CreatedDate = new DateTime(2016,1,1), CardioMinutes = 2016},
        //        new HeartRateSummary {CreatedDate = new DateTime(2017,1,1), CardioMinutes = 2017},
        //        new HeartRateSummary {CreatedDate = new DateTime(2018,1,1), CardioMinutes = 2018}
        //    };

        //    //When
        //    _healthService.UpsertHeartSummaries(newHeartSummaries);

        //    //Then
        //    _healthRepository.Verify(x => x.Upsert(newHeartSummaries[0]), Times.Once);
        //    _healthRepository.Verify(x => x.Upsert(newHeartSummaries[1]), Times.Once);
        //    _healthRepository.Verify(x => x.Upsert(newHeartSummaries[2]), Times.Once);
        //}

        //[Fact]
        //public void ShouldGetAllRestingHeartRates()
        //{
        //    //Given
        //    var restingHeartRates = new List<RestingHeartRate>
        //    {
        //        new RestingHeartRate {CreatedDate = new DateTime(2018,6,6), Beats = 123}
        //    };

        //    _entityDecorator.Setup(x => x.GetAllRestingHeartRates()).Returns(restingHeartRates);

        //    //when
        //    var result = _healthService.GetAllRestingHeartRates();

        //    //then
        //    Assert.Equal(restingHeartRates, result);
        //}


        //[Fact]
        //public void ShouldGetAllBloodPressures(){

        //    //Given
        //    var bloodPressures = new List<BloodPressure>
        //    {
        //        new BloodPressure {CreatedDate = new DateTime(2018,6,6), Systolic = 123}
        //    };

        //    _entityDecorator.Setup(x => x.GetAllBloodPressures()).Returns(bloodPressures);

        //    //when
        //    var result = _healthService.GetAllBloodPressures();

        //    //then
        //    Assert.Equal(bloodPressures, result);

        //}

        //[Fact]
        //public void ShouldGetAllRuns()
        //{
        //    //Given
        //    var runs = new List<Run>
        //    {
        //        new Run {CreatedDate = new DateTime(2018,6,6), Metres = 123}
        //    };

        //    _healthRepository.Setup(x => x.GetAllRuns()).Returns(runs);

        //    //when
        //    var result = _healthService.GetAllRuns();

        //    //then
        //    Assert.Equal(runs, result);

        //}

        //[Fact]
        //public void ShouldGetAllErgos()
        //{
        //    //Given
        //    var ergos = new List<Ergo>
        //    {
        //        new Ergo {CreatedDate = new DateTime(2018,6,6), Metres = 123}
        //    };

        //    _healthRepository.Setup(x => x.GetAllErgos()).Returns(ergos);

        //    //when
        //    var result = _healthService.GetAllErgos();

        //    //then
        //    Assert.Equal(ergos, result);

        //}


    }
}