using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Utils;
using Xunit;

namespace GoogleSheets.Tests.Unit
{
    public class SheetsServiceTests
    {
        private readonly Mock<IConfig> _config;
        private readonly SheetsService _sheetsService;
        private readonly Mock<HttpMessageHandler> _httpMessageHandler;
        private HttpRequestMessage _capturedRequest;
        private readonly HttpClient _httpClient;
        private readonly Mock<ICalendar> _calendar;
        private readonly Mock<ILogger> _logger;

        public SheetsServiceTests()
        {
            _config = new Mock<IConfig>();
            _config.SetupGet(x => x.ExerciseSpreadsheetId).Returns("ExerciseSpreadsheetId");
            _config.SetupGet(x => x.DrinksSpreadsheetId).Returns("DrinksSpreadsheetId");
            _config.SetupGet(x => x.TargetsSpreadsheetId).Returns("TargetsSpreadsheetId");

            _httpMessageHandler = new Mock<HttpMessageHandler>();

            _capturedRequest = new HttpRequestMessage();

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _calendar = new Mock<ICalendar>();

            _logger = new Mock<ILogger>();

            SetupHttpMessageHandlerMock("");

            _sheetsService = new SheetsService(_config.Object, _httpClient, _calendar.Object, _logger.Object);
        }

        private void SetupHttpMessageHandlerMock(string content)
        {
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => _capturedRequest = h);
        }

        [Fact]
        public async Task ShouldSendCorrectRequestWhenGettingDrinks()
        {
            //When
            await _sheetsService.GetDrinks(new DateTime(2000,1,1));

            //Then
            Assert.Equal(HttpMethod.Get, _capturedRequest.Method);
            Assert.Equal("https://docs.google.com/spreadsheets/d/DrinksSpreadsheetId/gviz/tq?tqx=out:csv&sheet=Sheet1", _capturedRequest.RequestUri.AbsoluteUri);
        }

        [Fact]
        public async Task ShouldSendCorrectRequestWhenGettingExercises()
        {
            //When
            await _sheetsService.GetExercises(new DateTime(2000, 1, 1));

            //Then
            Assert.Equal(HttpMethod.Get, _capturedRequest.Method);
            Assert.Equal("https://docs.google.com/spreadsheets/d/ExerciseSpreadsheetId/gviz/tq?tqx=out:csv&sheet=Sheet1", _capturedRequest.RequestUri.AbsoluteUri);
        }

        [Fact]
        public async Task ShouldSendCorrectRequestWhenGettingTargets()
        {
            //When
            await _sheetsService.GetTargets();

            //Then
            Assert.Equal(HttpMethod.Get, _capturedRequest.Method);
            Assert.Equal("https://docs.google.com/spreadsheets/d/TargetsSpreadsheetId/gviz/tq?tqx=out:csv&sheet=Sheet1", _capturedRequest.RequestUri.AbsoluteUri);
        }

        public void ShouldAggregateDrinks()
        {

            //*********************************************TODO*******************************************************************************
        }

