using System.Threading.Tasks;

namespace Migrators
{
    public interface INokiaMigrator
    {
        Task MigrateBloodPressures();
        Task MigrateWeights();
    }
}