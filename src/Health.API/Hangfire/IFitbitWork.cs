using System.Threading.Tasks;

namespace HealthAPI.Hangfire
{
    public interface IFitbitWork
    {
        Task ImportRestingHeartRates();
        Task ImportSleepSummaries();
    }
}