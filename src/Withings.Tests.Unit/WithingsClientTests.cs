﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Utils;
using Xunit;

namespace Withings.Tests.Unit
{
    public class WithingsClientTests
    {
        private string withingsClientId = "12312jhjskdh937ey19d";
        private string withingsClientSecret = "fdjs982rhdgdsfogiuhd";
        private string baseAccountUrl = "https://www.nullaccount.com";
        private string baseApiUrl = "https://www.nullapi.com";
        private string redirectUrl = "https://www.redirect.com/thing/stuff/";
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private HttpClient _httpClient;
        private WithingsClient _withingsClient;
        private Mock<IConfig> _config;

        private HttpRequestMessage _capturedRequest;

        public WithingsClientTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();

            _capturedRequest = new HttpRequestMessage();

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _config = new Mock<IConfig>();
            _config.Setup(x => x.WithingsClientId).Returns(withingsClientId);
            _config.Setup(x => x.WithingsClientSecret).Returns(withingsClientSecret);
            
            _config.Setup(x => x.WithingsAccountBaseUrl).Returns(baseAccountUrl);
            _config.Setup(x => x.WithingsApiBaseUrl).Returns(baseApiUrl);
            _config.Setup(x => x.WithingsRedirectUrl).Returns(redirectUrl);
            
            _withingsClient = new WithingsClient(_httpClient, _config.Object);
        }

