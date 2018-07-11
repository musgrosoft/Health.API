namespace Utils
{
    public interface IConfig
    {
        string DyanmoDbAccessKey { get; }
        string DynamoDbSecretKey { get; }
        string FitbitUserId { get; }
        string HealthDbConnectionString { get; }
        string LogzIoToken { get; }
    }
}