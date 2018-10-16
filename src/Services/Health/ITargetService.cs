using System.Collections.Generic;
using Repositories.Models;

namespace Services.Health
{
    public interface ITargetService
    {
        IEnumerable<Weight> SetTargets(IEnumerable<Weight> weights);
        IList<StepCount> SetTargets(List<StepCount> stepCounts);
        IList<ActivitySummary> SetTargets(List<ActivitySummary> allActivitySummaries);
        IList<HeartRateSummary> SetTargets(List<HeartRateSummary> allHeartRateSummaries);
        IList<AlcoholIntake> SetTargets(List<AlcoholIntake> allAlcoholIntakes);
    }
}