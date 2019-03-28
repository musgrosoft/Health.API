using System;

namespace Services.Health
{
    public interface ITargetCalculator
    {
        double? GetTargetWeight(DateTime dateTime);
        
        
    }
}