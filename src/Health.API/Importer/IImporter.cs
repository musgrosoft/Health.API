using System.Threading.Tasks;

namespace HealthAPI.Importer
{
    public interface IImporter
    {
        Task ImportFitbitRestingHeartRates();
        Task ImportFitbitSleepSummaries();
        
        Task ImportWithingsWeights();
        Task ImportWithingsBloodPressures();

        Task ImportGoogleSheetsDrinks();
        Task ImportGoogleSheetsExercises();
        Task ImportGoogleSheetsTargets();
        Task ImportGoogleSheetsGarminRestingHeartRates();
        Task ImportGoogleSheetsGarminIntensityMinutes();
    }
}