        private void SetupHttpMessageHandlerMock(string content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedRequest = h);
        }

        [Fact]
        public async Task ShouldSendCorrectRequestWhenGettingMeasureGroups()
        {
            //Given
            SetupHttpMessageHandlerMock(withingsContent);

            //When
            await _withingsClient.GetMeasureGroups("abc123", new DateTime());

            //Then
            Assert.Equal(HttpMethod.Get, _capturedRequest.Method);
            Assert.Equal("https://www.nullapi.com/measure?action=getmeas", _capturedRequest.RequestUri.AbsoluteUri);
            Assert.Equal("abc123", _capturedRequest.Headers.Authorization.Parameter);
            //_capturedRequest.Headers.Accept.
        }

        [Fact]
        public async Task ShouldParseContentWhenGettingMeasureGroups()
        {
            //Given
            SetupHttpMessageHandlerMock(withingsContent);

            //When
            var measuregrps = await _withingsClient.GetMeasureGroups("abc123", new DateTime());

            Assert.Equal($"{baseApiUrl}/measure?action=getmeas", _capturedRequest.RequestUri.AbsoluteUri);
            Assert.Equal("abc123", _capturedRequest.Headers.Authorization.Parameter);
            Assert.Equal(8, measuregrps.Count());
            Assert.Contains(measuregrps, x => x.date == 1526015332 && x.measures.Exists(a => a.value == 83000 && a.type == 9 && a.unit == -3));
            //Assert.Contains(measuregrps, x => x.Kg == 90.261 && x.CreatedDate == new DateTime(2018, 5, 10, 5, 4, 42));
        }

        
        [Fact]
        public async void ShouldThrowErrorOnNonSuccessStatusCodeWhenGettingMeasureGroups()
        {
            //Given
            SetupHttpMessageHandlerMock("this is an error", HttpStatusCode.NotFound);

            //When
            var exception = await Assert.ThrowsAsync<Exception>(() => _withingsClient.GetMeasureGroups("abc123", new DateTime()));

            //Then
            Assert.Contains("status code is 404", exception.Message);
            Assert.Contains("content is this is an error", exception.Message);
        }

        [Fact]
        public async Task ShouldSendCorrectRequestWhenGettingTokensByAuthorisationCode()
        {
            //Given
            SetupHttpMessageHandlerMock("{'access_token':'ccc' , 'refresh_token':'ddd' }");

            //When
            await _withingsClient.GetTokensByAuthorisationCode("abc123");

            //Then
            var content = await _capturedRequest.Content.ReadAsStringAsync();
            Assert.Equal(HttpMethod.Post, _capturedRequest.Method);
            Assert.Equal("https://www.nullaccount.com/oauth2/token", _capturedRequest.RequestUri.AbsoluteUri);
            Assert.Contains("grant_type=authorization_code", content);
            Assert.Contains($"client_id={withingsClientId}", content);
            Assert.Contains($"client_secret={withingsClientSecret}", content);
            Assert.Contains("code=abc123", content);
            Assert.Contains("redirect_uri=https%3A%2F%2Fwww.redirect.com%2Fthing%2Fstuff%2F", content);
        }


        [Fact]
        public async Task ShouldParseContentWhenGettingTokensByAuthorisationCode()
        {
            //Given
            string authorizationCode = "qwe321";

            SetupHttpMessageHandlerMock("{'access_token':'aaa111' , 'refresh_token':'bbb111' }");

            //When
            var tokenResponse = await _withingsClient.GetTokensByAuthorisationCode(authorizationCode);

            //Then
            Assert.Equal("aaa111", tokenResponse.access_token);
            Assert.Equal("bbb111", tokenResponse.refresh_token);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnNonSuccessStatusCodeWhenGettingTokensByAuthorisationCode()
        {
            //Given
            SetupHttpMessageHandlerMock("Has Error", HttpStatusCode.NotFound);

            //When
            var ex = await Assert.ThrowsAsync<Exception>(() => _withingsClient.GetTokensByAuthorisationCode(""));

            //Then
            Assert.Contains(((int)HttpStatusCode.NotFound).ToString(), ex.Message);
            Assert.Contains("Has Error", ex.Message);
        }

        [Theory]
        [InlineData(null, "r")]
        [InlineData("", "r")]
        [InlineData("a", null)]
        [InlineData("a", "")]
        public async void ShouldThrowErrorIfAccessTokenOrRefreshTokenIsEmptyWhenGettingTokensByAuthorisationCode(string accessToken, string refreshToken)
        {
            //Given
            string authorizationCode = "qwe321";

            SetupHttpMessageHandlerMock($"{{'access_token':'{accessToken}' , 'refresh_token':'{refreshToken}' }}");

            //When
            var exception = await Assert.ThrowsAsync<Exception>(() => _withingsClient.GetTokensByAuthorisationCode(authorizationCode));

            //Then
            Assert.Contains("Access token or Refresh token is empty", exception.Message);

        }

        [Fact]
        public async Task ShouldSendCorrectRequestWhenGettingTokensByRefreshToken()
        {
            //Given
            SetupHttpMessageHandlerMock("{'access_token':'ccc' , 'refresh_token':'ddd' }");

            //When
            await _withingsClient.GetTokensByRefreshToken("abc123");

            //Then
            var content = await _capturedRequest.Content.ReadAsStringAsync();
            Assert.Equal(HttpMethod.Post, _capturedRequest.Method);
            Assert.Equal("https://www.nullaccount.com/oauth2/token", _capturedRequest.RequestUri.AbsoluteUri);
            Assert.Contains("grant_type=refresh_token", content);
            Assert.Contains($"client_id={withingsClientId}", content);
            Assert.Contains($"client_secret={withingsClientSecret}", content);
            Assert.Contains("refresh_token=abc123", content);
            Assert.Contains("redirect_uri=https%3A%2F%2Fwww.redirect.com%2Fthing%2Fstuff%2F", content);
        }

        [Fact]
        public async Task ShouldParseContentWhenGettingTokensByRefreshToken()
        {
            //Given
            SetupHttpMessageHandlerMock("{'access_token':'ccc' , 'refresh_token':'ddd' }");

            //When
            var tokenResponse = await _withingsClient.GetTokensByRefreshToken("refresh123");

            //Then
            Assert.Equal("ccc", tokenResponse.access_token);
            Assert.Equal("ddd", tokenResponse.refresh_token);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnNonSuccessStatusCodeWhenGettingTokensByRefreshToken()
        {
            //Given
            SetupHttpMessageHandlerMock("Has Error", HttpStatusCode.NotFound);

            //When
            var ex = await Assert.ThrowsAsync<Exception>(() => _withingsClient.GetTokensByRefreshToken(""));

            //Then
            Assert.Contains(((int)HttpStatusCode.NotFound).ToString(), ex.Message);
            Assert.Contains("Has Error", ex.Message);
        }

        [Theory]
        [InlineData(null, "r")]
        [InlineData("", "r")]
        [InlineData("a", null)]
        [InlineData("a", "")]
        public async void ShouldThrowErrorIfAccessTokenOrRefreshTokenIsEmptyWhenGettingTokensByRefreshToken(string accessToken, string refreshToken)
        {
            //Given
            string authorizationCode = "qwe321";

            SetupHttpMessageHandlerMock($"{{'access_token':'{accessToken}' , 'refresh_token':'{refreshToken}' }}");
            
            //When
            var exception = await Assert.ThrowsAsync<Exception>(() => _withingsClient.GetTokensByRefreshToken(authorizationCode));

            //Then
            Assert.Contains("Access token or Refresh token is empty", exception.Message);

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
