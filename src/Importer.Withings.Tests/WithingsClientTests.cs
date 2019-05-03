using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Moq;
using Moq.Protected;
using Utils;
using Xunit;

namespace Importer.Withings.Tests
{
    public class WithingsClientTests
    {
      //  Uri _capturedUri = new Uri("http://www.null.com");
        private string withingsClientId = "12312jhjskdh937ey19d";
        private string withingsClientSecret = "fdjs982rhdgdsfogiuhd";
        private string baseUrl = "https://www.null.com";
        private string redirectUrl = "https://www.redirect.com/thing/stuff/";
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private HttpClient _httpClient;
        private WithingsClient _withingsClient;
        private Mock<IConfig> _config;

        private Mock<ILogger> _logger;

        private HttpRequestMessage _capturedRequest;

        public WithingsClientTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();

            _capturedRequest = new HttpRequestMessage();

            _httpClient = new HttpClient(_httpMessageHandler.Object);
            _logger = new Mock<ILogger>();

            _config = new Mock<IConfig>();
            _config.Setup(x => x.WithingsClientId).Returns(withingsClientId);
            _config.Setup(x => x.WithingsClientSecret).Returns(withingsClientSecret);
            
            _config.Setup(x => x.WithingsBaseUrl).Returns(baseUrl);
            _config.Setup(x => x.WithingsRedirectUrl).Returns(redirectUrl);
            
            _withingsClient = new WithingsClient(_httpClient, _logger.Object,  _config.Object);
        }

        [Fact]
        public async Task GetTokensByAuthorisationCodeShouldThrowExceptionOnNonSuccessStatusCode()
        {
            //Given
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Has error")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedRequest = h);

            //When
            var ex = await Assert.ThrowsAsync<Exception>(() => _withingsClient.GetTokensByAuthorisationCode(""));

            //Then
            Assert.Contains("404", ex.Message);
            Assert.Contains("Has error", ex.Message);
        }

        [Fact]
        public async Task ShouldGetTokensByAuthorisationCode()
        {
            //Given
            string authorizationCode = "qwe321";

            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'access_token':'aaa111' , 'refresh_token':'bbb111' }")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedRequest = h);


            //When

            var tokenResponse = await _withingsClient.GetTokensByAuthorisationCode(authorizationCode);

            //Then
            Assert.Equal("aaa111", tokenResponse.access_token);
            Assert.Equal("bbb111", tokenResponse.refresh_token);

            Assert.Equal($"{baseUrl}/oauth2/token", _capturedRequest.RequestUri.AbsoluteUri);

            var content = await _capturedRequest.Content.ReadAsStringAsync();

