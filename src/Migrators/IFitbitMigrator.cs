using System.Threading.Tasks;

namespace Migrators
{
    public interface IFitbitMigrator
    {
        Task MigrateActivitySummaries();
        Task MigrateHeartSummaries();
        Task MigrateRestingHeartRates();
        Task MigrateStepCounts();
    }
}