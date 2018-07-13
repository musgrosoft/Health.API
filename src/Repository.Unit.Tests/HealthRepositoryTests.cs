﻿using Repositories.Health;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void ShouldGetAllActivitySummaries()
        {
            var activitySummaries = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2018,1,1), TargetCumSumActiveMinutes = 1},
                new ActivitySummary {CreatedDate = new DateTime(2018,1,2), TargetCumSumActiveMinutes = 2},
                new ActivitySummary {CreatedDate = new DateTime(2018,1,3), TargetCumSumActiveMinutes = 3}

            };

            foreach (var activitySummary in activitySummaries)
            {
                _fakeLocalContext.ActivitySummaries.Add(activitySummary);
            }
            
            _fakeLocalContext.SaveChanges();

            var result = _healthRepository.GetAllActivitySummaries().ToList();

            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.TargetCumSumActiveMinutes == 1);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.TargetCumSumActiveMinutes == 2);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.TargetCumSumActiveMinutes == 3);
        }

        [Fact]
        public void ShouldGetAllAlcoholIntakes()
        {
            var alcoholIntakes = new List<AlcoholIntake>
            {
                new AlcoholIntake {CreatedDate = new DateTime(2018,1,1), Units = 1},
                new AlcoholIntake {CreatedDate = new DateTime(2018,1,2), Units = 2},
                new AlcoholIntake {CreatedDate = new DateTime(2018,1,3), Units = 3}

            };

            foreach (var alcoholIntake in alcoholIntakes)
            {
                _fakeLocalContext.AlcoholIntakes.Add(alcoholIntake);
            }

            _fakeLocalContext.SaveChanges();

            var result = _healthRepository.GetAllAlcoholIntakes().ToList();

            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Units == 1);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Units == 2);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Units == 3);
        }

        [Fact]
        public void ShouldGetAllBloodPressures()
        {
            var bloodPressures = new List<BloodPressure>
            {
                new BloodPressure {CreatedDate = new DateTime(2018,1,1), Systolic = 1},
                new BloodPressure {CreatedDate = new DateTime(2018,1,2), Systolic = 2},
                new BloodPressure {CreatedDate = new DateTime(2018,1,3), Systolic = 3}

            };

            foreach (var bloodPressure in bloodPressures)
            {
                _fakeLocalContext.BloodPressures.Add(bloodPressure);
            }

            _fakeLocalContext.SaveChanges();

            var result = _healthRepository.GetAllBloodPressures().ToList();

            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Systolic == 1);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Systolic == 2);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Systolic == 3);
        }

        [Fact]
        public void ShouldGetAllHeartRateSummaries()
        {
            var heartRateSummaries = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018,1,1), TargetCumSumCardioAndAbove = 1},
                new HeartRateSummary {CreatedDate = new DateTime(2018,1,2), TargetCumSumCardioAndAbove = 2},
                new HeartRateSummary {CreatedDate = new DateTime(2018,1,3), TargetCumSumCardioAndAbove = 3}

            };

            foreach (var heartRateSummary in heartRateSummaries)
            {
                _fakeLocalContext.HeartRateSummaries.Add(heartRateSummary);
            }

            _fakeLocalContext.SaveChanges();

            var result = _healthRepository.GetAllHeartRateSummaries().ToList();

            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.TargetCumSumCardioAndAbove == 1);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.TargetCumSumCardioAndAbove == 2);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.TargetCumSumCardioAndAbove == 3);
        }


        [Fact]
        public void ShouldGetAllRestingHeartRates()
        {
            var unorderedRestingHeartRates = new List<RestingHeartRate>
            {
                new RestingHeartRate {CreatedDate = new DateTime(2018,1,3), Beats = 1},
                new RestingHeartRate {CreatedDate = new DateTime(2018,1,2), Beats = 2},
                new RestingHeartRate {CreatedDate = new DateTime(2018,1,1), Beats = 3}

            };

            foreach (var restingHeartRate in unorderedRestingHeartRates)
            {
                _fakeLocalContext.RestingHeartRates.Add(restingHeartRate);
            }

            _fakeLocalContext.SaveChanges();

            var orderedRestingHeartRates = _healthRepository.GetAllRestingHeartRates().ToList();

            Assert.Equal(3, orderedRestingHeartRates.Count());
            Assert.Contains(orderedRestingHeartRates, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Beats == 3);
            Assert.Contains(orderedRestingHeartRates, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Beats == 2);
            Assert.Contains(orderedRestingHeartRates, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Beats == 1);
        }

        [Fact]
        public void ShouldGetAllStepCounts()
        {
            var stepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2018,1,1), TargetCumSumCount = 1},
                new StepCount {CreatedDate = new DateTime(2018,1,2), TargetCumSumCount = 2},
                new StepCount {CreatedDate = new DateTime(2018,1,3), TargetCumSumCount = 3}

            };

            foreach (var stepCount in stepCounts)
            {
                _fakeLocalContext.StepCounts.Add(stepCount);
            }

            _fakeLocalContext.SaveChanges();

            var result = _healthRepository.GetAllStepCounts().ToList();

            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.TargetCumSumCount == 1);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.TargetCumSumCount == 2);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.TargetCumSumCount == 3);
        }

        [Fact]
        public void ShouldGetAllWeights()
        {
            var weights = new List<Weight>
            {
                new Weight {CreatedDate = new DateTime(2018,1,1), TargetKg = 1},
                new Weight {CreatedDate = new DateTime(2018,1,2), TargetKg = 2},
                new Weight {CreatedDate = new DateTime(2018,1,3), TargetKg = 3}

            };

            foreach (var weight in weights)
            {
                _fakeLocalContext.Weights.Add(weight);
            }

            _fakeLocalContext.SaveChanges();

            var result = _healthRepository.GetAllWeights().ToList();

            Assert.Equal(3, result.Count());
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.TargetKg == 1);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.TargetKg == 2);
            Assert.Contains(result, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.TargetKg == 3);
        }
    }
}
