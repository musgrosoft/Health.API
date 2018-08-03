using System;
using System.Collections.Generic;
using Moq;
using Repositories.Health;
using Repositories.Models;
using Services.Health;
using Utils;
using Xunit;

namespace Services.Tests.MyHealth
{
    public class HealthServiceTests
    {
        private Mock<IHealthRepository> _healthRepository;
        private HealthService _healthService;
        private Mock<IConfig> _config;
        private Mock<ILogger> _logger;

        private Mock<IAggregateStatisticsCalculator> _aggregationCalculator;
        private Mock<ITargetService> _targetService;
        private Mock<IEntityDecorator> _entityDecorator;
        private Mock<IEntityAggregator> _entityAggregator;


        public HealthServiceTests()
        {
            _healthRepository = new Mock<IHealthRepository>();
            _config = new Mock<IConfig>();
            _logger = new Mock<ILogger>();
            _aggregationCalculator = new Mock<IAggregateStatisticsCalculator>();
            _targetService = new Mock<ITargetService>();

            _entityDecorator = new Mock<IEntityDecorator>();
            _entityAggregator = new Mock<IEntityAggregator>();

            _healthService = new HealthService(
                _config.Object, 
                _logger.Object, 
                _healthRepository.Object, 
                _entityAggregator.Object, 
                _entityDecorator.Object);
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
        public void ShouldGetLatestStepCountDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 4);
            _healthRepository.Setup(x => x.GetLatestStepCountDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestStepCountDate(DateTime.MinValue);

            //then
            Assert.Equal(date, latestDate);

        }

