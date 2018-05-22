using Exceptionless;
using System.Net.Http;

namespace Utils
{
    public class Logger : ILogger
    {
        private ExceptionlessClient _client;

        public Logger()
        {
            _client = new ExceptionlessClient(c => {
                c.ApiKey = "iSMEpOiijtDRT9gFq0Knp2cArkdg4gP0xcPmGIFm";
                //c.SetVersion(version);
            });
        }

        public void Log(string message)
        {
            var requestUri = "http://listener.logz.io:8070/?token=gDonUjvYKMuWpLcDeBdSGyAbowpiusee&type=MY-TYPE";
            var content = new StringContent("{\"message\": \"" + message + "\"}");
            var httpClient = new HttpClient();
            httpClient.PostAsync(requestUri, content);
            //drop table stepcounts
            // _client.SubmitLog(message);
        }


    }
}