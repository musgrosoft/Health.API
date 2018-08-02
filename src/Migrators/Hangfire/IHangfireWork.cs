using System.Threading.Tasks;

namespace Migrators
{
    public interface IHangfireWork
    {
        Task MigrateAllFitbitData();
    }
}