        [Fact]
        public async Task ShouldParseContentWhenGettingDrinks()
        {
            var fromDate = new DateTime(2019,11,19);
            var today = new DateTime(2019,11,24);

            _calendar.Setup(x => x.Now()).Returns(today);
            
            var content = $@"""Day of week"",""CreatedDate"","""","""","""",""Units"","""","""","""","""","""","""",""""
            ""Thursday"",""19-Nov-2019"","""","""","""",""7.9"","""","""","""","""","""","""",""""
            ""Friday"",""20-Nov-2019"","""","""","""",""4.9"","""","""","""","""","""","""",""""
            ""Saturday"",""21-Nov-2019"","""","""","""",""0"","""","""","""","""","""","""",""""
            ""Sunday"",""22-Nov-2019"","""","""","""",""123"","""","""","""","""","""","""",""""
            ""Monday"",""23-Nov-2019"","""","""","""",""234"","""","""","""","""","""","""",""""
            ""Tuesday"",""24-Nov-2019"","""","""","""",""456"","""","""","""","""","""","""",""""".Replace("\r\n", "\n");

            SetupHttpMessageHandlerMock(content);

            var drinks = (await _sheetsService.GetDrinks(fromDate)).ToList();

            Assert.Equal(6,drinks.Count);

            Assert.Equal(new DateTime(2019, 11, 19), drinks[0].CreatedDate);
            Assert.Equal(7.9, drinks[0].Units);
            Assert.Equal(new DateTime(2019, 11, 24), drinks[5].CreatedDate);
            Assert.Equal(456, drinks[5].Units);


        }

        [Fact]
        public async Task ShouldParseContentWhenGettingExercises()
        {
            var fromDate = new DateTime(2019, 9, 9);
            var today = new DateTime(2019, 11, 13);

            _calendar.Setup(x => x.Now()).Returns(today);

            var content = $@"""CreatedDate"",""Metres"",""Time"",""Description"",""TotalSeconds"",""Notes"","""",""""
            ""9-Sep-2019"",""3404"",""0:15:00"",""ergo"",""900"","""","""",""""
            ""9-Sep-2019"",""8000"",""0:15:00"",""airbike"",""900"","""","""",""""
            ""14-Sep-2019"",""0"",""0:32:00"",""swim"",""1920"","""","""",""""
            ""15-Sep-2019"",""6000"",""0:36:06"",""road"",""2166"","""","""",""""
            ""17-Sep-2019"",""3501"",""0:15:00"",""ergo"",""900"","""","""",""""
            ""17-Sep-2019"",""8200"",""0:15:00"",""airbike"",""900"","""","""",""""
            ""19-Sep-2019"",""6000"",""0:34:17"",""treadmill"",""2057"","""","""",""""
            ""21-Sep-2019"",""1250"",""0:35:00"",""Swim"",""2100"",""50 x 25m pool"","""",""""
            ""23-Sep-2019"",""8400"",""0:15:00"",""airbike"",""900"","""","""",""""
            ""28-Sep-2019"",""1500"",""0:39:00"",""swim"",""2340"","""","""",""""
            ""30-Sep-2019"",""5000"",""0:30:00"",""treadmill"",""1800"","""","""",""""
            ""9-Nov-2019"",""1000"",""0:40:00"",""swim"",""2400"","""","""",""""
            ""13-Nov-2019"",""2616"",""0:15:00"",""treadmill"",""900"","""","""",""""
            """","""","""","""",""0"","""","""",""""
            """","""","""","""",""0"","""","""",""""
            """","""","""","""",""0"","""","""",""""
            """","""","""","""",""0"","""","""",""""
            """","""","""","""",""0"","""","""",""""
            """","""","""","""",""0"","""","""",""""".Replace("\r\n", "\n");

            SetupHttpMessageHandlerMock(content);

            var exercises = (await _sheetsService.GetExercises(fromDate)).ToList();

            Assert.Equal(13, exercises.Count);

            Assert.Equal(new DateTime(2019,9,9), exercises[0].CreatedDate );
            Assert.Equal("ergo", exercises[0].Description);
            Assert.Equal(3404, exercises[0].Metres);
            Assert.Equal(900, exercises[0].TotalSeconds);

            Assert.Equal(new DateTime(2019, 9, 9), exercises[1].CreatedDate);
            Assert.Equal("airbike", exercises[1].Description);
            Assert.Equal(8000, exercises[1].Metres);
            Assert.Equal(900, exercises[1].TotalSeconds);

            Assert.Equal(new DateTime(2019, 9, 19), exercises[6].CreatedDate);
            Assert.Equal("treadmill", exercises[6].Description);
            Assert.Equal(6000, exercises[6].Metres);
            Assert.Equal(2057, exercises[6].TotalSeconds);
        }


        [Fact]
        public async Task ShouldParseContentWhenGettingTargets()
        {
            var content = 
$@"""Date"",""Kg"",""Diastolic"",""Systolic"",""Units"",""CardioMinutes"",""MetresErgo15Minutes"",""MetresTreadmill30Minutes""
""1-May-2018"",""90.74"",""80"",""120"",""4"",""11"","""",""""
""2-May-2018"",""90.72333333"",""80"",""120"",""4"",""11"",""123"",""456""
""3-May-2018"",""90.70666667"",""80"",""120"",""4"",""11"","""",""""
""4-May-2018"",""90.69"",""80"",""120"",""4"",""11"","""",""""
""5-May-2018"",""90.67333333"",""80"",""120"",""4"",""11"","""",""""".Replace("\r\n","\n");


            SetupHttpMessageHandlerMock(content);


            var targets = (await _sheetsService.GetTargets()).OrderBy(x => x.Date).ToList();

            Assert.Equal(5, targets.Count());

            Assert.Equal(new DateTime(2018, 5, 2), targets[1].Date);
            Assert.Equal(90.72333333, targets[1].Kg);
            Assert.Equal(80, targets[1].Diastolic);
            Assert.Equal(120, targets[1].Systolic);
            Assert.Equal(4, targets[1].Units);
            Assert.Equal(11, targets[1].CardioMinutes);
            Assert.Equal(123, targets[1].MetresErgo15Minutes);
            Assert.Equal(456, targets[1].MetresTreadmill30Minutes);

            Assert.Equal(new DateTime(2018, 5, 4), targets[3].Date);
            Assert.Equal(90.69, targets[3].Kg);
            Assert.Equal(80, targets[3].Diastolic);
            Assert.Equal(120, targets[3].Systolic);
            Assert.Equal(4, targets[3].Units);
            Assert.Equal(11, targets[3].CardioMinutes);
            Assert.Equal(0, targets[3].MetresErgo15Minutes);
            Assert.Equal(0, targets[3].MetresTreadmill30Minutes);

        }




    }
}