using Utils;

namespace Services.MyHealth
{
    public static class HealthServiceFactory
    {
        public static HealthService Build(ILogger logger) {
            
            return new HealthService(new Config(), logger);
        } 
    }
}
