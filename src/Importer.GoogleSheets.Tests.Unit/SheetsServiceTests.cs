using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Repositories.Health.Models;
using Utils;
using Xunit;

namespace Importer.GoogleSheets.Tests
{
    public class SheetsServiceTests
    {
        private Mock<IConfig> _config;
        //private Mock<IMapFunctions> _mapFunctions;
        private IMapFunctions _mapFunctions;
        private Mock<IRowMapper> _rowMapper;
        private Mock<ISheetsClient> _sheetsClient;
        private SheetsService _sheetsService;

        public SheetsServiceTests()
        {
            _config = new Mock<IConfig>();
            //_mapFunctions = new Mock<IMapFunctions>();
            _mapFunctions = new MapFunctions();
            _rowMapper = new Mock<IRowMapper>();
            _sheetsClient = new Mock<ISheetsClient>();

            _sheetsService = new SheetsService(_config.Object,_rowMapper.Object, _mapFunctions, _sheetsClient.Object);
        }

        [Fact]
        public async Task ShouldGetHistoricDrinks()
        {
            //Given
            var spreadsheetId = "sdfdsfsdf";
            var range = "A1B1";

            var someRows = new List<IList<object>>();

            var someDrinks = new List<Drink>
            {
                new Drink
                {
                    CreatedDate = new DateTime(2010,1,1),
                    Units = 11
                },
                new Drink
                {
                    CreatedDate = new DateTime(2010, 1, 1),
                    Units = 2
                },
                new Drink
                {
                    CreatedDate = new DateTime(2010, 1, 2),
                    Units = 3
                }
            };
            
            _config.Setup(x => x.HistoricAlcoholSpreadsheetId).Returns(spreadsheetId);
            _config.Setup(x => x.DrinksRange).Returns(range);

            _sheetsClient.Setup(x => x.GetRows(spreadsheetId, range)).Returns(someRows);
            
            _rowMapper.Setup(x => x.Get(someRows, _mapFunctions.MapRowToDrink)).Returns(someDrinks);
            
            //When
            var drinks = _sheetsService.GetHistoricDrinks();

            //Then
            Assert.Equal(2,drinks.Count);
            Assert.Contains(drinks, x => x.CreatedDate == new DateTime(2010, 1, 1) && x.Units == 13);
            Assert.Contains(drinks, x => x.CreatedDate == new DateTime(2010, 1, 2) && x.Units == 3);

        }

        [Fact]
        public async Task ShouldGetDrinks()
        {
            //Given
            var spreadsheetId = "sdfsdf234234123423423";
            var range = "A1B1";

            var someRows = new List<IList<object>>();

            var someDrinks = new List<Drink>
            {
                new Drink
                {
                    CreatedDate = new DateTime(2010,1,1),
                    Units = 11
                },
                new Drink
                {
                    CreatedDate = new DateTime(2010, 1, 1),
                    Units = 2
                },
                new Drink
                {
                    CreatedDate = new DateTime(2010, 1, 2),
                    Units = 3
                }
            };

            _config.Setup(x => x.AlcoholSpreadsheetId).Returns(spreadsheetId);
            _config.Setup(x => x.DrinksRange).Returns(range);

            _sheetsClient.Setup(x => x.GetRows(spreadsheetId, range)).Returns(someRows);

            _rowMapper.Setup(x => x.Get(someRows, _mapFunctions.MapRowToDrink)).Returns(someDrinks);

            //When
            var drinks = _sheetsService.GetDrinks();

            //Then
            Assert.Equal(2, drinks.Count);
            Assert.Contains(drinks, x => x.CreatedDate == new DateTime(2010, 1, 1) && x.Units == 13);
            Assert.Contains(drinks, x => x.CreatedDate == new DateTime(2010, 1, 2) && x.Units == 3);

        }

        [Fact]
        public async Task ShouldGetExercises()
        {
            //Given
            var spreadsheetId = "sadasdasdassdasd";
            var range = "A1B1";

            var someRows = new List<IList<object>>();

            var someExercises = new List<Exercise>
            {
                new Exercise
                {
                    CreatedDate = new DateTime(2010,1,1),
                    Metres = 1
                },
                new Exercise
                {
                    CreatedDate = new DateTime(2010, 1, 1),
                    Metres = 2
                },
                new Exercise
                {
                    CreatedDate = new DateTime(2010, 1, 2),
                    Metres = 3
                }
            };

            _config.Setup(x => x.ExerciseSpreadsheetId).Returns(spreadsheetId);
            _config.Setup(x => x.ExercisesRange).Returns(range);

            _sheetsClient.Setup(x => x.GetRows(spreadsheetId, range)).Returns(someRows);

            _rowMapper.Setup(x => x.Get(someRows, _mapFunctions.MapRowToExercise)).Returns(someExercises);

            //When
            var exercises = _sheetsService.GetExercises();

            //Then
            Assert.Equal(3, exercises.Count);
            Assert.Contains(exercises, x => x.Metres == 1);
            Assert.Contains(exercises, x => x.Metres == 2);
            Assert.Contains(exercises, x => x.Metres == 3);


        }
    }
}