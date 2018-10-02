using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Utils
{
    public class LogzIoLogger : ILogger
    {
        private readonly IHealthConfig _config;
        private readonly HttpClient _httpClient;
        private readonly ICalendar _calendar;
        private const string LOGZ_IO_BASE_URL = "http://listener.logz.io:8070";

        public LogzIoLogger(IHealthConfig config, HttpClient httpClient, ICalendar calendar)
        {
            _config = config;
            _httpClient = httpClient;
            _calendar = calendar;
        }

        public async Task LogErrorAsync(Exception ex)
        {
            var token = _config.LogzIoToken;
            var requestUri = $"{LOGZ_IO_BASE_URL}/?token={token}&type=ERROR";
            var content = new StringContent("{\"message\": \"" + System.Web.HttpUtility.JavaScriptStringEncode(ex.ToString()) + "\", \"timestamp\" : \"" + _calendar.Now().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\"}");
            await _httpClient.PostAsync(requestUri, content);
        }

        public async Task LogMessageAsync(string message)
        {
            var token = _config.LogzIoToken;
            var requestUri = $"{LOGZ_IO_BASE_URL}/?token={token}&type=LOG";
            var content = new StringContent("{\"message\": \"" + System.Web.HttpUtility.JavaScriptStringEncode(message) + "\", \"timestamp\" : \"" + _calendar.Now().ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\"}");
            await _httpClient.PostAsync(requestUri, content);
        }


    }
}