using System;
using Microsoft.Extensions.Configuration;

namespace Utils
{
    public class Config : IConfig
    {
        public string DyanmoDbAccessKey {
            get
            {
                var value = Environment.GetEnvironmentVariable("DyanmoDbAccessKey");
                if (string.IsNullOrWhiteSpace(value))
                {
                    var builder = new ConfigurationBuilder().SetBasePath("C:\\config").AddJsonFile("HealthAPI.json");
                    var config = builder.Build();

                    value = config["DyanmoDbAccessKey"];

                }
                return value;
            }
        }

        public string DynamoDbSecretKey{

            get
            {
                var value = Environment.GetEnvironmentVariable("DynamoDbSecretKey");
                if (string.IsNullOrWhiteSpace(value))
                {
                    var builder = new ConfigurationBuilder().SetBasePath("C:\\config").AddJsonFile("HealthAPI.json");
                    var config = builder.Build();

                    value = config["DynamoDbSecretKey"];

                }
                return value;
            }
            
        }

        public string FitbitUserId
        {
            get
            {
                var value = Environment.GetEnvironmentVariable("FitbitUserId");
                if (string.IsNullOrWhiteSpace(value))
                {
                    var builder = new ConfigurationBuilder().SetBasePath("C:\\config").AddJsonFile("HealthAPI.json");
                    var config = builder.Build();

                    value = config["FitbitUserId"];

                }
                return value;
            }
        }

        public string HealthDbConnectionString
        {
            get
            {
                var value = Environment.GetEnvironmentVariable("HealthDbConnectionString");
                if (string.IsNullOrWhiteSpace(value))
                {
                    var builder = new ConfigurationBuilder().SetBasePath("C:\\config").AddJsonFile("HealthAPI.json");
                    var config = builder.Build();

                    value = config["HealthDbConnectionString"];

                }
                return value;
            }
        }

        public string LogzIoToken
        {
            get
            {
                var value = Environment.GetEnvironmentVariable("LogzIoToken");
                if (string.IsNullOrWhiteSpace(value))
                {
                    var builder = new ConfigurationBuilder().SetBasePath("C:\\config").AddJsonFile("HealthAPI.json");
                    var config = builder.Build();

                    value = config["LogzIoToken"];

                }
                return value;
            }
        }
    }
}