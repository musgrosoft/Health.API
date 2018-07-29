using System.Threading.Tasks;

namespace Migrators
{
    public interface IFitbitMigrator
    {
        Task MigrateAll();

        Task MigrateActivitySummaries();
        Task MigrateHeartSummaries();
        Task MigrateRestingHeartRates();
        Task MigrateStepCounts();
    }
}