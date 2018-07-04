﻿using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface IHealthService
    {


        DateTime GetLatestBloodPressureDate(DateTime defaultDateTime);
        DateTime GetLatestActivitySummaryDate(DateTime defaultDateTime);
        DateTime GetLatestHeartSummaryDate(DateTime defaultDateTime);
        DateTime GetLatestRestingHeartRateDate(DateTime defaultDateTime);
        DateTime GetLatestStepCountDate(DateTime defaultDateTime);
        DateTime GetLatestWeightDate(DateTime defaultDateTime);

        void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures);
        
        void UpsertActivitySummaries(IEnumerable<ActivitySummary> activitySummaries);
        void UpsertHeartSummaries(IEnumerable<HeartRateSummary> heartSummaries);
        void UpsertStepCounts(IEnumerable<StepCount> stepCount);
        void UpsertWeights(IEnumerable<Weight> weights);
        void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates);
        void UpsertAlcoholIntakes();


        IList<Weight> GetAllWeights();
        IList<BloodPressure> GetAllBloodPressures();
        IList<RestingHeartRate> GetAllRestingHeartRates();
    }
}