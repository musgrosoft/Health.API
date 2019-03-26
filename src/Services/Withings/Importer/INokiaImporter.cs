using System.Threading.Tasks;

namespace Services.Withings.Importer
{
    public interface INokiaImporter
    {
        Task MigrateBloodPressures();
        Task MigrateWeights();
    }
}