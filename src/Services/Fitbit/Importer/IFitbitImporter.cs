using System.Threading.Tasks;

namespace Services.Fitbit.Importer
{
    public interface IFitbitImporter
    {
        Task MigrateRestingHeartRates();
        
    }
}