using System.Threading.Tasks;

namespace Migrators.Fitbit
{
    public interface IFitbitMigrator
    {
        Task MigrateActivitySummaries();
        Task MigrateHeartSummaries();
        Task MigrateRestingHeartRates();
        Task MigrateStepCounts();
    }
}