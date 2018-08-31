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

        public string FitbitUserId => GetConfigValue("FitbitUserId");

        public string HealthDbConnectionString => GetConfigValue("HealthDbConnectionString");

        public string LogzIoToken => GetConfigValue("LogzIoToken");

        public string GoogleClientId => GetConfigValue("GoogleClientId");
        public string GoogleClientSecret {
            get
            {
                var value = Environment.GetEnvironmentVariable("GoogleClientSecret");
                if (string.IsNullOrWhiteSpace(value))
                {

                    value = ConfigurationRoot["GoogleClientSecret"];

                }
                //
                return $"-----BEGIN PRIVATE KEY-----\n{value}gwggSkAgEAAoIBAQDDI5wOko3yoI+3\nYfXDh1bVfCTslr64SZ1omwr+w0dC9ggAKP1gF13R66lf4QZ1fu8Z/Et9AhaZp1rS\nBLJ/LV0HvKl+b5zP6N/frcl96EEEspkow1qxbGZCt7ikSwDBtk7l4kb2I3DLIgsY\nLtQvS2+dit+PgtgzI2Ede/5QxN+jI4+EQGWAHASCarIKtf6aZNQMvUX+9MsD3BsZ\nFPHZKIY98Cuh3J2UrWDHQnYkORN55VsPJzScxDxoGWukWw6yQYhLfPygL+/YH7n7\nElOxsRZhmv5Nyzj/Fj61xK/Z6QgqS92Phj64fxxWn84Uk9aRUwVRMLrT7LHNwB0h\n+GP7do/zAgMBAAECggEAB+NHGhtLci/4tZyklTt09s388hrjAuQ0epEV0zF49Nh1\nsof1ofsGN4eNAgtz069s7wIQ2eQKBNQyd1EDfa3x19YejDYeKRTmA8HsswWx2lF1\nbNcX5RpKhRtHDLRDB6scw+175jqNgyO3pH0af5gb68rV+ZRigXXf8mConLXKJZ5z\niwniQNGesKUO5+9Q5Tloz2B3837aA95CSvJwDQeWydpq5b5aUIgK30eX5YYb3W2W\ngBtNTT+8XIVYj118EAk92YmA50eBg+hS1ywjDqR+IyQZvDsFQl0wHo07HAlsMC7i\n5gJw8tOVb3cV7DkYOHKL9NWV06eV+G7+EDMlw8cMvQKBgQDxCdYX0w886qgXkE8X\nfvYVINUTzbP3geWXrDTqXMlp3KxGogmz7gKshArDWw0EMoXTVFqP7MpBsGDp/Ld8\n92j3/cg6Vf4O0/Wo5uadRF/DZzXzwnjr4f/oYQ94ifpCZ59rTceSfrcy9P2jCjwa\n/LeFUkj8sPcbmKoxt5+ZnWAXHQKBgQDPQGvIYobA5jlzreDOkTZ7C5ih6GOTYLGH\nPeqcdExrajkMCNrDfaOZ2/vpX8oUt0N5TWgSorbdP2hPQf4Etzs4qnZgSFubb8Zr\nimrfFYJDSmzrW0mnOo6OBMtSEvOwWtmHcKEJ0kU0ZUjHyzVV57LgQdxX5p5DdKC1\nmWhmlnvGTwKBgQCti5l8Gdh00bcpEBwCHMVtPPBti8OxxvLZm4GV5CyYbewUBwLZ\na9q8/20IbvwWM5IrMCbsTV7qoOWGUxCBh1kqhKyvY87COlY7v9P3E42nkR5FXZXW\nsCei1o9fCGkJerZLefrhPR8GL2KXCv1vqNkxwqcOWRPx6J9RoJXm8mqG+QKBgQCv\nAUsCTpBTWxSwZecrBtvmUwhzz3QmAsRl2xa9Pkc5fNznGKMB6Jm5VlUF3+kYTRgg\n2ghDlDqt4NkH9EW3XbIYS3jiHLeeLWhzbtKHxfXVNId8xf0PxRaTPeEiUPaNuGlv\nQN/7Fx9w+wXWD/XdsGUPkQ2q/AYwF8+NCRlrZy7laQKBgBW87z7N9zbn5n7Vjosu\n9YONJ9I3MA2iF2Xb2oIVdNpLkAIfXM56YrwTy735trs1BHye7AI6EWzMC6bPdasa\ncvGD7x5YvHRwmtKETPMp6I4ULUw4wPczRJ/GXM4BJKtAWQmlN1Ua1b9XunjQzHXf\n/m4QhJq0/tKEmqXULO+BHSna\n-----END PRIVATE KEY-----";
            }
        }

        public string FitbitVerificationCode => GetConfigValue("FitbitVerificationCode");

        public string NokiaClientId => GetConfigValue("NokiaClientId");

        public string NokiaClientSecret => GetConfigValue("NokiaClientSecret");
       
        public string FitbitClientId => GetConfigValue("FitbitClientId");

        public string FitbitClientSecret => GetConfigValue("FitbitClientSecret");

    }
}