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
        
        public string HealthDbConnectionString => GetConfigValue("HealthDbConnectionString");
        public string LogzIoToken => GetConfigValue("LogzIoToken");

        //Fitbit
        public string FitbitClientId => GetConfigValue("FitbitClientId");
        public string FitbitClientSecret => GetConfigValue("FitbitClientSecret");
        public string FitbitUserId => GetConfigValue("FitbitUserId");
        public string FitbitVerificationCode => GetConfigValue("FitbitVerificationCode");
        public string FitbitOAuthRedirectUrl
        {
            get { return "http://musgrosoft-health-api.azurewebsites.net/api/fitbit/oauth/"; }
        }

        //Google Sheets
        public string GoogleClientId => GetConfigValue("GoogleClientId");
        public string GoogleClientSecret => GetConfigValue("GoogleClientSecret");
        
        //Nokia Health
        public string NokiaClientId => GetConfigValue("NokiaClientId");
        public string NokiaClientSecret => GetConfigValue("NokiaClientSecret");
    }
}