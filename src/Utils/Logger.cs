using System;
using System.Net.Http;

namespace Utils
{
    public class Logger : ILogger
    {
        public void Error(Exception ex)
        {
            var requestUri = "http://listener.logz.io:8070/?token=gDonUjvYKMuWpLcDeBdSGyAbowpiusee&type=ERROR";
            var content = new StringContent("{\"message\": \"" + System.Web.HttpUtility.JavaScriptStringEncode(ex.ToString()) + "\", \"timestamp\" : \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\"}");
            var httpClient = new HttpClient();
            httpClient.PostAsync(requestUri, content);
        }

        public void Log(string message)
        {
            var requestUri = "http://listener.logz.io:8070/?token=gDonUjvYKMuWpLcDeBdSGyAbowpiusee&type=LOG";
            var content = new StringContent("{\"message\": \"" + System.Web.HttpUtility.JavaScriptStringEncode(message) + "\", \"timestamp\" : \"" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + "\"}");
            var httpClient = new HttpClient();
            httpClient.PostAsync(requestUri, content);
        }


    }
}