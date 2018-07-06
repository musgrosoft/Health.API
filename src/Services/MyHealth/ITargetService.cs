using Services.Domain;
using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface ITargetService
    {
        IList<Weight> SetTargetWeights(IList<Weight> weights, int extraFutureDays);
        IList<StepCount> SetTargetStepCounts(List<StepCount> stepCounts, int extraFutureDays);
        IList<ActivitySummary> SetTargetActivitySummaries(List<ActivitySummary> allActivitySummaries, int extraFutureDays);
    }
}