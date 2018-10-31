using System.Collections.Generic;
using Repositories.Models;

namespace Services.Health
{
    public interface ITargetService
    {
        List<Weight> SetTargets(IList<Weight> weights);
        List<StepCount> SetTargetsZZZ(IList<StepCount> dailySteps);
        List<ActivitySummary> SetTargetsZZZ(IList<ActivitySummary> activitySummaries);
        List<HeartRateSummary> SetTargetsZZZ(IList<HeartRateSummary> heartSummaries);
        List<AlcoholIntake> SetTargetsZZZ(IList<AlcoholIntake> alcoholIntakes);
    }
}