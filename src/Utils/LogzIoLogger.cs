using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utils
{
    public class LogzIoLogger : ILogger
    {
        private readonly IConfig _config;
        private readonly HttpClient _httpClient;
        private const string LOGZ_IO_BASE_URL = "http://listener.logz.io:8070";

        public LogzIoLogger(IConfig config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task ErrorAsync(Exception ex)
        {
            var token = _config.LogzIoToken;
            var requestUri = $"{LOGZ_IO_BASE_URL}/?token={token}&type=ERROR";
            var content = new StringContent("{\"message\": \"" + System.Web.HttpUtility.JavaScriptStringEncode(ex.ToString()) + "\", \"timestamp\" : \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\"}");
            await _httpClient.PostAsync(requestUri, content);
        }

        public async Task LogAsync(string message)
        {
            var token = _config.LogzIoToken;
            var requestUri = $"{LOGZ_IO_BASE_URL}/?token={token}&type=LOG";
            var content = new StringContent("{\"message\": \"" + System.Web.HttpUtility.JavaScriptStringEncode(message) + "\", \"timestamp\" : \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\"}");
            await _httpClient.PostAsync(requestUri, content);
        }


    }
}