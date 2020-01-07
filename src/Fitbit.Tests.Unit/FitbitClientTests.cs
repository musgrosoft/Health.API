using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Utils;
using Xunit;

namespace Fitbit.Tests.Unit
{
    public class FitbitClientTests
    {

        private string fitbitRedirect = "www.redirect.com";
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private Mock<IConfig> _config;
        private Mock<ILogger> _logger;
        private HttpClient _httpClient;
        private FitbitClient _fitbitClient;


        private HttpRequestMessage _capturedRequest;
        // private string _accessToken;

        public FitbitClientTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();


            _config = new Mock<IConfig>();

            _config.Setup(x => x.FitbitBaseUrl).Returns("https://api.fitbit.com");

            _config.Setup(x => x.FitbitClientId).Returns("123456");

            _config.Setup(x => x.FitbitClientSecret).Returns("secret");
            _config.Setup(x => x.FitbitOAuthRedirectUrl).Returns(fitbitRedirect);

            _logger = new Mock<ILogger>();

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _fitbitClient = new FitbitClient(_httpClient, _config.Object, _logger.Object);
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
        public async Task ShouldGetSleeps()
        {
            //Given
            SetupHttpMessageHandlerMock(fitbitSleepsJson);
            
            var startDate = new DateTime(2011,3,4);

            //When
            //todo also verify use of accessTOken
            var sleeps = await _fitbitClient.Get100DaysOfSleeps(startDate, "accessToken");

            Assert.Equal("https://api.fitbit.com/1.2/user//sleep/date/2011-03-04/2011-06-12.json", _capturedRequest.RequestUri.AbsoluteUri);


            Assert.Equal(1, sleeps.sleep.Count);


        }

        [Fact]
        public async Task ShouldGetFitbitMonthlyHeartRates()
        {
            //Given
            SetupHttpMessageHandlerMock(fitbitHeartRateJson);

            //When
            //            //todo also verify use of accessTOken
            var fitbitActivity = await _fitbitClient.GetMonthOfFitbitHeartRates(new DateTime(2010,1,2), "accessToken");

            Assert.Equal("https://api.fitbit.com/1/user//activities/heart/date/2010-01-02/1m.json", _capturedRequest.RequestUri.AbsoluteUri);

            Assert.Equal(3, fitbitActivity.activitiesHeart.Count);

            Assert.Equal(new DateTime(2015, 8, 4), fitbitActivity.activitiesHeart[0].dateTime);
            Assert.Equal(68, fitbitActivity.activitiesHeart[0].value.restingHeartRate);

            Assert.Equal(new DateTime(2015, 8, 5), fitbitActivity.activitiesHeart[1].dateTime);
            Assert.Equal(69, fitbitActivity.activitiesHeart[1].value.restingHeartRate);
            
            Assert.Equal(new DateTime(2015, 8, 6), fitbitActivity.activitiesHeart[2].dateTime);
            Assert.Equal(70, fitbitActivity.activitiesHeart[2].value.restingHeartRate);


        }

        [Fact]
        public async Task ShouldThrowTooManyRequestsExceptionWhenStatusCode429()
        {
            SetupHttpMessageHandlerMock(fitbitHeartRateJson, (HttpStatusCode)429);

            await Assert.ThrowsAsync<TooManyRequestsException>(() => _fitbitClient.GetMonthOfFitbitHeartRates(new DateTime(), It.IsAny<string>()));
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

            Exception ex = await Assert.ThrowsAsync<Exception>(() => _fitbitClient.GetMonthOfFitbitHeartRates(new DateTime(), It.IsAny<string>()));
            Assert.Contains("status code is 400", ex.Message);
            Assert.Contains("content is I'm a little teapot", ex.Message);

        }
        
