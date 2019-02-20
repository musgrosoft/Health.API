using Repositories.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repositories.Health.Models;
using Xunit;

namespace Repository.Unit.Tests
{
    public class HealthRepositoryTests : IDisposable
    {
        private HealthRepository _healthRepository;
        private FakeLocalContext _fakeLocalContext;

        public HealthRepositoryTests()
        {
            _fakeLocalContext = new FakeLocalContext();
            _healthRepository = new HealthRepository(_fakeLocalContext);
        }

        public void Dispose()
        {
            _fakeLocalContext.Database.EnsureDeleted();
        }


        //[Fact]
        //public void ShouldInsertAlcoholIntake()
        //{
        //    var alcoholIntake = new AlcoholIntake { Units = 123 };

        //    _healthRepository.Upsert(alcoholIntake);

        //    var alcoholIntakes = _fakeLocalContext.AlcoholIntakes;

        //    Assert.Contains(alcoholIntake, alcoholIntakes);
        //}

        [Fact]
        public void ShouldInsertBloodPressure()
        {
            var bloodPressure = new BloodPressure { Systolic = 123 };

            _healthRepository.Upsert(bloodPressure);

            var bloodPressures = _fakeLocalContext.BloodPressures;

            Assert.Contains(bloodPressure, bloodPressures);
        }


        [Fact]
        public void ShouldInsertRestingHeartRate()
        {
            var restingHeartRate = new RestingHeartRate { Beats = 123 };

            _healthRepository.Upsert(restingHeartRate);

            var restingHeartRates = _fakeLocalContext.RestingHeartRates;

            Assert.Contains(restingHeartRate, restingHeartRates);
        }

        [Fact]
        public void ShouldInsertWeight()
        {
            var weight = new Weight { Kg = 123, CreatedDate = new DateTime(2018,1,1)};

            _healthRepository.Upsert(weight);

            var weights = _fakeLocalContext.Weights;

            Assert.Contains(weight, weights);
        }

        [Fact]
        public void ShouldInsertAlcoholIntake()
        {
            var alcoholIntake = new AlcoholIntake { CreatedDate = new DateTime(2018,1,1) , Units = 123 };

            _healthRepository.Upsert(alcoholIntake);

            var alcoholIntakes = _fakeLocalContext.AlcoholIntakes;

            Assert.Contains(alcoholIntake, alcoholIntakes);
        }
        
        [Fact]
        public void ShouldGetLatestAlcoholIntakeDate()
        {
            var firstAlcoholIntake = new AlcoholIntake { CreatedDate = new DateTime(2018, 5, 1) };
            var secondAlcoholIntake = new AlcoholIntake { CreatedDate = new DateTime(2018, 5, 2) };
            var thirdAlcoholIntake = new AlcoholIntake { CreatedDate = new DateTime(2018, 5, 3) };

            _fakeLocalContext.AlcoholIntakes.Add(firstAlcoholIntake);
            _fakeLocalContext.AlcoholIntakes.Add(secondAlcoholIntake);
            _fakeLocalContext.AlcoholIntakes.Add(thirdAlcoholIntake);
            _fakeLocalContext.SaveChanges();

            var latestAlcoholIntakeDate = _healthRepository.GetLatestAlcoholIntakeDate();

            Assert.Equal(thirdAlcoholIntake.CreatedDate, latestAlcoholIntakeDate);

        }

