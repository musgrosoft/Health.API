using System;
using System.Collections.Generic;
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
        
        private Mock<ISheetsService> _googleClient;
        private Mock<IHealthService> _healthService;
        private Mock<ICalendar> _calendar;

        public GoogleSheetsControllerTests()
        {
            _logger = new Mock<ILogger>();
            _googleClient = new Mock<ISheetsService>();
            _healthService = new Mock<IHealthService>();
            _calendar = new Mock<ICalendar>();
     
            _googleSheetsController = new GoogleSheetsController(_logger.Object, _googleClient.Object, _healthService.Object, _calendar.Object);
        }

        //[Fact]
        //public void ShouldMigrateUnits()
        //{

        //    var someAlcoholIntakes = new List<Drink>
        //    {
        //        new Drink {CreatedDate = new DateTime(2018,1,1),Units = 1},
        //        new Drink {CreatedDate = new DateTime(2018,1,2),Units = 2},
        //        new Drink {CreatedDate = new DateTime(2018,1,3),Units = 3}
        //    };
        //    _googleClient.Setup(x => x.GetDrinks()).Returns(someAlcoholIntakes);
            
        //    _googleSheetsController.MigrateUnits();

        //    _healthService.Verify(x=>x.UpsertAlcoholIntakes(someAlcoholIntakes), Times.Once);
        //    _logger.Verify(x=>x.LogMessageAsync("GOOGLE SHEETS : Migrate Units"));

        //}
    }
}