        [Fact]
        public async Task ShouldSetTokens()
        {
            //Given
            var authorisationCode = "asdasd234234dfgdfgdf";

            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{
                    ""access_token"": ""aaa"",
                    ""expires_in"": 3600,
                    ""refresh_token"": ""bbb"",
                    ""token_type"": ""Bearer"",
                    ""user_id"": ""26FWFL""
                }")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);

            //When 
            var response = await _fitbitClient.GetTokensWithAuthorizationCode(authorisationCode);

            //Then
            Assert.Equal("aaa", response.access_token);
            Assert.Equal("bbb", response.refresh_token);

            Assert.Equal("https://api.fitbit.com/oauth2/token", capturedRequest.RequestUri.AbsoluteUri);
            Assert.True(capturedRequest.Headers.Contains("Authorization"));
            Assert.Equal(new List<string> { "Basic " + Base64Encode("123456:secret") }, capturedRequest.Headers.GetValues("Authorization"));

            Assert.Equal(HttpMethod.Post, capturedRequest.Method);

            Assert.Contains("grant_type=authorization_code", (await capturedRequest.Content.ReadAsStringAsync()));
            Assert.Contains("client_id=123456", (await capturedRequest.Content.ReadAsStringAsync()));
            Assert.Contains("code=asdasd234234dfgdfgdf", (await capturedRequest.Content.ReadAsStringAsync()));
            Assert.Contains($"redirect_uri={fitbitRedirect}", (await capturedRequest.Content.ReadAsStringAsync()));


        }


        [Fact]
        public async Task ShouldGetAccessToken()
        {
            //Given
            //_tokenService.Setup(x => x.GetFitbitRefreshToken()).Returns(Task.FromResult("abc123"));

            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(@"{
                    ""access_token"": ""aaa2"",
                    ""expires_in"": 3600,
                    ""refresh_token"": ""bbb2"",
                    ""scope"": ""xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"",
                    ""token_type"": ""Bearer"",
                    ""user_id"": ""26FWFL""
                }")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);


            //When
            var response =  await _fitbitClient.GetTokensWithRefreshToken("abc123");

            //Then

            Assert.Equal("aaa2", response.access_token);
            Assert.Equal("bbb2", response.refresh_token);

            Assert.Equal("https://api.fitbit.com/oauth2/token", capturedRequest.RequestUri.AbsoluteUri);
            Assert.True(capturedRequest.Headers.Contains("Authorization"));
            Assert.Equal(new List<string> { "Basic " + Base64Encode("123456:secret") }, capturedRequest.Headers.GetValues("Authorization"));

            Assert.Equal(HttpMethod.Post, capturedRequest.Method);

            Assert.Contains("grant_type=refresh_token", (await capturedRequest.Content.ReadAsStringAsync()));
            Assert.Contains("refresh_token=abc123", (await capturedRequest.Content.ReadAsStringAsync()));

            //_tokenService.Verify(x => x.SaveFitbitAccessToken("aaa2"));
            //_tokenService.Verify(x => x.SaveFitbitRefreshToken("bbb2"));

        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnNonSuccessStatusCodeWhenGetTokensWithRefreshToken()
        {
            //Given
            var refreshToken = "abc123";

            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode)500,
                    Content = new StringContent(@"This is an error 123")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);


            //When
            Exception ex = await Assert.ThrowsAsync<Exception>(() => _fitbitClient.GetTokensWithRefreshToken(refreshToken));

            //Then

            Assert.Contains("status code is : 500", ex.Message);
            Assert.Contains("This is an error 123", ex.Message);
        }

        [Fact]
        public async Task ShouldThrowExceptionOnNonSuccessStatusCodeWhenGetTokensWithAuthorizationCode()
        {
            //Given
            var authorisationCode = "asdasd234234dfgdfgdf";

            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = (HttpStatusCode)404,
                    Content = new StringContent("error abc")
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);



            //When 
            var ex = await Assert.ThrowsAsync<Exception>(() => _fitbitClient.GetTokensWithAuthorizationCode(authorisationCode));

