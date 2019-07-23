using System.Threading.Tasks;

namespace Importer.Fitbit.Importer
{
    public interface IFitbitImporter
    {
        Task MigrateRestingHeartRates();

        Task MigrateSleeps();
    }
}