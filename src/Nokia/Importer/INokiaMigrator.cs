using System.Threading.Tasks;

namespace Nokia.Importer
{
    public interface INokiaMigrator
    {
        Task MigrateBloodPressures();
        Task MigrateWeights();
    }
}