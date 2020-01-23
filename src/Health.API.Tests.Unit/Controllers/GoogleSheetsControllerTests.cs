using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleSheets;
using HealthAPI.Controllers;
using Moq;
using Repositories.Health.Models;
using Services.Health;
using Utils;
using Xunit;

namespace HealthAPI.Tests.Unit.Controllers.Migration
{
    public class GoogleSheetsControllerTests
    {
        private readonly Mock<ILogger> _logger;
        private readonly GoogleSheetsController _googleSheetsController;
        
        private readonly Mock<ISheetsService> _googleClient;
        private readonly Mock<IHealthService> _healthService;
        

        public GoogleSheetsControllerTests()
        {
            _logger = new Mock<ILogger>();
            _googleClient = new Mock<ISheetsService>();
            _healthService = new Mock<IHealthService>();
            
     
            _googleSheetsController = new GoogleSheetsController(_logger.Object, _googleClient.Object, _healthService.Object);
        }

        [Fact]
        public async Task ShouldImportDrinks()
        {

            IEnumerable<Drink> someAlcoholIntakes = new List<Drink>
            {
                new Drink {CreatedDate = new DateTime(2018,1,1),Units = 1},
                new Drink {CreatedDate = new DateTime(2018,1,2),Units = 2},
                new Drink {CreatedDate = new DateTime(2018,1,3),Units = 3}
            };

            var date = new DateTime(2001, 2, 3);
            _healthService.Setup(x => x.GetLatestDrinkDate(It.IsAny<DateTime>())).Returns(date);

            _googleClient.Setup(x=>x.GetDrinks(date.AddDays(-10))).Returns(Task.FromResult(someAlcoholIntakes));
            
            await _googleSheetsController.ImportDrinks();

            _healthService.Verify(x=>x.UpsertAsync(someAlcoholIntakes), Times.Once);
            //_logger.Verify(x=>x.LogMessageAsync("GOOGLE SHEETS : Migrate Units"));

        }

        [Fact]
        public async Task ShouldImportExercises()
        {
            IEnumerable<Exercise> someExercises = new List<Exercise>
            {
                new Exercise {CreatedDate = new DateTime(2018,1,1),Metres = 1},
                new Exercise {CreatedDate = new DateTime(2018,1,2),Metres = 2},
                new Exercise {CreatedDate = new DateTime(2018,1,3),Metres = 3}
            };

            var date = new DateTime(2001, 2, 3);
            _healthService.Setup(x => x.GetLatestExerciseDate(It.IsAny<DateTime>())).Returns(date);

            _googleClient.Setup(x => x.GetExercises(date.AddDays(-10))).Returns(Task.FromResult(someExercises));

            await _googleSheetsController.ImportExercises();

            _healthService.Verify(x => x.UpsertAsync(someExercises), Times.Once);

        }

        [Fact]
        public async Task ShouldImportTargets()
        {
            IEnumerable<Target> someTargets = new List<Target>
            {
                new Target {Date = new DateTime(2001,2,3), Kg = 123}
            };

            _googleClient.Setup(x => x.GetTargets()).Returns(Task.FromResult(someTargets));

            await _googleSheetsController.ImportTargets();

            _healthService.Verify(x=>x.UpsertAsync(someTargets));

        }
    }
}