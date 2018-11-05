using System.Collections.Generic;
using Repositories.Models;

namespace Services.Health
{
    public interface ITargetService
    {
        List<Weight> SetTargets(IList<Weight> weights);
        List<StepCount> SetTargets(IList<StepCount> dailySteps);
        //List<ActivitySummary> SetTargets(IList<ActivitySummary> activitySummaries);
        //List<HeartRateSummary> SetTargets(IList<HeartRateSummary> heartSummaries);
        //List<AlcoholIntake> SetTargets(IList<AlcoholIntake> alcoholIntakes);
    }
}