        [Fact]
        public void ShouldGetLatestBloodPressureDate()
        {
            var firstBloodPressure = new BloodPressure { CreatedDate = new DateTime(2018, 5, 1) };
            var secondBloodPressure = new BloodPressure { CreatedDate = new DateTime(2018, 5, 2) };
            var thirdBloodPressure = new BloodPressure { CreatedDate = new DateTime(2018, 5, 3) };

            _fakeLocalContext.BloodPressures.Add(firstBloodPressure);
            _fakeLocalContext.BloodPressures.Add(secondBloodPressure);
            _fakeLocalContext.BloodPressures.Add(thirdBloodPressure);
            _fakeLocalContext.SaveChanges();

            var latestBloodPressureDate = _healthRepository.GetLatestBloodPressureDate();

            Assert.Equal(thirdBloodPressure.CreatedDate, latestBloodPressureDate);
        }

        
        [Fact]
        public void ShouldGetLatestRestingHeartRateDate()
        {
            var firstRestingHeartRate = new RestingHeartRate { CreatedDate = new DateTime(2018, 5, 1) };
            var secondRestingHeartRate = new RestingHeartRate { CreatedDate = new DateTime(2018, 5, 2) };
            var thirdRestingHeartRate = new RestingHeartRate { CreatedDate = new DateTime(2018, 5, 3) };

            _fakeLocalContext.RestingHeartRates.Add(firstRestingHeartRate);
            _fakeLocalContext.RestingHeartRates.Add(secondRestingHeartRate);
            _fakeLocalContext.RestingHeartRates.Add(thirdRestingHeartRate);
            _fakeLocalContext.SaveChanges();

            var latestRestingHeartRateDate = _healthRepository.GetLatestRestingHeartRateDate();

            Assert.Equal(thirdRestingHeartRate.CreatedDate, latestRestingHeartRateDate);
        }


        [Fact]
        public void ShouldGetLatestWeightDate()
        {
            var firstWeight = new Weight { CreatedDate = new DateTime(2018, 5, 1) };
            var secondWeight = new Weight { CreatedDate = new DateTime(2018, 5, 2) };
            var thirdWeight = new Weight { CreatedDate = new DateTime(2018, 5, 3) };

            _fakeLocalContext.Weights.Add(firstWeight);
            _fakeLocalContext.Weights.Add(secondWeight);
            _fakeLocalContext.Weights.Add(thirdWeight);
            _fakeLocalContext.SaveChanges();

            var latestWeightDate = _healthRepository.GetLatestWeightDate();

            Assert.Equal(thirdWeight.CreatedDate, latestWeightDate);

        }

        [Fact]
        public void ShouldUpdateWeight()
        {
            var existingWeight = new Weight {CreatedDate = new DateTime(2017, 1, 1), Kg = 1, FatRatioPercentage = 3};
            _fakeLocalContext.Weights.Add(existingWeight);
            _fakeLocalContext.SaveChanges();

            var newWeight = new Weight { CreatedDate = new DateTime(2017, 1, 1), Kg = 2, FatRatioPercentage = 4 };

            _healthRepository.Upsert(newWeight);

            Assert.Equal(2,existingWeight.Kg);
            Assert.Equal(4, existingWeight.FatRatioPercentage);

        }

        [Fact]
        public void ShouldUpdateBloodPressure()
        {
            var existingBloodPressure = new BloodPressure() { CreatedDate = new DateTime(2017, 1, 1), Systolic = 1, Diastolic = 3};
            _fakeLocalContext.BloodPressures.Add(existingBloodPressure);
            _fakeLocalContext.SaveChanges();

            var newBloodPressure = new BloodPressure() { CreatedDate = new DateTime(2017, 1, 1), Systolic = 2, Diastolic = 4 };

            _healthRepository.Upsert(newBloodPressure);

            Assert.Equal(2, existingBloodPressure.Systolic);
            Assert.Equal(4, existingBloodPressure.Diastolic);
        }

    
        [Fact]
        public void ShouldUpdateAlcoholIntake()
        {
            var existingAlcoholIntake = new AlcoholIntake { CreatedDate = new DateTime(2017, 1, 1), Units = 1234};
            _fakeLocalContext.AlcoholIntakes.Add(existingAlcoholIntake);
            _fakeLocalContext.SaveChanges();

            var newAlcoholIntake = new AlcoholIntake { CreatedDate = new DateTime(2017, 1, 1), Units = 2345 };

            _healthRepository.Upsert(newAlcoholIntake);

            Assert.Equal(2345, existingAlcoholIntake.Units);
        }

      

        [Fact]
        public void ShouldUpdateRestingHeartRate()
        {
            var existingRestingHeartRate = new RestingHeartRate() { CreatedDate = new DateTime(2017, 1, 1), Beats = 1 };
            _fakeLocalContext.RestingHeartRates.Add(existingRestingHeartRate);
            _fakeLocalContext.SaveChanges();

            var newRestingHeartRate = new RestingHeartRate() { CreatedDate = new DateTime(2017, 1, 1), Beats = 2 };

            _healthRepository.Upsert(newRestingHeartRate);

            Assert.Equal(2, existingRestingHeartRate.Beats);
        }

