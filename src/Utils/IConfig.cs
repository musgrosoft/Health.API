namespace Utils
{
    public interface IConfig
    {   
        string HealthDbConnectionString { get; }

        string LogzIoToken { get; }

        string GoogleClientId { get; }
        string GoogleClientSecret { get; }
        
        string NokiaClientId { get; }
        string NokiaClientSecret { get; }

        string FitbitClientId { get; }
        string FitbitClientSecret { get; }

        string FitbitUserId { get; }

        string FitbitVerificationCode { get; }
    }
}