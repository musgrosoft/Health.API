using System;
using System.Net.Http;

namespace Utils
{
    public class Logger : ILogger
    {
        private readonly IConfig _config;

        public Logger(IConfig config)
        {
            _config = config;
        }

        public void Error(Exception ex)
        {
            var token = _config.LogzIoToken;
            var requestUri = $"http://listener.logz.io:8070/?token={token}&type=ERROR";
            var content = new StringContent("{\"message\": \"" + System.Web.HttpUtility.JavaScriptStringEncode(ex.ToString()) + "\", \"timestamp\" : \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\"}");
            var httpClient = new HttpClient();
            httpClient.PostAsync(requestUri, content);
        }

        public void Log(string message)
        {
            var token = _config.LogzIoToken;
            var requestUri = $"http://listener.logz.io:8070/?token={token}&type=LOG";
            var content = new StringContent("{\"message\": \"" + System.Web.HttpUtility.JavaScriptStringEncode(message) + "\", \"timestamp\" : \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\"}");
            var httpClient = new HttpClient();
            httpClient.PostAsync(requestUri, content);
        }


    }
}