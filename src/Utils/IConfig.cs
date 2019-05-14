namespace Utils
{
    public interface IConfig
    {
        //Database
        string HealthDbConnectionString { get; }

        //Logging Logz.io 
        string LogzIoToken { get; }

        //Fitbit
        string FitbitClientId { get; }
        string FitbitClientSecret { get; }
        string FitbitUserId { get; }
        string FitbitVerificationCode { get; }
        string FitbitOAuthRedirectUrl { get; }
        string FitbitBaseUrl { get;}

        //Google Sheets
        string GoogleClientId { get; }
        string GoogleClientSecret { get; }
        string HistoricAlcoholSpreadsheetId { get; }
        string AlcoholSpreadsheetId { get; }
        string ExerciseSpreadsheetId { get; }
        string DrinksRange { get; }
        string ExercisesRange { get; }

        //Withings
        string WithingsClientId { get; }
        string WithingsClientSecret { get; }
        string WithingsRedirectUrl { get; }

        string WithingsAccountBaseUrl { get; }
        string WithingsApiBaseUrl { get; }
    }
}