using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GoogleSheets;
using Moq;
using Moq.Protected;
using Repositories.Health.Models;
using Utils;
using Xunit;

namespace GoogleSheets.Tests.Unit
{
    public class SheetsServiceTests
    {
        private Mock<IConfig> _config;
//        //private Mock<IMapFunctions> _mapFunctions;
//        private IMapFunctions _mapFunctions;
//        private Mock<IRowMapper> _rowMapper;
        //private Mock<ISheetsClient> _sheetsClient;
        private SheetsService _sheetsService;
        private Mock<HttpMessageHandler> _httpMessageHandler;
        private HttpRequestMessage _capturedRequest;
        private HttpClient _httpClient;
        private Mock<ICalendar> _calendar;

        public SheetsServiceTests()
        {
            _config = new Mock<IConfig>();
            //            //_mapFunctions = new Mock<IMapFunctions>();
            //            _mapFunctions = new MapFunctions();
            //            _rowMapper = new Mock<IRowMapper>();
            //_sheetsClient = new Mock<ISheetsClient>();

            _httpMessageHandler = new Mock<HttpMessageHandler>();

            //_httpMessageHandler = new Mock<HttpMessageHandler> { CallBase = true };

            _capturedRequest = new HttpRequestMessage();

            _httpClient = new HttpClient(_httpMessageHandler.Object);

            _calendar = new Mock<ICalendar>();


            _sheetsService = new SheetsService(_config.Object, _httpClient, _calendar.Object);
        }

        public void ShouldSendCorrectRequestWhenGettingDrinks()
        { }

        public void ShouldAggregateDrinks()
        { }

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
            ""Tuesday"",""24-Nov-2019"","""","""","""",""456"","""","""","""","""","""","""",""""";


            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);

            var drinks = (await _sheetsService.GetDrinks(fromDate)).ToList();

            Assert.Equal(6,drinks.Count());
            Assert.Equal(new DateTime(2019,11,19), drinks[0].CreatedDate);


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
            """","""","""","""",""0"","""","""",""""";


            HttpRequestMessage capturedRequest = new HttpRequestMessage();
            _httpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content)
                })).Callback<HttpRequestMessage, CancellationToken>((h, c) => capturedRequest = h);

            var exercises = await _sheetsService.GetExercises(fromDate);

            Assert.Equal(13, exercises.Count());



        }


        [Fact]
        public async Task shouldDoThing()
        {
            var targetsCsvUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vR39rv_K6Lx1Gn-i8BbzOicLdZNm_whlpFgnhGxDC3nh1PUCY04j2Aa3JKN6TU1MS7O8QHEZ7Gn85nE/pub?gid=0&single=true&output=csv";

            var http = new HttpClient();

            var response = await http.GetAsync(targetsCsvUrl);

            var csv = await response.Content.ReadAsStringAsync();

            var targets = csv.FromCSVToIEnumerableOf<Target>();

            Assert.True(targets.Count() > 2);


        }

        //[Fact]
        //public async Task ShouldGetHistoricDrinks()
        //{
        //    //Given
        //    var spreadsheetId = "sdfdsfsdf";
        //    var range = "A1B1";

        //    var someRows = new List<IList<object>>();

        //    var someDrinks = new List<Drink>
        //    {
        //        new Drink
        //        {
        //            CreatedDate = new DateTime(2010,1,1),
        //            Units = 11
        //        },
        //        new Drink
        //        {
        //            CreatedDate = new DateTime(2010, 1, 1),
        //            Units = 2
        //        },
        //        new Drink
        //        {
        //            CreatedDate = new DateTime(2010, 1, 2),
        //            Units = 3
        //        }
        //    };
            
        //    _config.Setup(x => x.HistoricAlcoholSpreadsheetId).Returns(spreadsheetId);
        //    _config.Setup(x => x.DrinksRange).Returns(range);

        //    _sheetsClient.Setup(x => x.GetRows(spreadsheetId, range)).Returns(someRows);
            
        //    _rowMapper.Setup(x => x.Get(someRows, _mapFunctions.MapRowToDrink)).Returns(someDrinks);
            
        //    //When
        //    var drinks = _sheetsService.GetHistoricDrinks();

        //    //Then
        //    Assert.Equal(2,drinks.Count);
        //    Assert.Contains(drinks, x => x.CreatedDate == new DateTime(2010, 1, 1) && x.Units == 13);
        //    Assert.Contains(drinks, x => x.CreatedDate == new DateTime(2010, 1, 2) && x.Units == 3);

        //}

        //[Fact]
        //public async Task ShouldGetDrinks()
        //{
        //    //Given
        //    var spreadsheetId = "sdfsdf234234123423423";
        //    var range = "A1B1";

        //    var someRows = new List<IList<object>>();

        //    var someDrinks = new List<Drink>
        //    {
        //        new Drink
        //        {
        //            CreatedDate = new DateTime(2010,1,1),
        //            Units = 11
        //        },
        //        new Drink
        //        {
        //            CreatedDate = new DateTime(2010, 1, 1),
        //            Units = 2
        //        },
        //        new Drink
        //        {
        //            CreatedDate = new DateTime(2010, 1, 2),
        //            Units = 3
        //        }
        //    };

        //    _config.Setup(x => x.AlcoholSpreadsheetId).Returns(spreadsheetId);
        //    _config.Setup(x => x.DrinksRange).Returns(range);

        //    _sheetsClient.Setup(x => x.GetRows(spreadsheetId, range)).Returns(someRows);

        //    _rowMapper.Setup(x => x.Get(someRows, _mapFunctions.MapRowToDrink)).Returns(someDrinks);

        //    //When
        //    var drinks = _sheetsService.GetDrinks();

        //    //Then
        //    Assert.Equal(2, drinks.Count);
        //    Assert.Contains(drinks, x => x.CreatedDate == new DateTime(2010, 1, 1) && x.Units == 13);
        //    Assert.Contains(drinks, x => x.CreatedDate == new DateTime(2010, 1, 2) && x.Units == 3);

        //}

        //[Fact]
        //public async Task ShouldGetExercises()
        //{
        //    //Given
        //    var spreadsheetId = "sadasdasdassdasd";
        //    var range = "A1B1";

        //    var someRows = new List<IList<object>>();

        //    var someExercises = new List<Exercise>
        //    {
        //        new Exercise
        //        {
        //            CreatedDate = new DateTime(2010,1,1),
        //            Metres = 1
        //        },
        //        new Exercise
        //        {
        //            CreatedDate = new DateTime(2010, 1, 1),
        //            Metres = 2
        //        },
        //        new Exercise
        //        {
        //            CreatedDate = new DateTime(2010, 1, 2),
        //            Metres = 3
        //        }
        //    };

        //    _config.Setup(x => x.ExerciseSpreadsheetId).Returns(spreadsheetId);
        //    _config.Setup(x => x.ExercisesRange).Returns(range);

        //    _sheetsClient.Setup(x => x.GetRows(spreadsheetId, range)).Returns(someRows);

        //    _rowMapper.Setup(x => x.Get(someRows, _mapFunctions.MapRowToExercise)).Returns(someExercises);

        //    //When
        //    var exercises = _sheetsService.GetExercises();

        //    //Then
        //    Assert.Equal(3, exercises.Count);
        //    Assert.Contains(exercises, x => x.Metres == 1);
        //    Assert.Contains(exercises, x => x.Metres == 2);
        //    Assert.Contains(exercises, x => x.Metres == 3);


        //}
    }
}