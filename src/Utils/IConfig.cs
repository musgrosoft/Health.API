namespace Utils
{
    public interface IConfig
    {
        string HealthDbConnectionString { get; }
        string LogzIoToken { get; }

        //Fitbit
        string FitbitClientId { get; }
        string FitbitClientSecret { get; }
        string FitbitUserId { get; }
        string FitbitVerificationCode { get; }
        string FitbitOAuthRedirectUrl { get; }
    

        //Google Sheets
        string GoogleClientId { get; }
        string GoogleClientSecret { get; }
        
        //Nokia Health
        string NokiaClientId { get; }
        string NokiaClientSecret { get; }
    }
}