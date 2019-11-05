﻿using System;
using System.Collections.Generic;
using Repositories.Health.Models;

namespace Repositories.Health
{
    public interface IHealthRepository
    {
        DateTime? GetLatestBloodPressureDate();
        DateTime? GetLatestWeightDate();
        DateTime? GetLatestRestingHeartRateDate();
        DateTime? GetLatestDrinkDate();
        DateTime? GetLatestSleepSummaryDate();
        DateTime? GetLatestSleepStateDate(DateTime defaultDateTime);

        void Upsert(Weight weight);
        void Upsert(BloodPressure bloodPressure);
        void Upsert(RestingHeartRate restingHeartRate);
        void Upsert(Drink drink);
        void Upsert(Exercise exercise);
        void Upsert(SleepSummary sleepSummary);

        void Upsert(SleepState sleepState);
        //void Upsert(IEnumerable<SleepState> sleepStates);

        List<Weight> GetLatestWeights(int num);
        List<BloodPressure> GetLatestBloodPressures(int num);
        DateTime? GetLatestExerciseDate();
        List<Exercise> GetLatestExercises(int num);
        List<Drink> GetLatestDrinks(int num);
        List<RestingHeartRate> GetLatestRestingHeartRate(int num);
        Exercise GetFurthest(DateTime fromDate, string exerciseType, int totalSeconds);

        double GetCumSumUnits();
        Target GetTarget(DateTime date);
        double GetCumSumCardioMinutes();

        List<SleepSummary> GetLatestSleeps(int num);
        

    }
}