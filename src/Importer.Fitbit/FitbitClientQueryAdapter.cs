using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Importer.Fitbit.Domain;
using Utils;

namespace Importer.Fitbit
{
    public class FitbitClientQueryAdapter : IFitbitClientQueryAdapter
    {
        private readonly FitbitClient _fitbitClient;
        private readonly ILogger _logger;

        public FitbitClientQueryAdapter(HttpClient httpClient, IConfig config, ILogger logger)
        {
            _fitbitClient = new FitbitClient(httpClient, config, logger);
            _logger = logger;
        }

        public async Task<IEnumerable<ActivitiesHeart>> GetFitbitHeartActivities(DateTime fromDate, DateTime toDate, string accessToken)
        {
            var heartActivities = new List<ActivitiesHeart>();

            for (DateTime date = fromDate.AddMonths(1);
                date <= toDate.AddMonths(1).AddDays(1);
                date = date.AddMonths(1))
            {
                FitBitActivity fitbitActivity;
                try
                { 
                    fitbitActivity = await _fitbitClient.GetMonthOfFitbitActivities(date, accessToken);
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

        public async Task<IEnumerable<Sleep>> GetFitbitSleeps(DateTime fromDate, DateTime toDate, string accessToken)
        {
            var allTheSleeps = new List<Sleep>();


            for (DateTime date = fromDate;
                date < toDate;
                date = date.AddDays(100))
            {
                FitbitSleeps fitbitSleeps;
                try
                {
                    fitbitSleeps = await _fitbitClient.Get100DaysOfSleeps(date, accessToken);
                }
                catch (TooManyRequestsException ex)
                {
                    await _logger.LogErrorAsync(ex);
                    break;
                }
                allTheSleeps.AddRange(fitbitSleeps.sleep);
            }

            return allTheSleeps.Where(x => x.dateOfSleep.Between(fromDate, toDate));
        }
    }
}