        //[Fact]
        //public void ShouldUpdateHeartSummary()
        //{
        //    var existingHeartSummary = new HeartRateSummary() { CreatedDate = new DateTime(2017, 1, 1), CardioMinutes = 3, PeakMinutes = 4};
        //    _fakeLocalContext.HeartRateSummaries.Add(existingHeartSummary);
        //    _fakeLocalContext.SaveChanges();

        //    var newHeartSummary = new HeartRateSummary() { CreatedDate = new DateTime(2017, 1, 1), CardioMinutes = 7, PeakMinutes = 8};

        //    _healthRepository.Upsert(newHeartSummary);

        //    Assert.Equal(7, existingHeartSummary.CardioMinutes);
        //    Assert.Equal(8, existingHeartSummary.PeakMinutes);
        //}

        ////[Fact]
        ////public void ShouldGetAllActivitySummaries()
        ////{
        ////    var activitySummaries = new List<ActivitySummary>
        ////    {
        ////        new ActivitySummary {CreatedDate = new DateTime(2018,1,1)},
        ////        new ActivitySummary {CreatedDate = new DateTime(2018,1,2)},
        ////        new ActivitySummary {CreatedDate = new DateTime(2018,1,3)}

        ////    };

        ////    foreach (var activitySummary in activitySummaries)
        ////    {
        ////        _fakeLocalContext.ActivitySummaries.Add(activitySummary);
        ////    }
            
        ////    _fakeLocalContext.SaveChanges();

        ////    var result = _healthRepository.GetAllActivitySummaries().ToList();

        ////    Assert.Equal(3, result.Count());

        ////    Assert.Equal(new DateTime(2018, 1, 1), result[0].CreatedDate);
            
        ////    Assert.Equal(new DateTime(2018, 1, 2), result[1].CreatedDate);
            
        ////    Assert.Equal(new DateTime(2018, 1, 3), result[2].CreatedDate);
        ////}

        ////[Fact]
        ////public void ShouldGetAllAlcoholIntakes()
        ////{
        ////    var alcoholIntakes = new List<AlcoholIntake>
        ////    {
        ////        new AlcoholIntake {CreatedDate = new DateTime(2018,1,3), Units = 1},
        ////        new AlcoholIntake {CreatedDate = new DateTime(2018,1,2), Units = 2},
        ////        new AlcoholIntake {CreatedDate = new DateTime(2018,1,1), Units = 3}

        ////    };

        ////    foreach (var alcoholIntake in alcoholIntakes)
        ////    {
        ////        _fakeLocalContext.AlcoholIntakes.Add(alcoholIntake);
        ////    }

        ////    _fakeLocalContext.SaveChanges();

        ////    var result = _healthRepository.GetAllAlcoholIntakes().ToList();

        ////    Assert.Equal(3, result.Count());
        ////    Assert.Equal(new DateTime(2018, 1, 1), result[0].CreatedDate);
        ////    Assert.Equal(3, result[0].Units);

        ////    Assert.Equal(new DateTime(2018, 1, 2), result[1].CreatedDate);
        ////    Assert.Equal(2, result[1].Units);

        ////    Assert.Equal(new DateTime(2018, 1, 3), result[2].CreatedDate);
        ////    Assert.Equal(1, result[2].Units);
        ////}

        ////[Fact]
        ////public void ShouldGetAllBloodPressures()
        ////{
        ////    var bloodPressures = new List<BloodPressure>
        ////    {
                
        ////        new BloodPressure {CreatedDate = new DateTime(2018,1,2), Systolic = 2},
        ////        new BloodPressure {CreatedDate = new DateTime(2018,1,1,1,0,0), Systolic = 10},
        ////        new BloodPressure {CreatedDate = new DateTime(2018,1,1,2,0,0), Systolic = 20},
        ////        new BloodPressure {CreatedDate = new DateTime(2018,1,1,3,0,0), Systolic = 30},
        ////        new BloodPressure {CreatedDate = new DateTime(2018,1,3), Systolic = 3}

        ////    };

        ////    foreach (var bloodPressure in bloodPressures)
        ////    {
        ////        _fakeLocalContext.BloodPressures.Add(bloodPressure);
        ////    }

