using System.Threading.Tasks;

namespace HealthAPI.Hangfire
{
    public interface IHangfireWork
    {
        Task MigrateAllFitbitData();
    }
}