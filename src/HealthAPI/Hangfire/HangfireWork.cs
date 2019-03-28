using System;
using System.Threading.Tasks;
using Services.Fitbit.Importer;
using Utils;

namespace HealthAPI.Hangfire
{
    public class HangfireWork : IHangfireWork
    {
        private readonly IFitbitImporter _fitbitImporter;
        private readonly ILogger _logger;

        public HangfireWork(IFitbitImporter fitbitImporter, ILogger logger)
        {
            _fitbitImporter = fitbitImporter;
            _logger = logger;
        }

        public async Task MigrateAllFitbitData()
        {
            try
            {
                await _fitbitImporter.MigrateRestingHeartRates();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex);
            }
        }
    }
}