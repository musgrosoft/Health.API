using System;
using Repositories.Health;
using Repositories.Health.Models;
using Xunit;

namespace Repository.Tests.Unit
{
    public class HealthRepositoryTests : IDisposable
    {
        private readonly HealthRepository _healthRepository;
        private readonly FakeLocalContext _fakeLocalContext;

        public HealthRepositoryTests()
        {
            _fakeLocalContext = new FakeLocalContext();
            _healthRepository = new HealthRepository(_fakeLocalContext);
        }

        public void Dispose()
        {
            _fakeLocalContext.Database.EnsureDeleted();
        }
        
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

        //[Fact]
        //public void ShouldInsertWeight()
        //{
        //    var weight = new Weight { Kg = 123, CreatedDate = new DateTime(2018,1,1)};

        //    _healthRepository.Upsert(weight);

        //    var weights = _fakeLocalContext.Weights;

        //    Assert.Contains(weight, weights);
        //}

        [Fact]
        public void ShouldInsertAlcoholIntake()
        {
            var alcoholIntake = new Drink { CreatedDate = new DateTime(2018,1,1) , Units = 123 };

            _healthRepository.Upsert(alcoholIntake);

            var alcoholIntakes = _fakeLocalContext.Drinks;

            Assert.Contains(alcoholIntake, alcoholIntakes);
        }

        [Fact]
        public void ShouldInsertExercise()
        {
            var exercise = new Exercise { CreatedDate = new DateTime(2018, 1, 1), Description = "treadmill", Metres = 1, TotalSeconds = 1};

            _healthRepository.Upsert(exercise);

            var exercises = _fakeLocalContext.Exercises;

            Assert.Contains(exercise, exercises);
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
        public void ShouldGetLatestDrinkDate()
        {
            var firstDrink = new Drink { CreatedDate = new DateTime(2018, 5, 1) };
            var secondDrink = new Drink { CreatedDate = new DateTime(2018, 5, 2) };
            var thirdDrink = new Drink { CreatedDate = new DateTime(2018, 5, 3) };

            _fakeLocalContext.Drinks.Add(firstDrink);
            _fakeLocalContext.Drinks.Add(secondDrink);
            _fakeLocalContext.Drinks.Add(thirdDrink);
            _fakeLocalContext.SaveChanges();

            var latestDrinkDate = _healthRepository.GetLatestDrinkDate();

            Assert.Equal(thirdDrink.CreatedDate, latestDrinkDate);
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

        //[Fact]
        //public void ShouldUpdateWeight()
        //{
        //    var existingWeight = new Weight {CreatedDate = new DateTime(2017, 1, 1), Kg = 1, FatRatioPercentage = 3};
        //    _fakeLocalContext.Weights.Add(existingWeight);
        //    _fakeLocalContext.SaveChanges();

        //    var newWeight = new Weight { CreatedDate = new DateTime(2017, 1, 1), Kg = 2, FatRatioPercentage = 4 };

        //    _healthRepository.Upsert(newWeight);

        //    Assert.Equal(2,existingWeight.Kg);
        //    Assert.Equal(4, existingWeight.FatRatioPercentage);

        //}

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
            var existingAlcoholIntake = new Drink { CreatedDate = new DateTime(2017, 1, 1), Units = 1234};
            _fakeLocalContext.Drinks.Add(existingAlcoholIntake);
            _fakeLocalContext.SaveChanges();

            var newAlcoholIntake = new Drink { CreatedDate = new DateTime(2017, 1, 1), Units = 2345 };

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

        [Fact]
        public void ShouldUpdateExercise()
        {
            var existingExercise = new Exercise() { CreatedDate = new DateTime(2017, 1, 1), Description = "Ergo" , Metres = 1234, TotalSeconds = 445566};
            _fakeLocalContext.Exercises.Add(existingExercise);
            _fakeLocalContext.SaveChanges();

            var newRestingHeartRate = new Exercise() { CreatedDate = new DateTime(2017, 1, 1), Description = "Ergo", Metres = 2345, TotalSeconds = 556677};

            _healthRepository.Upsert(newRestingHeartRate);

            Assert.Equal(2345, existingExercise.Metres);
            Assert.Equal(556677, existingExercise.TotalSeconds);
        }


    }
}
