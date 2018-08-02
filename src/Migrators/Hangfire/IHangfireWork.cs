using System.Threading.Tasks;

namespace Migrators.Hangfire
{
    public interface IHangfireWork
    {
        Task MigrateAllFitbitData();
    }
}