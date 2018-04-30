namespace Utils
{
    public interface IConfig
    {
        string DyanmoDbAccessKey { get; }
        string DynamoDbSecretKey { get; }
        string FitbitUserId { get; }
    }
}