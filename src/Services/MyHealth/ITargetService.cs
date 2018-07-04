using Services.Domain;
using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Services.MyHealth
{
    public interface ITargetService
    {
        //List<TargetWeight> GetTargetWeights();
//        decimal? GetTargetWeight(DateTime dateTime);
        IList<Weight> SetTargetWeights(IList<Weight> weights, int extraFutureDays);
    }
}