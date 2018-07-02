using Services.Domain;
using System;
using System.Collections.Generic;

namespace Services.MyHealth
{
    public interface ITargetService
    {
        List<TargetWeight> GetTargetWeights();
//        decimal? GetTargetWeight(DateTime dateTime);
    }
}