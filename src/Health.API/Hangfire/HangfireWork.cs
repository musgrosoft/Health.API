using System;
using System.Linq;
using System.Threading.Tasks;
using Fitbit;
using Services.Health;
using Utils;

namespace HealthAPI.Hangfire
{
    public class HangfireWork : IHangfireWork
    {
        private readonly IFitbitWork _fitbitWork;
        private ILogger _logger;

        public HangfireWork(ILogger logger, IFitbitWork fitbitWork)
        {
            _logger = logger;
            _fitbitWork = fitbitWork;
        }

        public async Task MigrateAllFitbitData()
        {
            try
            {
                await _fitbitWork.ImportRestingHeartRates();
                await _fitbitWork.ImportSleepSummaries();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex);
            }
        }




    }
}