using System.Threading.Tasks;

namespace Fitbit.Importer
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