        ////    _fakeLocalContext.SaveChanges();

        ////    var result = _healthRepository.GetAllBloodPressures().ToList();

        ////    Assert.Equal(3, result.Count());

        ////    Assert.Equal(new DateTime(2018, 1, 1), result[0].CreatedDate);
        ////    Assert.Equal(20, result[0].Systolic);

        ////    Assert.Equal(new DateTime(2018, 1, 2), result[1].CreatedDate);
        ////    Assert.Equal(2, result[1].Systolic);

        ////    Assert.Equal(new DateTime(2018, 1, 3), result[2].CreatedDate);
        ////    Assert.Equal(3, result[2].Systolic);

        ////}

        ////[Fact]
        ////public void ShouldGetAllHeartRateSummaries()
        ////{
        ////    var heartRateSummaries = new List<HeartRateSummary>
        ////    {
        ////        new HeartRateSummary {CreatedDate = new DateTime(2018,1,3)},
        ////        new HeartRateSummary {CreatedDate = new DateTime(2018,1,2)},
        ////        new HeartRateSummary {CreatedDate = new DateTime(2018,1,1)}

        ////    };

        ////    foreach (var heartRateSummary in heartRateSummaries)
        ////    {
        ////        _fakeLocalContext.HeartRateSummaries.Add(heartRateSummary);
        ////    }

        ////    _fakeLocalContext.SaveChanges();

        ////    var result = _healthRepository.GetAllHeartRateSummaries().ToList();

        ////    Assert.Equal(3, result.Count());

        ////    Assert.Equal(new DateTime(2018, 1, 1), result[0].CreatedDate);
            
        ////    Assert.Equal(new DateTime(2018, 1, 2), result[1].CreatedDate);
            
        ////    Assert.Equal(new DateTime(2018, 1, 3), result[2].CreatedDate);
            
        ////}


        ////[Fact]
        ////public void ShouldGetAllRestingHeartRates()
        ////{
        ////    var unorderedRestingHeartRates = new List<RestingHeartRate>
        ////    {
        ////        new RestingHeartRate {CreatedDate = new DateTime(2018,1,3), Beats = 1},
        ////        new RestingHeartRate {CreatedDate = new DateTime(2018,1,2), Beats = 2},
        ////        new RestingHeartRate {CreatedDate = new DateTime(2018,1,1), Beats = 3}

        ////    };

        ////    foreach (var restingHeartRate in unorderedRestingHeartRates)
        ////    {
        ////        _fakeLocalContext.RestingHeartRates.Add(restingHeartRate);
        ////    }

        ////    _fakeLocalContext.SaveChanges();

        ////    var orderedRestingHeartRates = _healthRepository.GetAllRestingHeartRates().ToList();

        ////    Assert.Equal(3, orderedRestingHeartRates.Count());

        ////    Assert.Equal(new DateTime(2018, 1, 1), orderedRestingHeartRates[0].CreatedDate);
        ////    Assert.Equal(3, orderedRestingHeartRates[0].Beats);

        ////    Assert.Equal(new DateTime(2018, 1, 2), orderedRestingHeartRates[1].CreatedDate);
        ////    Assert.Equal(2, orderedRestingHeartRates[1].Beats);
            
        ////    Assert.Equal(new DateTime(2018, 1, 3), orderedRestingHeartRates[2].CreatedDate);
        ////    Assert.Equal(1, orderedRestingHeartRates[2].Beats);
        ////}

        ////[Fact]
        ////public void ShouldGetAllStepCounts()
        ////{
        ////    var stepCounts = new List<StepCount>
        ////    {
        ////        new StepCount {CreatedDate = new DateTime(2018,1,3)},
        ////        new StepCount {CreatedDate = new DateTime(2018,1,1)},
        ////        new StepCount {CreatedDate = new DateTime(2018,1,2)}

        ////    };

        ////    foreach (var stepCount in stepCounts)
        ////    {
        ////        _fakeLocalContext.StepCounts.Add(stepCount);
        ////    }

        ////    _fakeLocalContext.SaveChanges();

        ////    var result = _healthRepository.GetAllStepCounts().ToList();

        ////    Assert.Equal(3, result.Count());

