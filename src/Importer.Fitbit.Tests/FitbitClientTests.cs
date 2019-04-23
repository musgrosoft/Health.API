using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Utils;
using Xunit;

namespace Importer.Fitbit.Tests
{
    public class FitbitClientTests
    {

        
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private Mock<IConfig> _config;
        private Mock<ILogger> _logger;
        private HttpClient _httpClient;
        private FitbitClient _fitbitClient;

        private Mock<IFitbitAuthenticator> _fitbitAuthenticator;
       // private string _accessToken;

        public FitbitClientTests()
        {


            //_httpMessageHandler = new Mock<HttpMessageHandler>();
            //_httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            //    .Returns(Task.FromResult(new HttpResponseMessage
            //    {
            //        StatusCode = HttpStatusCode.OK,
            //        Content = new StringContent(fitbitDailyActivityContent)
            //    })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;

            //_httpClient = new HttpClient(_httpMessageHandler.Object);


            _httpMessageHandler = new Mock<HttpMessageHandler>();


            _config = new Mock<IConfig>();
            _logger = new Mock<ILogger>();
            _fitbitAuthenticator = new Mock<IFitbitAuthenticator>();

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _fitbitClient = new FitbitClient(_httpClient, _config.Object, _fitbitAuthenticator.Object, _logger.Object);
        }

        [Fact]
        public async Task ShouldGetFitbitDailyActivity()
        {
            Uri _capturedUri = new Uri("http://www.null.com");
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fitbitDailyActivityContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;

         //   var fitbitDailyActivity = await _fitbitClient.GetFitbitDailyActivity(new DateTime());

            //Assert.Equal("", _capturedUri.AbsoluteUri);

            //Assert.Equal(123, activity.activities.Count);


        }


        [Fact]
        public async Task ShouldGetFitbitMonthlyActivity()
        {
            Uri _capturedUri = new Uri("http://www.null.com");

            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(fitbitMonthlyActivityContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;



            var fitbitActivity = await _fitbitClient.GetMonthOfFitbitActivities(new DateTime());

            //Assert.Equal("", _capturedUri.AbsoluteUri);

            //Assert.Equal(123, activity.activities.Count);


        }

        [Fact]
        public async Task ShouldThrowTooManyRequestsExceptionWhenStatusCode429()
        {
            Uri _capturedUri = new Uri("http://www.null.com");

            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode) 429,
                    Content = new StringContent(fitbitMonthlyActivityContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;

            await Assert.ThrowsAsync<TooManyRequestsException>(() => _fitbitClient.GetMonthOfFitbitActivities(new DateTime()));

//            var fitbitActivity = await _fitbitClient.GetMonthOfFitbitActivities(new DateTime());
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenStatusCodeNotSuccess()
        {
            Uri _capturedUri = new Uri("http://www.null.com");

            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode)400,
                    Content = new StringContent("I'm a little teapot")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;

            Exception ex = await Assert.ThrowsAsync<Exception>(() => _fitbitClient.GetMonthOfFitbitActivities(new DateTime()));
            Assert.Contains("status code is 400", ex.Message);
            Assert.Contains("content is I'm a little teapot", ex.Message);

        }

        [Fact]
        public async Task ShouldSubscribe()
        {
            //Given
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("wibble")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); ;


            _config.Setup(x => x.FitbitUserId).Returns("USER_ID_123");

            _fitbitAuthenticator.Setup(x => x.GetAccessToken()).Returns(Task.FromResult("I am a token"));

            //When
            await _fitbitClient.Subscribe();

            //Then

            Assert.Equal("https://api.fitbit.com/1/user/USER_ID_123/apiSubscriptions/123.json", _capturedUri.AbsoluteUri);
            Assert.Equal("I am a token", _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            //also verify bearer token


        }

        private readonly string fitbitDailyActivityContent = "";

        private readonly string fitbitMonthlyActivityContent = "";
        private Uri _capturedUri
            ;
    }
}
