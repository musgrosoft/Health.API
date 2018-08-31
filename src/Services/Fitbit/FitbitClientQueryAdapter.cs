using Services.Fitbit.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace Services.Fitbit
{
    public class FitbitClientQueryAdapter : IFitbitClientQueryAdapter
    {
        private readonly IFitbitClient _fitbitClient;
        private readonly ILogger _logger;

        public FitbitClientQueryAdapter(IFitbitClient fitbitClient, ILogger logger)
        {
            _fitbitClient = fitbitClient;
            _logger = logger;
        }

        public async Task<IEnumerable<FitbitDailyActivity>> GetFitbitDailyActivities(DateTime fromDate, DateTime toDate)
        {
            var fitbitDailyActivities = new List<FitbitDailyActivity>();

            for (DateTime date = fromDate;
                date <= toDate;
                date = date.AddDays(1))
            {
                FitbitDailyActivity fitbitDailyActivity;
                try
                {
                    fitbitDailyActivity = await _fitbitClient.GetFitbitDailyActivity(date);
                }
                catch (TooManyRequestsException ex)
                {
                    await _logger.LogErrorAsync(ex);
                    break;
                }
                
                if (fitbitDailyActivity != null)
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
                date <= toDate.AddMonths(1).AddDays(1);
                date = date.AddMonths(1))
            {
                FitBitActivity fitbitActivity;
                try
                { 
                    fitbitActivity = await _fitbitClient.GetMonthOfFitbitActivities(date);
                }
                catch (TooManyRequestsException ex)
                {
                    await _logger.LogErrorAsync(ex);
                    break;
                }
                heartActivities.AddRange(fitbitActivity.activitiesHeart);
            }

            return heartActivities.Where(x => x.dateTime.Between(fromDate, toDate));
        }
    }
}
