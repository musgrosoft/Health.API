using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Services.Fitbit;
using Services.Nokia;
using Utils;
using Xunit;

namespace Services.Tests.Fitbit
{
    public class FitbitClientTests
    {

        Uri _capturedUri = new Uri("http://www.null.com");
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private Mock<IConfig> _config;
        private Mock<ILogger> _logger;
        private HttpClient _httpClient;
        private FitbitClient _fitbitClient;
        private string _accessToken = "TEST_ACCESS_TOKEN";

        public FitbitClientTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fitbitDailyActivityContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;

            _config = new Mock<IConfig>();
            _logger = new Mock<ILogger>();

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _fitbitClient = new FitbitClient(_httpClient, _config.Object, _accessToken, _logger.Object);
        }

        //[Fact]
        //public async Task ShouldGetFitbitDailyActivity()
        //{
        //    var activity = await _fitbitClient.GetFitbitDailyActivity(new DateTime());

        //    Assert.Equal("http://api.health.nokia.com/measure?action=getmeas&oauth_consumer_key=ebb1cbd42bb69687cb85ccb20919b0ff006208b79c387059123344b921837d8d&oauth_nonce=742bef6a3da52fbf004573d18b8f04cf&oauth_signature=cgO95H%2Fg2qx0VQ9ma2k8qeHronM%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1503326610&oauth_token=7f027003b78369d415bd0ee8e91fdd43408896616108b72b97fd7c153685f&oauth_version=1.0&userid=8792669", _capturedUri.AbsoluteUri);
            
        //    Assert.Equal(123, activity.activities.Count);


        //}


        private readonly string fitbitDailyActivityContent = "";

    }
}
