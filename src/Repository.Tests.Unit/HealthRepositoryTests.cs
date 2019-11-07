using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task ShouldInsertBloodPressure()
        {
            var bloodPressures = new List<BloodPressure> {new BloodPressure {Systolic = 123}};

            await _healthRepository.UpsertAsync(bloodPressures);

            var retrievedBloodPressures = _fakeLocalContext.BloodPressures;

            Assert.Contains(bloodPressures.First(), retrievedBloodPressures);
        }

        [Fact]
        public async Task ShouldInsertRestingHeartRate()
        {
            var restingHeartRates = new List<RestingHeartRate> {new RestingHeartRate {Beats = 123}};

            await _healthRepository.UpsertAsync(restingHeartRates);

            var retrievedRestingHeartRates = _fakeLocalContext.RestingHeartRates;

            Assert.Contains(restingHeartRates.First(), retrievedRestingHeartRates);
        }

        [Fact]
        public async Task ShouldInsertWeight()
        {
            var weights = new List<Weight> { new Weight { Kg = 123, CreatedDate = new DateTime(2018,1,1)}};

            await _healthRepository.UpsertAsync(weights);

            var retrievedWeights = _fakeLocalContext.Weights;

            Assert.Contains(weights.First(), retrievedWeights);
        }

        [Fact]
        public async Task ShouldInsertDrink()
        {
            var drinks = new List<Drink> {new Drink {CreatedDate = new DateTime(2018, 1, 1), Units = 123}};

            await _healthRepository.UpsertAsync(drinks);

            var retrievedDrinks = _fakeLocalContext.Drinks;

            Assert.Contains(drinks.First(), retrievedDrinks);
        }

        [Fact]
        public async Task ShouldInsertExercise()
        {
            var exercises = new List<Exercise>
            {
                new Exercise {CreatedDate = new DateTime(2018, 1, 1), Description = "treadmill", Metres = 1, TotalSeconds = 1}
            };

            await _healthRepository.UpsertAsync(exercises);

            var retrievedExercises = _fakeLocalContext.Exercises;

            Assert.Contains(exercises.First(), retrievedExercises);
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

        [Fact]
        public async Task ShouldUpdateWeight()
        {
            var existingWeight = new Weight {CreatedDate = new DateTime(2017, 1, 1), Kg = 1, FatRatioPercentage = 3};
            _fakeLocalContext.Weights.Add(existingWeight);
            _fakeLocalContext.SaveChanges();

            var newWeights = new List<Weight> {new Weight {CreatedDate = new DateTime(2017, 1, 1), Kg = 2, FatRatioPercentage = 4}};

            await _healthRepository.UpsertAsync(newWeights);

            Assert.Equal(2, existingWeight.Kg);
            Assert.Equal(4, existingWeight.FatRatioPercentage);

        }

        [Fact]
        public async Task ShouldUpdateBloodPressure()
        {
            var existingBloodPressure = new BloodPressure() { CreatedDate = new DateTime(2017, 1, 1), Systolic = 1, Diastolic = 3 };
            _fakeLocalContext.BloodPressures.Add(existingBloodPressure);
            _fakeLocalContext.SaveChanges();

            var newBloodPressures = new List<BloodPressure> {new BloodPressure() {CreatedDate = new DateTime(2017, 1, 1), Systolic = 2, Diastolic = 4}};

            await _healthRepository.UpsertAsync(newBloodPressures);

            Assert.Equal(2, existingBloodPressure.Systolic);
            Assert.Equal(4, existingBloodPressure.Diastolic);
        }


        [Fact]
        public async Task ShouldUpdateDrink()
        {
            var existingAlcoholIntake = new Drink { CreatedDate = new DateTime(2017, 1, 1), Units = 1234};
            _fakeLocalContext.Drinks.Add(existingAlcoholIntake);
            _fakeLocalContext.SaveChanges();

            var newDrinks = new List<Drink> {new Drink {CreatedDate = new DateTime(2017, 1, 1), Units = 2345}};

            await _healthRepository.UpsertAsync(newDrinks);

            Assert.Equal(2345, existingAlcoholIntake.Units);
        }

        [Fact]
        public async Task ShouldUpdateRestingHeartRate()
        {
            var existingRestingHeartRate = new RestingHeartRate() { CreatedDate = new DateTime(2017, 1, 1), Beats = 1 };
            _fakeLocalContext.RestingHeartRates.Add(existingRestingHeartRate);
            _fakeLocalContext.SaveChanges();

            var newRestingHeartRates = new List<RestingHeartRate> {new RestingHeartRate() {CreatedDate = new DateTime(2017, 1, 1), Beats = 2}};

            await _healthRepository.UpsertAsync(newRestingHeartRates);

            Assert.Equal(2, existingRestingHeartRate.Beats);
        }

        [Fact]
        public async Task ShouldUpdateExercise()
        {
            var existingExercise = new Exercise() { CreatedDate = new DateTime(2017, 1, 1), Description = "Ergo", Metres = 1234, TotalSeconds = 445566 };
            _fakeLocalContext.Exercises.Add(existingExercise);
            _fakeLocalContext.SaveChanges();

            var newExercises = new List<Exercise> { new Exercise() {CreatedDate = new DateTime(2017, 1, 1), Description = "Ergo", Metres = 2345, TotalSeconds = 556677}};

            await _healthRepository.UpsertAsync(newExercises);

            Assert.Equal(2345, existingExercise.Metres);
            Assert.Equal(556677, existingExercise.TotalSeconds);
        }


    }
}