        ////    Assert.Equal(new DateTime(2018, 1, 1), result[0].CreatedDate);
            
        ////    Assert.Equal(new DateTime(2018, 1, 2), result[1].CreatedDate);
            
        ////    Assert.Equal(new DateTime(2018, 1, 3), result[2].CreatedDate);
        ////}

        ////[Fact]
        ////public void ShouldGetAllRuns()
        ////{
        ////    var runs = new List<Run>
        ////    {
        ////        new Run {CreatedDate = new DateTime(2018,1,3), Metres = 1},
        ////        new Run {CreatedDate = new DateTime(2018,1,1), Metres = 2},
        ////        new Run {CreatedDate = new DateTime(2018,1,2), Metres = 3}

        ////    };

        ////    foreach (var run in runs)
        ////    {
        ////        _fakeLocalContext.Runs.Add(run);
        ////    }

        ////    _fakeLocalContext.SaveChanges();

        ////    var result = _healthRepository.GetAllRuns().ToList();

        ////    Assert.Equal(3, result.Count());

        ////    Assert.Equal(new DateTime(2018, 1, 1), result[0].CreatedDate);
        ////    Assert.Equal(2, result[0].Metres);

        ////    Assert.Equal(new DateTime(2018, 1, 2), result[1].CreatedDate);
        ////    Assert.Equal(3, result[1].Metres);

        ////    Assert.Equal(new DateTime(2018, 1, 3), result[2].CreatedDate);
        ////    Assert.Equal(1, result[2].Metres);
        ////}

        ////[Fact]
        ////public void ShouldGetAllErgos()
        ////{
        ////    var ergos = new List<Ergo>
        ////    {
        ////        new Ergo {CreatedDate = new DateTime(2018,1,3), Metres = 1},
        ////        new Ergo {CreatedDate = new DateTime(2018,1,1), Metres = 2},
        ////        new Ergo {CreatedDate = new DateTime(2018,1,2), Metres = 3}

        ////    };

        ////    foreach (var ergo in ergos)
        ////    {
        ////        _fakeLocalContext.Ergos.Add(ergo);
        ////    }

        ////    _fakeLocalContext.SaveChanges();

        ////    var result = _healthRepository.GetAllErgos().ToList();

        ////    Assert.Equal(3, result.Count());

        ////    Assert.Equal(new DateTime(2018, 1, 1), result[0].CreatedDate);
        ////    Assert.Equal(2, result[0].Metres);

        ////    Assert.Equal(new DateTime(2018, 1, 2), result[1].CreatedDate);
        ////    Assert.Equal(3, result[1].Metres);

        ////    Assert.Equal(new DateTime(2018, 1, 3), result[2].CreatedDate);
        ////    Assert.Equal(1, result[2].Metres);
        ////}

        ////[Fact]
        ////public void ShouldGetAllWeights()
        ////{
        ////    var weights = new List<Weight>
        ////    {

        ////        new Weight {CreatedDate = new DateTime(2018,1,2), Kg = 2},
        ////        new Weight {CreatedDate = new DateTime(2018,1,1,1,0,0), Kg = 10},
        ////        new Weight {CreatedDate = new DateTime(2018,1,1,2,0,0), Kg = 20},
        ////        new Weight {CreatedDate = new DateTime(2018,1,1,3,0,0), Kg = 30},
        ////        new Weight {CreatedDate = new DateTime(2018,1,3), Kg = 3}

        ////    };

        ////    foreach (var weight in weights)
        ////    {
        ////        _fakeLocalContext.Weights.Add(weight);
        ////    }

        ////    _fakeLocalContext.SaveChanges();

        ////    var result = _healthRepository.GetAllWeights().ToList();

        ////    Assert.Equal(3, result.Count());

        ////    Assert.Equal(new DateTime(2018, 1, 1), result[0].CreatedDate);
        ////    Assert.Equal(20, result[0].Kg);

        ////    Assert.Equal(new DateTime(2018, 1, 2), result[1].CreatedDate);
        ////    Assert.Equal(2, result[1].Kg);

        ////    Assert.Equal(new DateTime(2018, 1, 3), result[2].CreatedDate);
        ////    Assert.Equal(3, result[2].Kg);
        ////}



    }
}
