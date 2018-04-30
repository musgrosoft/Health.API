using Repositories;
using Utils;

namespace Services.MyHealth
{
    public static class HealthServiceFactory
    {
        public static HealthService Build(ILogger logger, HealthContext healthContext) {
            
            return new HealthService(new Config(), logger, healthContext);
        } 
    }
}
