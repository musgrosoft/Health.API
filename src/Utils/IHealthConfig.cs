namespace Utils
{
    public interface IHealthConfig
    {
        string HealthDbConnectionString { get; }
        string LogzIoToken { get; }
    }

}