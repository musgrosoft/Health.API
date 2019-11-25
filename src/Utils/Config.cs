using System;
using Microsoft.Extensions.Configuration;

namespace Utils
{
    public class Config : IConfig
    {
        private IConfigurationRoot _configurationRoot;
        private IConfigurationRoot ConfigurationRoot
        {
            get
            {
                if (_configurationRoot == null)
                {
                    _configurationRoot = new ConfigurationBuilder().SetBasePath("C:\\config").AddJsonFile("HealthAPI.json").Build();
                }

                return _configurationRoot;
            }
        }

        private string GetConfigValue(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                value = ConfigurationRoot[name];
            }
            return value;
        }
        
        //Application
        public string HealthDbConnectionString => GetConfigValue("HealthDbConnectionString");
        public string LogzIoToken => GetConfigValue("LogzIoToken");

        //Fitbit
        public string FitbitClientId => GetConfigValue("FitbitClientId");
        public string FitbitClientSecret => GetConfigValue("FitbitClientSecret");
        public string FitbitUserId => GetConfigValue("FitbitUserId");
        public string FitbitVerificationCode => GetConfigValue("FitbitVerificationCode");
        public string FitbitOAuthRedirectUrl => "https://musgrosoft-health-api.azurewebsites.net/api/fitbit/oauth/";
        public string FitbitBaseUrl => "https://api.fitbit.com";

        //Google Sheets
//        public string GoogleClientId => GetConfigValue("GoogleClientId");
//        public string GoogleClientSecret => GetConfigValue("GoogleClientSecret");
        public string DrinksSpreadsheetId => "15c9GFccexP91E-YmcaGr6spIEeHVFu1APRl0tNVj1io";
        //public string AlcoholSpreadsheetId => "1f3aTKUUMwE63nKeow917vhfsQyN1RTyoLCu6M2iml0I";
        public string ExerciseSpreadsheetId => "1iZcGq0qBonWjU3cpmfz42zR-Mp7vHfr2uvw50s6Rj8g";

        public string TargetsSpreadsheetId => "1GinFPvwco9gmtH2ZnyVQNl10FtvxRgYrrGL4rEQ5PdQ";
//        public string DrinksRange => "Sheet1!A2:E";
//        public string ExercisesRange => "Sheet1!A2:E";
//        public string DrinksCsvUrl => GetConfigValue("DrinksCsvUrl");
//        public string ExercisesCsvUrl => GetConfigValue("ExercisesCsvUrl");
//        public string TargetsCsvUrl => GetConfigValue("TargetsCsvUrl");

        //Withings
        public string WithingsClientId => GetConfigValue("WithingsClientId");
        public string WithingsClientSecret => GetConfigValue("WithingsClientSecret");
        public string WithingsAccountBaseUrl => "https://account.withings.com";
        public string WithingsApiBaseUrl => "https://wbsapi.withings.net";
        public string WithingsRedirectUrl => "https://musgrosoft-health-api.azurewebsites.net/api/withings/oauth/";
    }
}
