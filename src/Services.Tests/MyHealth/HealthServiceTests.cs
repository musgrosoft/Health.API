using System;
using System.Collections.Generic;
using Moq;
using Repositories.Health;
using Repositories.Health.Models;
using Services.Health;
using Utils;
using Xunit;

namespace Services.Tests.MyHealth
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
            var date = new DateTime(2018, 1, 2);
            _healthRepository.Setup(x => x.GetLatestWeightDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestWeightDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);
        }

        [Fact]
        public void ShouldGetLatestDrinkDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 2);
            _healthRepository.Setup(x => x.GetLatestDrinkDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestDrinkDate();

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
            var date = new DateTime(2018, 1, 6);
            _healthRepository.Setup(x => x.GetLatestRestingHeartRateDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestRestingHeartRateDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);

        }
        
        [Fact]
        public void ShouldUpsertNewWeights()
        {
            //Given
            var newWeights = new List<Weight>
            {
                new Weight { CreatedDate = new DateTime(2010,10,10) },
                new Weight { CreatedDate = new DateTime(2010,10,11) },
                new Weight { CreatedDate = new DateTime(2010,10,12) }

            };

            //When
            _healthService.UpsertWeights(newWeights);

            //Then
            _healthRepository.Verify(x => x.Upsert(newWeights[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newWeights[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newWeights[2]), Times.Once);
        }


        [Fact]
        public void ShouldUpsertNewExercises()
        {
            //Given
            var newExercises = new List<Exercise>
            {
                new Exercise { CreatedDate = new DateTime(2010,10,10) },
                new Exercise { CreatedDate = new DateTime(2010,10,11) },
                new Exercise { CreatedDate = new DateTime(2010,10,12) }

            };

            //When
            _healthService.UpsertExercises(newExercises);

            //Then
            _healthRepository.Verify(x => x.Upsert(newExercises[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newExercises[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newExercises[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewAlcoholIntakes()
        {
            //Given
            var newAlcoholIntake = new List<Drink>
            {
                new Drink { CreatedDate = new DateTime(2010,10,10) },
                new Drink { CreatedDate = new DateTime(2010,10,11) },
                new Drink { CreatedDate = new DateTime(2010,10,12) }

            };

            //When
            _healthService.UpsertAlcoholIntakes(newAlcoholIntake);

            //Then
            _healthRepository.Verify(x => x.Upsert(newAlcoholIntake[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newAlcoholIntake[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newAlcoholIntake[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewBloodPressures()
        {
            //Given
            var newBloodPressures = new List<BloodPressure>
            {
                new BloodPressure { CreatedDate = new DateTime(2010,10,10) },
                new BloodPressure { CreatedDate = new DateTime(2010,10,11) },
                new BloodPressure { CreatedDate = new DateTime(2010,10,12) }
            };
            
            //When
            _healthService.UpsertBloodpressures(newBloodPressures);

            //Then
            _healthRepository.Verify(x => x.Upsert(newBloodPressures[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newBloodPressures[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newBloodPressures[2]), Times.Once);
        }

        //[Fact]
        //public void ShouldUpsertNewStepCounts()
        //{
        //    //Given
        //    var newStepCounts = new List<StepCount>
        //    {
        //        new StepCount {CreatedDate = new DateTime(2016,1,1), Count = 2016},
        //        new StepCount {CreatedDate = new DateTime(2017,1,1), Count = 2017},
        //        new StepCount {CreatedDate = new DateTime(2018,1,1), Count = 2018}
        //    };

        //    //When
        //    _healthService.UpsertStepCounts(newStepCounts);

        //    //Then
        //    _healthRepository.Verify(x => x.Upsert(newStepCounts[0]), Times.Once);
        //    _healthRepository.Verify(x => x.Upsert(newStepCounts[1]), Times.Once);
        //    _healthRepository.Verify(x => x.Upsert(newStepCounts[2]), Times.Once);
        //}

        //[Fact]
        //public void ShouldUpsertNewActivitySummaries()
        //{
        //    //Given
        //    var newActivitySummaries = new List<ActivitySummary>
        //    {
        //        new ActivitySummary {CreatedDate = new DateTime(2016,1,1), FairlyActiveMinutes = 2016},
        //        new ActivitySummary {CreatedDate = new DateTime(2017,1,1), FairlyActiveMinutes = 2017},
        //        new ActivitySummary {CreatedDate = new DateTime(2018,1,1), FairlyActiveMinutes = 2018}
        //    };
            
        //    //When
        //    _healthService.UpsertActivitySummaries(newActivitySummaries);

        //    //Then
        //    _healthRepository.Verify(x => x.Upsert(newActivitySummaries[0]), Times.Once);
        //    _healthRepository.Verify(x => x.Upsert(newActivitySummaries[1]), Times.Once);
        //    _healthRepository.Verify(x => x.Upsert(newActivitySummaries[2]), Times.Once);
        //}
        
        [Fact]
        public void ShouldUpsertNewRestingHeartRates()
        {
            //Given
            var newHeartSummaries = new List<RestingHeartRate>
            {
                new RestingHeartRate { CreatedDate = new DateTime(2010,10,10) },
                new RestingHeartRate { CreatedDate = new DateTime(2010,10,11) },
                new RestingHeartRate { CreatedDate = new DateTime(2010,10,12) }
            };
            
            //When
            _healthService.UpsertRestingHeartRates(newHeartSummaries);

            //Then
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[2]), Times.Once);

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