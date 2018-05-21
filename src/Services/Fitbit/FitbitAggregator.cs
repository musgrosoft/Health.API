using Services.Fitbit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Services.Fitbit
{
    public class FitbitAggregator
    {
        private readonly FitbitClient _fitbitClient;

        public FitbitAggregator(FitbitClient fitbitClient)
        {
            _fitbitClient = fitbitClient;
        }

        public async Task<IEnumerable<FitbitDailyActivity>> GetFitbitDailyActivities(DateTime fromDate, DateTime toDate)
        {
            var fitbitDailyActivities = new List<FitbitDailyActivity>();

            for (DateTime date = fromDate;
                date < toDate;
                date = date.AddDays(1))
            {
                var fitbitDailyActivity = await _fitbitClient.GetFitbitDailyActivity(date);
                if (fitbitDailyActivities != null)
                {
                    fitbitDailyActivities.Add(fitbitDailyActivity);
                }
            }

            return fitbitDailyActivities;
        }

        public async Task<IEnumerable<ActivitiesHeart>> GetFitbitHeartActivities(DateTime fromDate, DateTime toDate)
        {
            var heartActivities = new List<ActivitiesHeart>();

            for (DateTime date = fromDate.AddMonths(1);
                date < toDate.AddMonths(1).AddDays(1);
                date = date.AddMonths(1))
            {
                var fitbitActivity = await _fitbitClient.GetMonthOfFitbitActivities(date);
                heartActivities.AddRange(fitbitActivity.activitiesHeart);
            }

            return heartActivities.Where(x => x.dateTime.Between(fromDate, toDate));
        }
    }
}
