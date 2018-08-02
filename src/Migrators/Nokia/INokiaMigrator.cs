using System.Threading.Tasks;

namespace Migrators.Nokia
{
    public interface INokiaMigrator
    {
        Task MigrateBloodPressures();
        Task MigrateWeights();
    }
}