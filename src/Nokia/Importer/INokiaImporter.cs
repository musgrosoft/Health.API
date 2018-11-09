using System.Threading.Tasks;

namespace Nokia.Importer
{
    public interface INokiaImporter
    {
        Task MigrateBloodPressures();
        Task MigrateWeights();
    }
}