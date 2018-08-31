﻿using System;

namespace Services.Health
{
    public interface ITargetCalculator
    {
        double? GetTargetWeight(DateTime dateTime);
        double? GetTargetStepCountCumSum(DateTime dateTime);
        double? GetTargetActivitySummaryCumSum(DateTime createdDate);
        double? GetTargetCumSumCardioAndAbove(DateTime dateTime);
        double? GetAlcoholIntakeTarget(DateTime dateTime);
    }
}