            //Then
            Assert.Contains("status code is : 404", ex.Message);
            Assert.Contains("error abc", ex.Message);
        }































        private readonly string fitbitSleepsJson = @"{
    ""sleep"": [
        {
            ""dateOfSleep"": ""2017-04-02"",
            ""duration"": 123456,
            ""efficiency"": 99,
            ""isMainSleep"": true,
            ""levels"": {
                ""summary"": {
                    ""deep"": {
                        ""count"": 1,
                        ""minutes"": 2,
                        ""thirtyDayAvgMinutes"": 3
                    },
                    ""light"": {
                        ""count"": 4,
                        ""minutes"": 5,
                        ""thirtyDayAvgMinutes"": 6
                    },
                    ""rem"": {
                        ""count"": 7,
                        ""minutes"": 8,
                        ""thirtyDayAvgMinutes"": 9
                    },
                    ""wake"": {
                        ""count"": 10,
                        ""minutes"": 11,
                        ""thirtyDayAvgMinutes"": 12
                    }
                },
                ""data"": [
                    {
                        ""datetime"": ""2017-04-01T23:58:30.000"",
                        ""level"": ""wake"",
                        ""seconds"": 13
                    },
                    {
                        ""datetime"": ""2017-04-02T00:16:30.000"",
                        ""level"": ""light"",
                        ""seconds"": 14
                    }
                ],
                ""shortData"": [
                    {
                        ""datetime"": ""2017-04-02T05:58:30.000"",
                        ""level"": ""wake"",
                        ""seconds"": 15
                    }
                ]
            },
            ""logId"": 16,
            ""minutesAfterWakeup"": 17,
            ""minutesAsleep"": 18,
            ""minutesAwake"": 19,
            ""minutesToFallAsleep"": 20, 
            ""startTime"": ""2017-04-01T23:58:30.000"",
            ""timeInBed"": 21,
            ""type"": ""stages""
        }
    ]
}";

        

        private readonly string fitbitHeartRateJson = @"{
            ""activities-heart"": [
        {
            ""dateTime"": ""2015-08-04"",
            ""value"": {
                ""customHeartRateZones"": [],
                ""heartRateZones"": [
                {
                    ""caloriesOut"": 740.15264,
                    ""max"": 94,
                    ""min"": 30,
                    ""minutes"": 593,
                    ""name"": ""Out of Range""
                },
                {
                    ""caloriesOut"": 249.66204,
                    ""max"": 132,
                    ""min"": 94,
                    ""minutes"": 46,
                    ""name"": ""Fat Burn""
                },
                {
                    ""caloriesOut"": 0,
                    ""max"": 160,
                    ""min"": 132,
                    ""minutes"": 0,
                    ""name"": ""Cardio""
                },
                {
                    ""caloriesOut"": 0,
                    ""max"": 220,
                    ""min"": 160,
                    ""minutes"": 0,
                    ""name"": ""Peak""
                }
                ],
                ""restingHeartRate"": 68
            }
        },
{
            ""dateTime"": ""2015-08-05"",
            ""value"": {
                ""customHeartRateZones"": [],
                ""heartRateZones"": [
                {
                    ""caloriesOut"": 740.15264,
                    ""max"": 94,
                    ""min"": 30,
                    ""minutes"": 593,
                    ""name"": ""Out of Range""
                },
                {
                    ""caloriesOut"": 249.66204,
                    ""max"": 132,
                    ""min"": 94,
                    ""minutes"": 46,
                    ""name"": ""Fat Burn""
                },
                {
                    ""caloriesOut"": 0,
                    ""max"": 160,
                    ""min"": 132,
                    ""minutes"": 0,
                    ""name"": ""Cardio""
                },
                {
                    ""caloriesOut"": 0,
                    ""max"": 220,
                    ""min"": 160,
                    ""minutes"": 0,
                    ""name"": ""Peak""
                }
                ],
                ""restingHeartRate"": 69
            }
        },
        {
            ""dateTime"": ""2015-08-06"",
            ""value"": {
                ""customHeartRateZones"": [],
                ""heartRateZones"": [
                {
                    ""caloriesOut"": 740.15264,
                    ""max"": 94,
                    ""min"": 30,
                    ""minutes"": 593,
                    ""name"": ""Out of Range""
                },
                {
                    ""caloriesOut"": 249.66204,
                    ""max"": 132,
                    ""min"": 94,
                    ""minutes"": 46,
                    ""name"": ""Fat Burn""
                },
                {
                    ""caloriesOut"": 0,
                    ""max"": 160,
                    ""min"": 132,
                    ""minutes"": 0,
                    ""name"": ""Cardio""
                },
                {
                    ""caloriesOut"": 0,
                    ""max"": 220,
                    ""min"": 160,
                    ""minutes"": 0,
                    ""name"": ""Peak""
                }
                ],
                ""restingHeartRate"": 70
            }
        }
        ]
    }";
        
            
    }






}
