using System.Collections.Generic;
using Repositories.Models;

namespace Services.Health
{
    public interface IEntityDecorator
    {
        IList<ActivitySummary> GetAllActivitySummaries();
        IList<HeartRateSummary> GetAllHeartRateSummaries();
        IList<Weight> GetAllWeights();
        IList<BloodPressure> GetAllBloodPressures();
        IList<RestingHeartRate> GetAllRestingHeartRates();
        IList<StepCount> GetAllStepCounts();
        IList<AlcoholIntake> GetAllAlcoholIntakes();
    }
}