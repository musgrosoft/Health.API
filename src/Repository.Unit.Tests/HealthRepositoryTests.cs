using Repositories.Health;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Text;
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

        [Fact]
        public void ShouldInsertActivitySummary()
        {
            var activitySummary = new ActivitySummary { VeryActiveMinutes = 123};

            _healthRepository.Upsert(activitySummary);

            var activitySummaries = _fakeLocalContext.ActivitySummaries;

            Assert.Contains(activitySummary, activitySummaries);
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
        public void ShouldInsertHeartSummary()
        {
            var heartSummary = new HeartRateSummary { CardioMinutes = 123 };

            _healthRepository.Upsert(heartSummary);

            var heartSummaries = _fakeLocalContext.HeartRateSummaries;

            Assert.Contains(heartSummary, heartSummaries);
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
        //public void ShouldInsertRun()
        //{
        //    var run = new Run { Distance = 123 };

        //    _healthRepository.Upsert(run);

        //    var runs = _fakeLocalContext.Runs;

        //    Assert.Contains(run, runs);
        //}

        [Fact]
        public void ShouldInsertStepCount()
        {
            var stepCount = new StepCount { Count = 123 };

            _healthRepository.Upsert(stepCount);

            var stepCounts = _fakeLocalContext.StepCounts;

            Assert.Contains(stepCount, stepCounts);
        }

        [Fact]
        public void ShouldInsertWeight()
        {
            var weight = new Weight { Kg = 123 };

            _healthRepository.Upsert(weight);

            var weights = _fakeLocalContext.Weights;

            Assert.Contains(weight, weights);
        }

        [Fact]
        public void ShouldGetLatestActivitySummaryDate()
        {
            var firstActivitySummary = new ActivitySummary { CreatedDate = new DateTime(2018, 5, 1) };
            var secondActivitySummary = new ActivitySummary { CreatedDate = new DateTime(2018, 5, 2) };
            var thirdActivitySummary = new ActivitySummary { CreatedDate = new DateTime(2018, 5, 3) };

            _fakeLocalContext.ActivitySummaries.Add(firstActivitySummary);
            _fakeLocalContext.ActivitySummaries.Add(secondActivitySummary);
            _fakeLocalContext.ActivitySummaries.Add(thirdActivitySummary);
            _fakeLocalContext.SaveChanges();

            var latestActivitySummary = _healthRepository.GetLatestActivitySummaryDate();

            Assert.Equal(thirdActivitySummary.CreatedDate, latestActivitySummary);

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
        public void ShouldGetLatestHeartSummaryDate()
        {
            var firstHeartSummary = new HeartRateSummary { CreatedDate = new DateTime(2018, 5, 1) };
            var secondHeartSummary = new HeartRateSummary { CreatedDate = new DateTime(2018, 5, 2) };
            var thirdHeartSummary = new HeartRateSummary { CreatedDate = new DateTime(2018, 5, 3) };

            _fakeLocalContext.HeartRateSummaries.Add(firstHeartSummary);
            _fakeLocalContext.HeartRateSummaries.Add(secondHeartSummary);
            _fakeLocalContext.HeartRateSummaries.Add(thirdHeartSummary);
            _fakeLocalContext.SaveChanges();

            var latestHeartSummaryDate = _healthRepository.GetLatestHeartSummaryDate();

            Assert.Equal(thirdHeartSummary.CreatedDate, latestHeartSummaryDate);
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
        public void ShouldGetLatestRunDate()
        {
            var firstRun = new Run { CreatedDate = new DateTime(2018, 5, 1) };
            var secondRun = new Run { CreatedDate = new DateTime(2018, 5, 2) };
            var thirdRun = new Run { CreatedDate = new DateTime(2018, 5, 3) };

            _fakeLocalContext.Runs.Add(firstRun);
            _fakeLocalContext.Runs.Add(secondRun);
            _fakeLocalContext.Runs.Add(thirdRun);
            _fakeLocalContext.SaveChanges();

            var latestRunDate = _healthRepository.GetLatestRunDate();

            Assert.Equal(thirdRun.CreatedDate, latestRunDate);
        }

        [Fact]
        public void ShouldGetLatestStepCountDate()
        {
            var firstStepCount = new StepCount { CreatedDate = new DateTime(2018, 5, 1) };
            var secondStepCount = new StepCount { CreatedDate = new DateTime(2018, 5, 2) };
            var thirdStepCount = new StepCount { CreatedDate = new DateTime(2018, 5, 3) };

            _fakeLocalContext.StepCounts.Add(firstStepCount);
            _fakeLocalContext.StepCounts.Add(secondStepCount);
            _fakeLocalContext.StepCounts.Add(thirdStepCount);
            _fakeLocalContext.SaveChanges();

            var latestStepCountDate = _healthRepository.GetLatestStepCountDate();

            Assert.Equal(thirdStepCount.CreatedDate, latestStepCountDate);
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
        //public void ShouldFindWeight()
        //{
        //    var firstWeight = new Weight { DateTime = new DateTime(2018, 6, 1), Kg = 1};
        //    var secondWeight = new Weight { DateTime = new DateTime(2018, 6, 2), Kg = 2 };
        //    var thirdWeight = new Weight { DateTime = new DateTime(2018, 6, 3), Kg = 3 };

        //    _fakeLocalContext.Weights.Add(firstWeight);
        //    _fakeLocalContext.Weights.Add(secondWeight);
        //    _fakeLocalContext.Weights.Add(thirdWeight);
        //    _fakeLocalContext.SaveChanges();

        //    var weight = _healthRepository.FindWeight(new DateTime(2018, 6, 2));

        //    Assert.Equal(2, weight.Kg);

        //}


        //[Fact]
        //public void ShouldFindBloodPressure()
        //{
        //    var firstBloodPressure = new BloodPressure { DateTime = new DateTime(2018, 6, 1), Systolic = 1 };
        //    var secondBloodPressure = new BloodPressure { DateTime = new DateTime(2018, 6, 2), Systolic = 2 };
        //    var thirdBloodPressure = new BloodPressure { DateTime = new DateTime(2018, 6, 3), Systolic = 3 };

        //    _fakeLocalContext.BloodPressures.Add(firstBloodPressure);
        //    _fakeLocalContext.BloodPressures.Add(secondBloodPressure);
        //    _fakeLocalContext.BloodPressures.Add(thirdBloodPressure);
        //    _fakeLocalContext.SaveChanges();

        //    var bloodPressure = _healthRepository.Find(new BloodPressure { DateTime = new DateTime(2018, 6, 3) });

        //    Assert.Equal(3, bloodPressure.Systolic);

        //}

        //[Fact]
        //public void ShouldFindStepCount()
        //{
        //    var firstStepCount = new StepCount { DateTime = new DateTime(2018, 6, 1), Count = 1 };
        //    var secondStepCount = new StepCount { DateTime = new DateTime(2018, 6, 2), Count = 2 };
        //    var thirdStepCount = new StepCount { DateTime = new DateTime(2018, 6, 3), Count = 3 };

        //    _fakeLocalContext.StepCounts.Add(firstStepCount);
        //    _fakeLocalContext.StepCounts.Add(secondStepCount);
        //    _fakeLocalContext.StepCounts.Add(thirdStepCount);
        //    _fakeLocalContext.SaveChanges();

        //    var stepCount = _healthRepository.Find(new StepCount { DateTime = new DateTime(2018, 6, 1) });

        //    Assert.Equal(1, stepCount.Count);

        //}

        //[Fact]
        //public void ShouldFindActivitySummary()
        //{
        //    var firstActivitySummary = new ActivitySummary { DateTime = new DateTime(2018, 6, 1), LightlyActiveMinutes = 1 };
        //    var secondActivitySummary = new ActivitySummary { DateTime = new DateTime(2018, 6, 2), LightlyActiveMinutes = 2 };
        //    var thirdActivitySummary = new ActivitySummary { DateTime = new DateTime(2018, 6, 3), LightlyActiveMinutes = 3 };

        //    _fakeLocalContext.ActivitySummaries.Add(firstActivitySummary);
        //    _fakeLocalContext.ActivitySummaries.Add(secondActivitySummary);
        //    _fakeLocalContext.ActivitySummaries.Add(thirdActivitySummary);
        //    _fakeLocalContext.SaveChanges();

        //    var activitySummary = _healthRepository.Find(new ActivitySummary { DateTime = new DateTime(2018, 6, 3) });

        //    Assert.Equal(3, activitySummary.LightlyActiveMinutes);

        //}

        //[Fact]
        //public void ShouldFindRestingHeartRate()
        //{
        //    var firstRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2018, 6, 1), Beats = 1 };
        //    var secondRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2018, 6, 2), Beats = 2 };
        //    var thirdRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2018, 6, 3), Beats = 3 };

        //    _fakeLocalContext.RestingHeartRates.Add(firstRestingHeartRate);
        //    _fakeLocalContext.RestingHeartRates.Add(secondRestingHeartRate);
        //    _fakeLocalContext.RestingHeartRates.Add(thirdRestingHeartRate);
        //    _fakeLocalContext.SaveChanges();

        //    var restingHeartRate = _healthRepository.Find(new RestingHeartRate { DateTime = new DateTime(2018, 6, 2) });

        //    Assert.Equal(2, restingHeartRate.Beats);

        //}

        //[Fact]
        //public void ShouldFindHeartSummary()
        //{
        //    var firstHeartSummary = new HeartSummary { DateTime = new DateTime(2018, 6, 1), FatBurnMinutes = 1 };
        //    var secondHeartSummary = new HeartSummary { DateTime = new DateTime(2018, 6, 2), FatBurnMinutes = 2 };
        //    var thirdHeartSummary = new HeartSummary { DateTime = new DateTime(2018, 6, 3), FatBurnMinutes = 3 };

        //    _fakeLocalContext.HeartSummaries.Add(firstHeartSummary);
        //    _fakeLocalContext.HeartSummaries.Add(secondHeartSummary);
        //    _fakeLocalContext.HeartSummaries.Add(thirdHeartSummary);
        //    _fakeLocalContext.SaveChanges();

        //    var heartSummary = _healthRepository.Find(new HeartSummary { DateTime = new DateTime(2018, 6, 3) });

        //    Assert.Equal(3, heartSummary.FatBurnMinutes);

        //}

        [Fact]
        public void ShouldUpdateWeight()
        {
            var existingWeight = new Weight {CreatedDate = new DateTime(2017, 1, 1), Kg = 1, FatRatioPercentage = 3};
            _fakeLocalContext.Weights.Add(existingWeight);
            _fakeLocalContext.SaveChanges();

            existingWeight.Kg = 2;
            existingWeight.FatRatioPercentage = 4;

            _healthRepository.Upsert(existingWeight);

            Assert.Equal(2,existingWeight.Kg);
            Assert.Equal(4, existingWeight.FatRatioPercentage);

        }

        [Fact]
        public void ShouldUpdateBloodPressure()
        {
            var existingBloodPressure = new BloodPressure() { CreatedDate = new DateTime(2017, 1, 1), Systolic = 1, Diastolic = 3};
            _fakeLocalContext.BloodPressures.Add(existingBloodPressure);
            _fakeLocalContext.SaveChanges();

            existingBloodPressure.Systolic = 2;
            existingBloodPressure.Diastolic = 4;

            _healthRepository.Upsert(existingBloodPressure);

            Assert.Equal(2, existingBloodPressure.Systolic);
            Assert.Equal(4, existingBloodPressure.Diastolic);

        }

        [Fact]
        public void ShouldUpdateStepCount()
        {
            var existingStepCount = new StepCount { CreatedDate = new DateTime(2017, 1, 1), Count = 1 };
            _fakeLocalContext.StepCounts.Add(existingStepCount);
            _fakeLocalContext.SaveChanges();

            existingStepCount.Count = 2;

            _healthRepository.Upsert(existingStepCount);

            Assert.Equal(2, existingStepCount.Count);

        }

        [Fact]
        public void ShouldUpdateActivitySummary()
        {
            var existingActivitySummary = new ActivitySummary { CreatedDate = new DateTime(2017, 1, 1), SedentaryMinutes = 1 , LightlyActiveMinutes = 2, FairlyActiveMinutes = 3, VeryActiveMinutes = 4};
            _fakeLocalContext.ActivitySummaries.Add(existingActivitySummary);
            _fakeLocalContext.SaveChanges();

            var newActivitySummary = new ActivitySummary() { CreatedDate = new DateTime(2017, 1, 1), SedentaryMinutes = 5, LightlyActiveMinutes = 6, FairlyActiveMinutes = 7, VeryActiveMinutes = 8 };

            _healthRepository.Upsert(newActivitySummary);

            Assert.Equal(5, existingActivitySummary.SedentaryMinutes);
            Assert.Equal(6, existingActivitySummary.LightlyActiveMinutes);
            Assert.Equal(7, existingActivitySummary.FairlyActiveMinutes);
            Assert.Equal(8, existingActivitySummary.VeryActiveMinutes);

        }

        [Fact]
        public void ShouldUpdateRestingHeartRate()
        {
            var existingRestingHeartRate = new RestingHeartRate() { CreatedDate = new DateTime(2017, 1, 1), Beats = 1 };
            _fakeLocalContext.RestingHeartRates.Add(existingRestingHeartRate);
            _fakeLocalContext.SaveChanges();

            existingRestingHeartRate.Beats = 2;

            _healthRepository.Upsert(existingRestingHeartRate);

            Assert.Equal(2, existingRestingHeartRate.Beats);

        }

        [Fact]
        public void ShouldUpdateHeartSummary()
        {
            var existingHeartSummary = new HeartRateSummary() { CreatedDate = new DateTime(2017, 1, 1), OutOfRangeMinutes = 1 , FatBurnMinutes = 2, CardioMinutes = 3, PeakMinutes = 4};
            _fakeLocalContext.HeartRateSummaries.Add(existingHeartSummary);
            _fakeLocalContext.SaveChanges();

            existingHeartSummary.OutOfRangeMinutes = 5;
            existingHeartSummary.FatBurnMinutes = 6;
            existingHeartSummary.CardioMinutes = 7;
            existingHeartSummary.PeakMinutes = 8;

            _healthRepository.Upsert(existingHeartSummary);

            Assert.Equal(5, existingHeartSummary.OutOfRangeMinutes);
            Assert.Equal(6, existingHeartSummary.FatBurnMinutes);
            Assert.Equal(7, existingHeartSummary.CardioMinutes);
            Assert.Equal(8, existingHeartSummary.PeakMinutes);

        }
    }
}