            Assert.Contains("grant_type=authorization_code", content);
            Assert.Contains($"client_id={withingsClientId}", content);
            Assert.Contains($"client_secret={withingsClientSecret}", content);
            Assert.Contains(($"redirect_uri={HttpUtility.UrlEncode(redirectUrl)}").ToUpper(), content.ToUpper());
            Assert.Contains($"code={authorizationCode}", content);
        }

        [Fact]
        public async Task ShouldGetTokensByRefreshToken()
        {
            //Given
            string refreshToken = "slkdjflsdjkfjsldkf";

            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'access_token':'ccc' , 'refresh_token':'ddd' }")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedRequest = h);

            //When

            var tokenResponse = await _withingsClient.GetTokensByRefreshToken(refreshToken);

            //Then
            Assert.Equal("ccc", tokenResponse.access_token);
            Assert.Equal("ddd", tokenResponse.refresh_token);

            Assert.Equal($"{baseUrl}/oauth2/token", _capturedRequest.RequestUri.AbsoluteUri);

            var content = await _capturedRequest.Content.ReadAsStringAsync();

            Assert.Contains("grant_type=refresh_token", content);
            Assert.Contains($"client_id={withingsClientId}", content);
            Assert.Contains($"client_secret={withingsClientSecret}", content);
            Assert.Contains(($"redirect_uri={HttpUtility.UrlEncode(redirectUrl)}").ToUpper(), content.ToUpper());
            Assert.Contains($"refresh_token={refreshToken}", content);

        }

        [Fact]
        public async Task ShouldGetMeasureGroups()
        {
            //Given
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(withingsContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedRequest = h);

            //When

            var measuregrps = await _withingsClient.GetMeasureGroups("abc123");

            Assert.Equal($"{baseUrl}/measure?action=getmeas&access_token=abc123", _capturedRequest.RequestUri.AbsoluteUri);
            Assert.Equal(8, measuregrps.Count());
            Assert.Contains(measuregrps, x => x.date == 1526015332 && x.measures.Exists(a => a.value == 83000 && a.type == 9 && a.unit == -3));
            //Assert.Contains(measuregrps, x => x.Kg == 90.261 && x.CreatedDate == new DateTime(2018, 5, 10, 5, 4, 42));
        }

        [Fact]
        public async void ShouldThrowErrorOnNoSuccessStausCodeWhenGetMeasureGroups()
        {
            //Given
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("this is an error")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedRequest = h);

            var exception = await Assert.ThrowsAsync<Exception>(() => _withingsClient.GetMeasureGroups("abc123"));

            Assert.Contains("status code is 404", exception.Message);
            Assert.Contains("content is this is an error", exception.Message);
        }


        private string withingsContent = @"{
    ""status"": 0,
    ""body"": {
        ""updatetime"": 1526036073,
        ""timezone"": ""Europe/London"",
        ""measuregrps"": [
            {
                ""grpid"": 1121435193,
                ""attrib"": 0,
                ""date"": 1526015332,
                ""category"": 1,
                ""measures"": [
                    {
                        ""value"": 83000,
                        ""type"": 9,
                        ""unit"": -3
                    },
                    {
                        ""value"": 130000,
                        ""type"": 10,
                        ""unit"": -3
                    },
                    {
                        ""value"": 59000,
                        ""type"": 11,
                        ""unit"": -3
                    }
                ],
                ""comment"": """"
            },
            {
                ""grpid"": 1121418348,
                ""attrib"": 0,
                ""date"": 1526014647,
                ""category"": 1,
                ""measures"": [
                    {
                        ""value"": 82,
                        ""type"": 11,
                        ""unit"": 0
                    }
                ],
                ""comment"": """"
            },
            {
                ""grpid"": 1121418346,
                ""attrib"": 0,
                ""date"": 1526014647,
                ""category"": 1,
                ""measures"": [
                    {
                        ""value"": 90435,
                        ""type"": 1,
                        ""unit"": -3
                    },
                    {
                        ""value"": 18605,
                        ""type"": 8,
                        ""unit"": -3
                    },
                    {
                        ""value"": 20573,
                        ""type"": 6,
                        ""unit"": -3
                    },
                    {
                        ""value"": 71830,
                        ""type"": 5,
                        ""unit"": -3
                    }
                ],
                ""comment"": """"
            },
            {
                ""grpid"": 1120439701,
                ""attrib"": 0,
                ""date"": 1525929643,
                ""category"": 1,
                ""measures"": [
                    {
                        ""value"": 80000,
                        ""type"": 9,
                        ""unit"": -3
                    },
                    {
                        ""value"": 127000,
                        ""type"": 10,
                        ""unit"": -3
                    },
                    {
                        ""value"": 69000,
                        ""type"": 11,
                        ""unit"": -3
                    }
                ],
                ""comment"": """"
            },
            {
                ""grpid"": 1120420822,
                ""attrib"": 0,
                ""date"": 1525928682,
                ""category"": 1,
                ""measures"": [
                    {
                        ""value"": 90,
                        ""type"": 11,
                        ""unit"": 0
                    }
                ],
                ""comment"": """"
            },
            {
                ""grpid"": 1120420816,
                ""attrib"": 0,
                ""date"": 1525928682,
                ""category"": 1,
                ""measures"": [
                    {
                        ""value"": 90261,
                        ""type"": 1,
                        ""unit"": -3
                    },
                    {
                        ""value"": 18573,
                        ""type"": 8,
                        ""unit"": -3
                    },
                    {
                        ""value"": 20577,
                        ""type"": 6,
                        ""unit"": -3
                    },
                    {
                        ""value"": 71688,
                        ""type"": 5,
                        ""unit"": -3
                    }
                ],
                ""comment"": """"
            },
            {
                ""grpid"": 1119513382,
                ""attrib"": 0,
                ""date"": 1525842761,
                ""category"": 1,
                ""measures"": [
                    {
                        ""value"": 81000,
                        ""type"": 9,
                        ""unit"": -3
                    },
                    {
                        ""value"": 134000,
                        ""type"": 10,
                        ""unit"": -3
                    },
                    {
                        ""value"": 66000,
                        ""type"": 11,
                        ""unit"": -3
                    }
                ],
                ""comment"": """"
            },
            {
                ""grpid"": 1119487010,
                ""attrib"": 0,
                ""date"": 1525841919,
                ""category"": 1,
                ""measures"": [
                    {
                        ""value"": 81,
                        ""type"": 11,
                        ""unit"": 0
                    }
                ],
                ""comment"": """"
            }
        ]
    }
}";

        
    }
}
