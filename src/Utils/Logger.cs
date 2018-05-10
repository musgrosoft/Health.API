using Exceptionless;

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
           // _client.SubmitLog(message);
        }
    }
}