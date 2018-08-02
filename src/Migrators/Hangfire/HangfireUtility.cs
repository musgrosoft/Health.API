using System;
using System.Threading.Tasks;
using Hangfire;
using Utils;

namespace Migrators
{
    public class HangfireUtility : IHangfireUtility
    {
        private readonly IFitbitMigrator _fitbitMigrator;
        private readonly ILogger _logger;

        public HangfireUtility(IFitbitMigrator fitbitMigrator, ILogger logger)
        {
            _fitbitMigrator = fitbitMigrator;
            _logger = logger;
        }
        
        public void Enqueue(Func<Task> func)
        {
            BackgroundJob.Enqueue(() => func());
        }
               

    }
}