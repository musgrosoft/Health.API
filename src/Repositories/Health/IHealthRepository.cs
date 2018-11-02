﻿using System;
using Repositories.Models;

namespace Repositories.Health
{
    public interface IHealthRepository
    {
        DateTime? GetLatestStepCountDate();
        DateTime? GetLatestBloodPressureDate();
        DateTime? GetLatestWeightDate();
        DateTime? GetLatestActivitySummaryDate();
        DateTime? GetLatestRestingHeartRateDate();
        DateTime? GetLatestHeartSummaryDate();
        DateTime? GetLatestRunDate();

        void Upsert(Weight weight);
        void Upsert(BloodPressure bloodPressure);
        void Upsert(StepCount stepCount);
        void Upsert(ActivitySummary activitySummary);
        void Upsert(RestingHeartRate restingHeartRate);
        void Upsert(HeartRateSummary heartSummary);
        void Upsert(Run run);
        void Upsert(AlcoholIntake alcoholIntake);
        void Upsert(Ergo ergo);
        
    }
}