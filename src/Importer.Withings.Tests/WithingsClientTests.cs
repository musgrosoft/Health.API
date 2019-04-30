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

namespace Importer.Withings.Tests
{
    public class WithingsClientTests
    {
        Uri _capturedUri = new Uri("http://www.null.com");
        private string withingsClientId = "12312jhjskdh937ey19d";
        private string withingsClientSecret = "fdjs982rhdgdsfogiuhd";
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

            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(nokiaContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedRequest = h);

            _httpClient = new HttpClient(_httpMessageHandler.Object);
            _logger = new Mock<ILogger>();
//            _withingsAuthenticator = new Mock<IWithingsAuthenticator>();
//            _withingsAuthenticator.Setup(x => x.GetAccessToken()).Returns(Task.FromResult("abc123"));

            _config = new Mock<IConfig>();
            _config.Setup(x => x.WithingsClientId).Returns(withingsClientId);
            _config.Setup(x => x.WithingsClientSecret).Returns(withingsClientSecret);


            _withingsClient = new WithingsClient(_httpClient, _logger.Object,  _config.Object);
        }


        [Fact]
        public async Task ShouldGetTokensByAuthorisationCode()
        {
            //Given
            string authorizationCode = "qwe321";


            //When

            var tokenResponse = await _withingsClient.GetTokensByAuthorisationCode(authorizationCode);

            //Then
            Assert.Equal("https://wbsapi.withings.net/oauth2/token", _capturedUri.AbsoluteUri);

            Assert.Contains("grant_type=authorization_code", (await _capturedRequest.Content.ReadAsStringAsync()));

            Assert.Contains($"client_id={withingsClientId}", (await _capturedRequest.Content.ReadAsStringAsync()));
            Assert.Contains($"client_secret={withingsClientSecret}", (await _capturedRequest.Content.ReadAsStringAsync()));
            //Assert.Contains($"redirect_urei={authorizationCode}", (await _capturedRequest.Content.ReadAsStringAsync()));



            Assert.Contains($"code={authorizationCode}", (await _capturedRequest.Content.ReadAsStringAsync()));
        }


        [Fact]
        public async Task ShouldGetMeasureGroups()
        {
            var measuregrps = await _withingsClient.GetMeasureGroups("abc123");

            Assert.Equal("https://wbsapi.withings.net/measure?action=getmeas&access_token=abc123", _capturedUri.AbsoluteUri);
            Assert.Equal(8, measuregrps.Count());
            Assert.Contains(measuregrps, x => x.date == 1526015332 && x.measures.Exists(a => a.value == 83000 && a.type == 9 && a.unit == -3));
            //Assert.Contains(measuregrps, x => x.Kg == 90.261 && x.CreatedDate == new DateTime(2018, 5, 10, 5, 4, 42));
        }


        private string nokiaContent = @"{
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
