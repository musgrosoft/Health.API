using System.Threading.Tasks;

namespace Fitbit.Importer
{
    public interface IFitbitImporter
    {
        //Task MigrateActivitySummaries();
        //Task MigrateHeartSummaries();
        Task MigrateRestingHeartRates();
        //Task MigrateStepCounts();
    }
}