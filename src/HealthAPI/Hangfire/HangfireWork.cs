using System;
using System.Threading.Tasks;
using Fitbit.Importer;
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
                //monthly gets
                await _fitbitImporter.MigrateRestingHeartRates();
                await _fitbitImporter.MigrateHeartSummaries();
                //daily gets
                await _fitbitImporter.MigrateStepCounts();
                await _fitbitImporter.MigrateActivitySummaries();
                //await _fitbitMigrator.MigrateRuns();

                await _fitbitImporter.MigrateDetailedHeartRates();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex);
            }
        }
    }
}