using Services.Domain;
using System;
using System.Collections.Generic;

namespace Services.MyHealth
{
    public class TargetService : ITargetService
    {
        public decimal? GetTargetWeight(DateTime dateTime)
        {
            var targets = new List<TargetWeight>();

            var targetStartDate = new DateTime(2018, 5, 1);
            var targetEndDate = DateTime.Now.AddDays(600);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var weightOnTargetStartDate = 90.74;
            var targetDailyWeightLoss = 0.5 / 30;
            var targetDailyWeightLoss2 = 0.25 / 30;

            var daysToHitHealthyWeight = 123;

            var daysDiff = (dateTime - targetStartDate).TotalDays;

            if (daysDiff < 0)
            {
                return null;
            }

            if (daysDiff <= daysToHitHealthyWeight)
            {
                return (decimal)(weightOnTargetStartDate - (daysDiff * targetDailyWeightLoss));
            }

            if (daysDiff <= totalDays)
            {
                return (decimal)((weightOnTargetStartDate - (daysToHitHealthyWeight * targetDailyWeightLoss + (daysDiff - daysToHitHealthyWeight) * targetDailyWeightLoss2)));
            }

            return null;


        }


    }
}
