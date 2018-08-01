using System.Threading.Tasks;

namespace Migrators
{
    public interface IFitbitMigrator
    {
        Task MigrateAllTheThings();

        Task MigrateActivitySummaries();
        Task MigrateHeartSummaries();
        Task MigrateRestingHeartRates();
        Task MigrateStepCounts();
    }
}