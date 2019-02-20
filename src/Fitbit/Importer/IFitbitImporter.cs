using System.Threading.Tasks;

namespace Fitbit.Importer
{
    public interface IFitbitImporter
    {
        Task MigrateRestingHeartRates();
        
    }
}