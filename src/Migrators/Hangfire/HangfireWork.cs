using System;
using System.Threading.Tasks;
using Utils;

namespace Migrators
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
                _logger.Error(ex);
            }
        }
    }
}