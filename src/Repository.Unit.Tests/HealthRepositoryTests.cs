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

            _healthRepository.Insert(activitySummary);

            var activitySummaries = _fakeLocalContext.ActivitySummaries;

            Assert.Contains(activitySummary, activitySummaries);
        }

        [Fact]
        public void ShouldInsertAlcoholIntake()
        {
            var alcoholIntake = new AlcoholIntake { Units = 123 };

            _healthRepository.Insert(alcoholIntake);

            var alcoholIntakes = _fakeLocalContext.AlcoholIntakes;

            Assert.Contains(alcoholIntake, alcoholIntakes);
        }

        [Fact]
        public void ShouldInsertBloodPressure()
        {
            var bloodPressure = new BloodPressure { Systolic = 123 };

            _healthRepository.Insert(bloodPressure);

            var bloodPressures = _fakeLocalContext.BloodPressures;

            Assert.Contains(bloodPressure, bloodPressures);
        }

        [Fact]
        public void ShouldInsertHeartSummary()
        {
            var heartSummary = new HeartSummary { CardioMinutes = 123 };

            _healthRepository.Insert(heartSummary);

            var heartSummaries = _fakeLocalContext.HeartSummaries;

            Assert.Contains(heartSummary, heartSummaries);
        }

        [Fact]
        public void ShouldInsertRestingHeartRate()
        {
            var restingHeartRate = new RestingHeartRate { Beats = 123 };

            _healthRepository.Insert(restingHeartRate);

            var restingHeartRates = _fakeLocalContext.RestingHeartRates;

            Assert.Contains(restingHeartRate, restingHeartRates);
        }

        [Fact]
        public void ShouldInsertRun()
        {
            var run = new Run { Distance = 123 };

            _healthRepository.Insert(run);

            var runs = _fakeLocalContext.Runs;

            Assert.Contains(run, runs);
        }

        [Fact]
        public void ShouldInsertStepCount()
        {
            var stepCount = new StepCount { Count = 123 };

            _healthRepository.Insert(stepCount);

            var stepCounts = _fakeLocalContext.StepCounts;

            Assert.Contains(stepCount, stepCounts);
        }

        [Fact]
        public void ShouldInsertWeight()
        {
            var weight = new Weight { Kg = 123 };

            _healthRepository.Insert(weight);

            var weights = _fakeLocalContext.Weights;

            Assert.Contains(weight, weights);
        }

        [Fact]
        public void ShouldGetLatestActivitySummaryDate()
        {
            var firstActivitySummary = new ActivitySummary { DateTime = new DateTime(2018, 5, 1) };
            var secondActivitySummary = new ActivitySummary { DateTime = new DateTime(2018, 5, 2) };
            var thirdActivitySummary = new ActivitySummary { DateTime = new DateTime(2018, 5, 3) };

            _fakeLocalContext.ActivitySummaries.Add(firstActivitySummary);
            _fakeLocalContext.ActivitySummaries.Add(secondActivitySummary);
            _fakeLocalContext.ActivitySummaries.Add(thirdActivitySummary);
            _fakeLocalContext.SaveChanges();

            var latestActivitySummary = _healthRepository.GetLatestActivitySummaryDate();

            Assert.Equal(thirdActivitySummary.DateTime, latestActivitySummary);

        }

        [Fact]
        public void ShouldGetLatestAlcoholIntakeDate()
        {
            var firstAlcoholIntake = new AlcoholIntake { DateTime = new DateTime(2018, 5, 1) };
            var secondAlcoholIntake = new AlcoholIntake { DateTime = new DateTime(2018, 5, 2) };
            var thirdAlcoholIntake = new AlcoholIntake { DateTime = new DateTime(2018, 5, 3) };

            _fakeLocalContext.AlcoholIntakes.Add(firstAlcoholIntake);
            _fakeLocalContext.AlcoholIntakes.Add(secondAlcoholIntake);
            _fakeLocalContext.AlcoholIntakes.Add(thirdAlcoholIntake);
            _fakeLocalContext.SaveChanges();

            var latestAlcoholIntakeDate = _healthRepository.GetLatestAlcoholIntakeDate();

            Assert.Equal(thirdAlcoholIntake.DateTime, latestAlcoholIntakeDate);

        }

        [Fact]
        public void ShouldGetLatestBloodPressureDate()
        {
            var firstBloodPressure = new BloodPressure { DateTime = new DateTime(2018, 5, 1) };
            var secondBloodPressure = new BloodPressure { DateTime = new DateTime(2018, 5, 2) };
            var thirdBloodPressure = new BloodPressure { DateTime = new DateTime(2018, 5, 3) };

            _fakeLocalContext.BloodPressures.Add(firstBloodPressure);
            _fakeLocalContext.BloodPressures.Add(secondBloodPressure);
            _fakeLocalContext.BloodPressures.Add(thirdBloodPressure);
            _fakeLocalContext.SaveChanges();

            var latestBloodPressureDate = _healthRepository.GetLatestBloodPressureDate();

            Assert.Equal(thirdBloodPressure.DateTime, latestBloodPressureDate);
        }




        [Fact]
        public void ShouldGetLatestHeartSummaryDate()
        {
            var firstHeartSummary = new HeartSummary { DateTime = new DateTime(2018, 5, 1) };
            var secondHeartSummary = new HeartSummary { DateTime = new DateTime(2018, 5, 2) };
            var thirdHeartSummary = new HeartSummary { DateTime = new DateTime(2018, 5, 3) };

            _fakeLocalContext.HeartSummaries.Add(firstHeartSummary);
            _fakeLocalContext.HeartSummaries.Add(secondHeartSummary);
            _fakeLocalContext.HeartSummaries.Add(thirdHeartSummary);
            _fakeLocalContext.SaveChanges();

            var latestHeartSummaryDate = _healthRepository.GetLatestHeartSummaryDate();

            Assert.Equal(thirdHeartSummary.DateTime, latestHeartSummaryDate);
        }

        [Fact]
        public void ShouldGetLatestRestingHeartRateDate()
        {
            var firstRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2018, 5, 1) };
            var secondRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2018, 5, 2) };
            var thirdRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2018, 5, 3) };

            _fakeLocalContext.RestingHeartRates.Add(firstRestingHeartRate);
            _fakeLocalContext.RestingHeartRates.Add(secondRestingHeartRate);
            _fakeLocalContext.RestingHeartRates.Add(thirdRestingHeartRate);
            _fakeLocalContext.SaveChanges();

            var latestRestingHeartRateDate = _healthRepository.GetLatestRestingHeartRateDate();

            Assert.Equal(thirdRestingHeartRate.DateTime, latestRestingHeartRateDate);
        }

        [Fact]
        public void ShouldGetLatestRunDate()
        {
            var firstRun = new Run { DateTime = new DateTime(2018, 5, 1) };
            var secondRun = new Run { DateTime = new DateTime(2018, 5, 2) };
            var thirdRun = new Run { DateTime = new DateTime(2018, 5, 3) };

            _fakeLocalContext.Runs.Add(firstRun);
            _fakeLocalContext.Runs.Add(secondRun);
            _fakeLocalContext.Runs.Add(thirdRun);
            _fakeLocalContext.SaveChanges();

            var latestRunDate = _healthRepository.GetLatestRunDate();

            Assert.Equal(thirdRun.DateTime, latestRunDate);
        }

        [Fact]
        public void ShouldGetLatestStepCountDate()
        {
            var firstStepCount = new StepCount { DateTime = new DateTime(2018, 5, 1) };
            var secondStepCount = new StepCount { DateTime = new DateTime(2018, 5, 2) };
            var thirdStepCount = new StepCount { DateTime = new DateTime(2018, 5, 3) };

            _fakeLocalContext.StepCounts.Add(firstStepCount);
            _fakeLocalContext.StepCounts.Add(secondStepCount);
            _fakeLocalContext.StepCounts.Add(thirdStepCount);
            _fakeLocalContext.SaveChanges();

            var latestStepCountDate = _healthRepository.GetLatestStepCountDate();

            Assert.Equal(thirdStepCount.DateTime, latestStepCountDate);
        }

        [Fact]
        public void ShouldGetLatestWeightDate()
        {
            var firstWeight = new Weight { DateTime = new DateTime(2018, 5, 1) };
            var secondWeight = new Weight { DateTime = new DateTime(2018, 5, 2) };
            var thirdWeight = new Weight { DateTime = new DateTime(2018, 5, 3) };

            _fakeLocalContext.Weights.Add(firstWeight);
            _fakeLocalContext.Weights.Add(secondWeight);
            _fakeLocalContext.Weights.Add(thirdWeight);
            _fakeLocalContext.SaveChanges();

            var latestWeightDate = _healthRepository.GetLatestWeightDate();

            Assert.Equal(thirdWeight.DateTime, latestWeightDate);

        }

        [Fact]
        public void ShouldFindWeight()
        {
            var firstWeight = new Weight { DateTime = new DateTime(2018, 6, 1), Kg = 1};
            var secondWeight = new Weight { DateTime = new DateTime(2018, 6, 2), Kg = 2 };
            var thirdWeight = new Weight { DateTime = new DateTime(2018, 6, 3), Kg = 3 };

            _fakeLocalContext.Weights.Add(firstWeight);
            _fakeLocalContext.Weights.Add(secondWeight);
            _fakeLocalContext.Weights.Add(thirdWeight);
            _fakeLocalContext.SaveChanges();

            var weight = _healthRepository.Find(new Weight {DateTime = new DateTime(2018, 6, 2)} );

            Assert.Equal(2, weight.Kg);

        }


        [Fact]
        public void ShouldFindBloodPressure()
        {
            var firstBloodPressure = new BloodPressure { DateTime = new DateTime(2018, 6, 1), Systolic = 1 };
            var secondBloodPressure = new BloodPressure { DateTime = new DateTime(2018, 6, 2), Systolic = 2 };
            var thirdBloodPressure = new BloodPressure { DateTime = new DateTime(2018, 6, 3), Systolic = 3 };

            _fakeLocalContext.BloodPressures.Add(firstBloodPressure);
            _fakeLocalContext.BloodPressures.Add(secondBloodPressure);
            _fakeLocalContext.BloodPressures.Add(thirdBloodPressure);
            _fakeLocalContext.SaveChanges();

            var bloodPressure = _healthRepository.Find(new BloodPressure { DateTime = new DateTime(2018, 6, 3) });

            Assert.Equal(3, bloodPressure.Systolic);

        }

        [Fact]
        public void ShouldFindStepCount()
        {
            var firstStepCount = new StepCount { DateTime = new DateTime(2018, 6, 1), Count = 1 };
            var secondStepCount = new StepCount { DateTime = new DateTime(2018, 6, 2), Count = 2 };
            var thirdStepCount = new StepCount { DateTime = new DateTime(2018, 6, 3), Count = 3 };

            _fakeLocalContext.StepCounts.Add(firstStepCount);
            _fakeLocalContext.StepCounts.Add(secondStepCount);
            _fakeLocalContext.StepCounts.Add(thirdStepCount);
            _fakeLocalContext.SaveChanges();

            var stepCount = _healthRepository.Find(new StepCount { DateTime = new DateTime(2018, 6, 1) });

            Assert.Equal(1, stepCount.Count);

        }

        [Fact]
        public void ShouldFindActivitySummary()
        {
            var firstActivitySummary = new ActivitySummary { DateTime = new DateTime(2018, 6, 1), LightlyActiveMinutes = 1 };
            var secondActivitySummary = new ActivitySummary { DateTime = new DateTime(2018, 6, 2), LightlyActiveMinutes = 2 };
            var thirdActivitySummary = new ActivitySummary { DateTime = new DateTime(2018, 6, 3), LightlyActiveMinutes = 3 };

            _fakeLocalContext.ActivitySummaries.Add(firstActivitySummary);
            _fakeLocalContext.ActivitySummaries.Add(secondActivitySummary);
            _fakeLocalContext.ActivitySummaries.Add(thirdActivitySummary);
            _fakeLocalContext.SaveChanges();

            var activitySummary = _healthRepository.Find(new ActivitySummary { DateTime = new DateTime(2018, 6, 3) });

            Assert.Equal(3, activitySummary.LightlyActiveMinutes);

        }

        [Fact]
        public void ShouldFindRestingHeartRate()
        {
            var firstRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2018, 6, 1), Beats = 1 };
            var secondRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2018, 6, 2), Beats = 2 };
            var thirdRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2018, 6, 3), Beats = 3 };

            _fakeLocalContext.RestingHeartRates.Add(firstRestingHeartRate);
            _fakeLocalContext.RestingHeartRates.Add(secondRestingHeartRate);
            _fakeLocalContext.RestingHeartRates.Add(thirdRestingHeartRate);
            _fakeLocalContext.SaveChanges();

            var restingHeartRate = _healthRepository.Find(new RestingHeartRate { DateTime = new DateTime(2018, 6, 2) });

            Assert.Equal(2, restingHeartRate.Beats);

        }

        [Fact]
        public void ShouldFindHeartSummary()
        {
            var firstHeartSummary = new HeartSummary { DateTime = new DateTime(2018, 6, 1), FatBurnMinutes = 1 };
            var secondHeartSummary = new HeartSummary { DateTime = new DateTime(2018, 6, 2), FatBurnMinutes = 2 };
            var thirdHeartSummary = new HeartSummary { DateTime = new DateTime(2018, 6, 3), FatBurnMinutes = 3 };

            _fakeLocalContext.HeartSummaries.Add(firstHeartSummary);
            _fakeLocalContext.HeartSummaries.Add(secondHeartSummary);
            _fakeLocalContext.HeartSummaries.Add(thirdHeartSummary);
            _fakeLocalContext.SaveChanges();

            var heartSummary = _healthRepository.Find(new HeartSummary { DateTime = new DateTime(2018, 6, 3) });

            Assert.Equal(3, heartSummary.FatBurnMinutes);

        }
        //https://www.totaljobs.com/jobs-sitemaps/01.xml
        [Fact]
        public void ShouldUpdateWeight()
        {
            var existingWeight = new Weight {DateTime = new DateTime(2017, 1, 1), Kg = 1, FatRatioPercentage = 3};
            _fakeLocalContext.Weights.Add(existingWeight);
            _fakeLocalContext.SaveChanges();

            var newWeight = new Weight { DateTime = new DateTime(2017, 1, 1), Kg = 2, FatRatioPercentage = 4 };

            _healthRepository.Update(existingWeight, newWeight);

            Assert.Equal(2,existingWeight.Kg);
            Assert.Equal(4, existingWeight.FatRatioPercentage);

        }

        [Fact]
        public void ShouldUpdateBloodPressure()
        {
            var existingBloodPressure = new BloodPressure() { DateTime = new DateTime(2017, 1, 1), Systolic = 1, Diastolic = 3};
            _fakeLocalContext.BloodPressures.Add(existingBloodPressure);
            _fakeLocalContext.SaveChanges();

            var newBloodPressure = new BloodPressure() { DateTime = new DateTime(2017, 1, 1), Systolic = 2, Diastolic = 4};

            _healthRepository.Update(existingBloodPressure, newBloodPressure);

            Assert.Equal(2, existingBloodPressure.Systolic);
            Assert.Equal(4, existingBloodPressure.Diastolic);

        }

        [Fact]
        public void ShouldUpdateStepCount()
        {
            var existingStepCount = new StepCount { DateTime = new DateTime(2017, 1, 1), Count = 1 };
            _fakeLocalContext.StepCounts.Add(existingStepCount);
            _fakeLocalContext.SaveChanges();

            var newStepCount = new StepCount() { DateTime = new DateTime(2017, 1, 1), Count = 2 };

            _healthRepository.Update(existingStepCount, newStepCount);

            Assert.Equal(2, existingStepCount.Count);

        }

        [Fact]
        public void ShouldUpdateActivitySummary()
        {
            var existingActivitySummary = new ActivitySummary { DateTime = new DateTime(2017, 1, 1), SedentaryMinutes = 1 , LightlyActiveMinutes = 2, FairlyActiveMinutes = 3, VeryActiveMinutes = 4};
            _fakeLocalContext.ActivitySummaries.Add(existingActivitySummary);
            _fakeLocalContext.SaveChanges();

            var newActivitySummary = new ActivitySummary() { DateTime = new DateTime(2017, 1, 1), SedentaryMinutes = 5, LightlyActiveMinutes = 6, FairlyActiveMinutes = 7, VeryActiveMinutes = 8 };

            _healthRepository.Update(existingActivitySummary, newActivitySummary);

            Assert.Equal(5, existingActivitySummary.SedentaryMinutes);
            Assert.Equal(6, existingActivitySummary.LightlyActiveMinutes);
            Assert.Equal(7, existingActivitySummary.FairlyActiveMinutes);
            Assert.Equal(8, existingActivitySummary.VeryActiveMinutes);

        }

        [Fact]
        public void ShouldUpdateRestingHeartRate()
        {
            var existingRestingHeartRate = new RestingHeartRate() { DateTime = new DateTime(2017, 1, 1), Beats = 1 };
            _fakeLocalContext.RestingHeartRates.Add(existingRestingHeartRate);
            _fakeLocalContext.SaveChanges();

            var newRestingHeartRate = new RestingHeartRate { DateTime = new DateTime(2017, 1, 1), Beats = 2 };

            _healthRepository.Update(existingRestingHeartRate, newRestingHeartRate);

            Assert.Equal(2, existingRestingHeartRate.Beats);

        }

        [Fact]
        public void ShouldUpdateHeartSummary()
        {
            var existingHeartSummary = new HeartSummary() { DateTime = new DateTime(2017, 1, 1), OutOfRangeMinutes = 1 , FatBurnMinutes = 2, CardioMinutes = 3, PeakMinutes = 4};
            _fakeLocalContext.HeartSummaries.Add(existingHeartSummary);
            _fakeLocalContext.SaveChanges();

            var newHeartSummary = new HeartSummary() { DateTime = new DateTime(2017, 1, 1), OutOfRangeMinutes = 5, FatBurnMinutes = 6, CardioMinutes = 7, PeakMinutes = 8 };

            _healthRepository.Update(existingHeartSummary, newHeartSummary);

            Assert.Equal(5, existingHeartSummary.OutOfRangeMinutes);
            Assert.Equal(6, existingHeartSummary.FatBurnMinutes);
            Assert.Equal(7, existingHeartSummary.CardioMinutes);
            Assert.Equal(8, existingHeartSummary.PeakMinutes);

        }
    }
}