        [Fact]
        public void ShouldGetLatestActivitySummaryDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 5);
            _healthRepository.Setup(x => x.GetLatestActivitySummaryDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestActivitySummaryDate(DateTime.MinValue);

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
        public void ShouldGetLatestHeartSummaryDate()
        {
            //Given 
            var date = new DateTime(2018, 1, 7);
            _healthRepository.Setup(x => x.GetLatestHeartSummaryDate()).Returns(date);

            //when
            var latestDate = _healthService.GetLatestHeartSummaryDate(DateTime.MinValue);

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
        public void ShouldUpsertNewRuns()
        {
            //Given
            var newRuns = new List<Run>
            {
                new Run { CreatedDate = new DateTime(2010,10,10) },
                new Run { CreatedDate = new DateTime(2010,10,11) },
                new Run { CreatedDate = new DateTime(2010,10,12) }

            };

            //When
            _healthService.UpsertRuns(newRuns);

            //Then
            _healthRepository.Verify(x => x.Upsert(newRuns[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newRuns[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newRuns[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewErgos()
        {
            //Given
            var newErgos = new List<Ergo>
            {
                new Ergo { CreatedDate = new DateTime(2010,10,10) },
                new Ergo { CreatedDate = new DateTime(2010,10,11) },
                new Ergo { CreatedDate = new DateTime(2010,10,12) }

            };

            //When
            _healthService.UpsertErgos(newErgos);

            //Then
            _healthRepository.Verify(x => x.Upsert(newErgos[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newErgos[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newErgos[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewAlcoholIntakes()
        {
            //Given
            var newAlcoholIntake = new List<AlcoholIntake>
            {
                new AlcoholIntake { CreatedDate = new DateTime(2010,10,10) },
                new AlcoholIntake { CreatedDate = new DateTime(2010,10,11) },
                new AlcoholIntake { CreatedDate = new DateTime(2010,10,12) }

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
            _healthService.UpsertBloodPressures(newBloodPressures);

            //Then
            _healthRepository.Verify(x => x.Upsert(newBloodPressures[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newBloodPressures[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newBloodPressures[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewStepCounts()
        {
            //Given
            var newStepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2016,1,1), Count = 2016},
                new StepCount {CreatedDate = new DateTime(2017,1,1), Count = 2017},
                new StepCount {CreatedDate = new DateTime(2018,1,1), Count = 2018}
            };

            //When
            _healthService.UpsertStepCounts(newStepCounts);

            //Then
            _healthRepository.Verify(x => x.Upsert(newStepCounts[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newStepCounts[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newStepCounts[2]), Times.Once);
        }

        [Fact]
        public void ShouldUpsertNewActivitySummaries()
        {
            //Given
            var newActivitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2016,1,1), FairlyActiveMinutes = 2016},
                new ActivitySummary {CreatedDate = new DateTime(2017,1,1), FairlyActiveMinutes = 2017},
                new ActivitySummary {CreatedDate = new DateTime(2018,1,1), FairlyActiveMinutes = 2018}
            };
            
            //When
            _healthService.UpsertActivitySummaries(newActivitySummaries);

            //Then
            _healthRepository.Verify(x => x.Upsert(newActivitySummaries[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newActivitySummaries[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newActivitySummaries[2]), Times.Once);
        }
        
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
        
        [Fact]
        public void ShouldUpsertHeartSummaries()
        {
            //Given
            var newHeartSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2016,1,1), FatBurnMinutes = 2016},
                new HeartRateSummary {CreatedDate = new DateTime(2017,1,1), FatBurnMinutes = 2017},
                new HeartRateSummary {CreatedDate = new DateTime(2018,1,1), FatBurnMinutes = 2018}
            };

            //When
            _healthService.UpsertHeartSummaries(newHeartSummaries);

            //Then
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[0]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[1]), Times.Once);
            _healthRepository.Verify(x => x.Upsert(newHeartSummaries[2]), Times.Once);
        }

        [Fact]
        public void ShouldGetAllRestingHeartRates()
        {
            //Given
            var restingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate {CreatedDate = new DateTime(2018,6,6), Beats = 123}
            };
            
            _entityDecorator.Setup(x => x.GetAllRestingHeartRates()).Returns(restingHeartRates);
            
            //when
            var result = _healthService.GetAllRestingHeartRates();

            //then
            Assert.Equal(restingHeartRates, result);
        }


        [Fact]
        public void ShouldGetAllBloodPressures(){

            //Given
            var bloodPressures = new List<BloodPressure>
            {
                new BloodPressure {CreatedDate = new DateTime(2018,6,6), Systolic = 123}
            };

            _entityDecorator.Setup(x => x.GetAllBloodPressures()).Returns(bloodPressures);

            //when
            var result = _healthService.GetAllBloodPressures();

            //then
            Assert.Equal(bloodPressures, result);

        }

        [Fact]
        public void ShouldGetAllRuns()
        {
            //Given
            var runs = new List<Run>
            {
                new Run {CreatedDate = new DateTime(2018,6,6), Metres = 123}
            };

            _healthRepository.Setup(x => x.GetAllRuns()).Returns(runs);

            //when
            var result = _healthService.GetAllRuns();

            //then
            Assert.Equal(runs, result);

        }

        [Fact]
        public void ShouldGetAllErgos()
        {
            //Given
            var ergos = new List<Ergo>
            {
                new Ergo {CreatedDate = new DateTime(2018,6,6), Metres = 123}
            };

            _healthRepository.Setup(x => x.GetAllErgos()).Returns(ergos);

            //when
            var result = _healthService.GetAllErgos();

            //then
            Assert.Equal(ergos, result);

        }

        [Fact]
        public void ShouldGetAllWeights()
        {
            //Given
            var weights = new List<Weight>
            {
                new Weight {CreatedDate = new DateTime(2018,6,6), Kg = 123}
            };

            _entityDecorator.Setup(x => x.GetAllWeights()).Returns(weights);

            //when
            var result = _healthService.GetAllWeights();

            //then
            Assert.Equal(weights, result);

        }

        [Fact]
        public void ShouldGetAllStepCounts()
        {

            //Given
            var stepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2018,6,6), Count = 123}
            };

            _entityDecorator.Setup(x => x.GetAllStepCounts()).Returns(stepCounts);

            //when
            var result = _healthService.GetAllStepCounts();

            //then
            Assert.Equal(stepCounts, result);

        }

        [Fact]
        public void ShouldGetAllStepCountsByWeek()
        {
            //Given
            var stepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2018,6,6), Count = 123}
            };

            var weeklyStepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2017,7,7), Count = 777}
            };

            _entityDecorator.Setup(x => x.GetAllStepCounts()).Returns(stepCounts);
            _entityAggregator.Setup(x => x.GroupByWeek(stepCounts)).Returns(weeklyStepCounts);

            //when
            var result = _healthService.GetAllStepCountsByWeek();

            //then
            Assert.Equal(weeklyStepCounts, result);
        }

        [Fact]
        public void ShouldGetAllStepCountsByMonth()
        {
            //Given
            var stepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2018,6,6), Count = 123}
            };

            var monthlyStepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2017,7,7), Count = 777}
            };

            _entityDecorator.Setup(x => x.GetAllStepCounts()).Returns(stepCounts);
            _entityAggregator.Setup(x => x.GroupByMonth(stepCounts)).Returns(monthlyStepCounts);

            //when
            var result = _healthService.GetAllStepCountsByMonth();

            //then
            Assert.Equal(monthlyStepCounts, result);
        }

        [Fact]
        public void ShouldGetAllAlcoholIntakes()
        {

            //Given
            var alcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake {CreatedDate = new DateTime(2018,6,6), Units = 123}
            };
            
            _entityDecorator.Setup(x => x.GetAllAlcoholIntakes()).Returns(alcoholIntakes);

            //when
            var result = _healthService.GetAllAlcoholIntakes();

            //then
            Assert.Equal(alcoholIntakes, result);

        }

        [Fact]
        public void ShouldGetAllAlcoholIntakesByWeek()
        {

            //Given
            var alcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake {CreatedDate = new DateTime(2018,6,6), Units = 123}
            };

            var weeklyAlcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,7), Units = 127}
            };

            _entityDecorator.Setup(x => x.GetAllAlcoholIntakes()).Returns(alcoholIntakes);
            _entityAggregator.Setup(x => x.GroupByWeek(alcoholIntakes)).Returns(weeklyAlcoholIntakes);

            //when
            var result = _healthService.GetAllAlcoholIntakesByWeek();

            //then
            Assert.Equal(weeklyAlcoholIntakes, result);

        }

        [Fact]
        public void ShouldGetAllAlcoholIntakesByMonth()
        {

            //Given
            var alcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake {CreatedDate = new DateTime(2018,6,6), Units = 123}
            };

            var monthlyAlcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,7), Units = 127}
            };

            _entityDecorator.Setup(x => x.GetAllAlcoholIntakes()).Returns(alcoholIntakes);
            _entityAggregator.Setup(x => x.GroupByMonth(alcoholIntakes)).Returns(monthlyAlcoholIntakes);

            //when
            var result = _healthService.GetAllAlcoholIntakesByMonth();

            //then
            Assert.Equal(monthlyAlcoholIntakes, result);

        }

        [Fact]
        public void ShouldGetAllHeartRateSummaries()
        {

            //Given
            var heartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018,6,6), CardioMinutes = 123}
            };

            _entityDecorator.Setup(x => x.GetAllHeartRateSummaries()).Returns(heartRateSummaries);

            //when
            var result = _healthService.GetAllHeartRateSummaries();

            //then
            Assert.Equal(heartRateSummaries, result);

        }

        [Fact]
        public void ShouldGetAllHeartRateSummariesByWeek()
        {

            //Given
            var heartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018,6,6), CardioMinutes = 123}
            };

            var weeklyHeartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,7), CardioMinutes = 127}
            };

            _entityDecorator.Setup(x => x.GetAllHeartRateSummaries()).Returns(heartRateSummaries);
            _entityAggregator.Setup(x => x.GroupByWeek(heartRateSummaries)).Returns(weeklyHeartRateSummaries);

            //when
            var result = _healthService.GetAllHeartRateSummariesByWeek();

            //then
            Assert.Equal(weeklyHeartRateSummaries, result);

        }

        [Fact]
        public void ShouldGetAllHeartRateSummariesByMonth()
        {

            //Given
            var heartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018,6,6), CardioMinutes = 123}
            };

            var monthlyHeartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,7), CardioMinutes = 127}
            };

            _entityDecorator.Setup(x => x.GetAllHeartRateSummaries()).Returns(heartRateSummaries);
            _entityAggregator.Setup(x => x.GroupByMonth(heartRateSummaries)).Returns(monthlyHeartRateSummaries);

            //when
            var result = _healthService.GetAllHeartRateSummariesByMonth();

            //then
            Assert.Equal(monthlyHeartRateSummaries, result);

        }


        [Fact]
        public void ShouldGetAllActivitySummaries()
        {

            //Given
            var activitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2018,6,6), VeryActiveMinutes = 123}
            };
            
            _entityDecorator.Setup(x => x.GetAllActivitySummaries()).Returns(activitySummaries);

            //when
            var result = _healthService.GetAllActivitySummaries();

            //then
            Assert.Equal(activitySummaries, result);

        }

        [Fact]
        public void ShouldGetAllActivitySummariesByWeek()
        {

            //Given
            var activitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2018,6,6), VeryActiveMinutes = 123}
            };

            var activitySummariesByWeek = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2017,7,7), VeryActiveMinutes = 777}
            };

            _entityDecorator.Setup(x => x.GetAllActivitySummaries()).Returns(activitySummaries);
            _entityAggregator.Setup(x => x.GroupByWeek(activitySummaries)).Returns(activitySummariesByWeek);

            //when
            var result = _healthService.GetAllActivitySummariesByWeek();

            //then
            Assert.Equal(activitySummariesByWeek, result);

        }

        [Fact]
        public void ShouldGetAllActivitySummariesByMonth()
        {

            //Given
            var activitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2018,6,6), VeryActiveMinutes = 123}
            };

            var activitySummariesByMonth = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2016,6,6), VeryActiveMinutes = 66}
            };

            _entityDecorator.Setup(x => x.GetAllActivitySummaries()).Returns(activitySummaries);
            _entityAggregator.Setup(x => x.GroupByMonth(activitySummaries)).Returns(activitySummariesByMonth);

            //when
            var result = _healthService.GetAllActivitySummariesByMonth();

            //then
            Assert.Equal(activitySummariesByMonth, result);

        }
    }
}