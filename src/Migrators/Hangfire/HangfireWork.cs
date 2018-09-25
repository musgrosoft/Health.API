using System;
using System.Threading.Tasks;
using Fitbit.Migrator;
using Utils;

namespace Migrators.Hangfire
{
    public class HangfireWork : IHangfireWork
    {
        private readonly IFitbitMigrator _fitbitMigrator;
        private readonly ILogger _logger;

        public HangfireWork(IFitbitMigrator fitbitMigrator, ILogger logger)
        {
            _fitbitMigrator = fitbitMigrator;
            _logger = logger;
        }

        public async Task MigrateAllFitbitData()
        {
            try
            {
                //monthly gets
                await _fitbitMigrator.MigrateRestingHeartRates();
                await _fitbitMigrator.MigrateHeartSummaries();
                //daily gets
                await _fitbitMigrator.MigrateStepCounts();
                await _fitbitMigrator.MigrateActivitySummaries();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex);
            }
        }
    }
}