using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Services.Withings;

namespace Services.Tests.Withings
{
    public class WithingsClientTests
    {
        Uri _capturedUri = new Uri("http://www.null.com");
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private HttpClient _httpClient;
        private WithingsClient _withingsClient;

        public WithingsClientTests()
        {
            _httpMessageHandler = new Mock<HttpMessageHandler>();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(nokiaContent)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedUri = h.RequestUri); 

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _withingsClient = new WithingsClient(_httpClient, null, null);
        }

        //[Fact]
        //public async Task ShouldGetWeights()
        //{
        //    var weights = await _nokiaClient.GetWeights(new DateTime());

        //    Assert.Equal("http://api.health.nokia.com/measure?action=getmeas&oauth_consumer_key=ebb1cbd42bb69687cb85ccb20919b0ff006208b79c387059123344b921837d8d&oauth_nonce=742bef6a3da52fbf004573d18b8f04cf&oauth_signature=cgO95H%2Fg2qx0VQ9ma2k8qeHronM%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1503326610&oauth_token=7f027003b78369d415bd0ee8e91fdd43408896616108b72b97fd7c153685f&oauth_version=1.0&userid=8792669", _capturedUri.AbsoluteUri);
        //    Assert.Equal(2, weights.Count());
        //    Assert.Contains(weights, x => x.Kg == 90.435 && x.CreatedDate == new DateTime(2018,5,11,4,57,27));
        //    Assert.Contains(weights, x => x.Kg == 90.261 && x.CreatedDate == new DateTime(2018,5,10,5,4,42));
        //}


        //[Fact]
        //public async Task ShouldGetBloodPressureMeasures()
        //{
        //    var bps = await _nokiaClient.GetBloodPressures(new DateTime());

        //    Assert.Equal("http://api.health.nokia.com/measure?action=getmeas&oauth_consumer_key=ebb1cbd42bb69687cb85ccb20919b0ff006208b79c387059123344b921837d8d&oauth_nonce=742bef6a3da52fbf004573d18b8f04cf&oauth_signature=cgO95H%2Fg2qx0VQ9ma2k8qeHronM%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1503326610&oauth_token=7f027003b78369d415bd0ee8e91fdd43408896616108b72b97fd7c153685f&oauth_version=1.0&userid=8792669",_capturedUri.AbsoluteUri);
        //    Assert.Equal(3, bps.Count());
        //    Assert.Contains(bps, x => x.Systolic == 130 && x.Diastolic == 83 && x.CreatedDate == new DateTime(2018, 5, 11, 5, 8, 52));
        //    Assert.Contains(bps, x => x.Systolic == 127 && x.Diastolic == 80 && x.CreatedDate == new DateTime(2018, 5, 10, 5, 20, 43));
        //    Assert.Contains(bps, x => x.Systolic == 134 && x.Diastolic == 81 && x.CreatedDate == new DateTime(2018, 5, 9, 5, 12, 41));
        //}

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
