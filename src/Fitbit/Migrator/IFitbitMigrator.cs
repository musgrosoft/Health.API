using System.Threading.Tasks;

namespace Fitbit.Migrator
{
    public interface IFitbitMigrator
    {
        Task MigrateActivitySummaries();
        Task MigrateHeartSummaries();
        Task MigrateRestingHeartRates();
        Task MigrateStepCounts();
        //Task MigrateRuns();
    }
}