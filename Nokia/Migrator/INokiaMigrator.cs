using System.Threading.Tasks;

namespace Nokia.Migrator
{
    public interface INokiaMigrator
    {
        Task MigrateBloodPressures();
        Task MigrateWeights();
    }
}