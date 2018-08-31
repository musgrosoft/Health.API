using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;

namespace Utils.Unit.Tests
{
    public class LogzIoLoggerTests
    {
        private LogzIoLogger _logger;
        private Mock<IConfig> _config;
        private Uri _capturedUri;
        private StringContent _capturedContent;
        private Mock<ICalendar> _calendar;

        public LogzIoLoggerTests()
        {
            _config = new Mock<IConfig>();
            var httpMessageHandler = new Mock<HttpMessageHandler>();

            httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("yo")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) =>
                {
                    _capturedUri = h.RequestUri;
                    _capturedContent = (StringContent)h.Content;
                });

            var httpClient = new HttpClient(httpMessageHandler.Object);
            _calendar = new Mock<ICalendar>();

            _logger = new LogzIoLogger(_config.Object, httpClient, _calendar.Object);
        }

        [Fact]
        public async Task ShouldLogMessage()
        {
            //Given
            _calendar.Setup(x => x.Now()).Returns(new DateTime(2018, 12, 25, 12, 15, 59));
            var message = "This is my message";

            //When
            await _logger.LogAsync(message);

            Assert.Equal("http://listener.logz.io:8070/?token=&type=LOG", _capturedUri.AbsoluteUri);
            Assert.Equal(
                "{\"message\": \"This is my message\", \"timestamp\" : \"" + DateTime.Now.ToString("2018-12-25T12:15:59.000Z") + "\"}",
                await _capturedContent.ReadAsStringAsync()
                );
        }
    }
}
