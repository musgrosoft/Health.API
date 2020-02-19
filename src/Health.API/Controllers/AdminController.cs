using System;
using System.Threading.Tasks;
using HealthAPI.Importer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Health.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly HealthContext _healthContext;
        private readonly IImporter _importer;

        public AdminController(HealthContext healthContext, IImporter importer)
        {
            _healthContext = healthContext;
            _importer = importer;
        }

        [HttpGet]
        [Route("EnsureDeleted")]
        public IActionResult EnsureDeleted()
        {
            var result = _healthContext.Database.EnsureDeleted();

            return Ok(result);
        }

        [HttpGet]
        [Route("EnsureCreated")]
        public IActionResult EnsureCreated()
        {
            var result = _healthContext.Database.EnsureCreated();

            return Ok(result);
        }

        [HttpGet]
        [Route("Populate")]
        public async Task<IActionResult> Populate()
        {
            await _importer.ImportGoogleSheetsDrinks();
            await _importer.ImportGoogleSheetsExercises();
            await _importer.ImportGoogleSheetsGarminIntensityMinutes();
            await _importer.ImportGoogleSheetsGarminRestingHeartRates();
            await _importer.ImportGoogleSheetsTargets();

            await _importer.ImportWithingsBloodPressures();
            await _importer.ImportWithingsWeights();

            await _importer.ImportFitbitRestingHeartRates();
            await _importer.ImportFitbitSleepSummaries();

            // await Task.WhenAll(
            // _importer.ImportGoogleSheetsDrinks(),
            // _importer.ImportGoogleSheetsExercises(),
            // _importer.ImportGoogleSheetsGarminIntensityMinutes(),
            // _importer.ImportGoogleSheetsGarminRestingHeartRates(),
            // _importer.ImportGoogleSheetsTargets(),
            //
            // _importer.ImportWithingsBloodPressures(),
            // _importer.ImportWithingsWeights(),
            //
            // _importer.ImportFitbitRestingHeartRates(),
            // _importer.ImportFitbitSleepSummaries()
            //     );

            return Ok("ok");
        }


        [HttpGet]
        [Route("RefreshViews")]
        public IActionResult RefreshViews()
        {
            //Task.Run(async () => await logger.LogMessageAsync("DB WAS JUST RECREATED, START RUNNING IN VIEWS"));

            _healthContext.Database.SetCommandTimeout(180);

            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/Drop_All_Views.sql"));

            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Alcohol_Daily.sql"));
            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Alcohol_Monthly.sql"));
            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Alcohol_Weekly.sql"));

            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_BloodPressures_Daily.sql"));

            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Exercises_Daily.sql"));
            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Exercises_Daily_2.sql"));

            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Exercises_Daily_Agg.sql"));
            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Exercises_Monthly.sql"));
            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Exercises_Weekly.sql"));

            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Weights_Daily.sql"));

            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Sleeps_Daily.sql"));
            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Sleeps_Monthly.sql"));
            _healthContext.Database.ExecuteSqlRaw(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Sleeps_Weekly.sql"));

            //Task.Run(async () => await logger.LogMessageAsync("FINISHED RUNNING IN VIEWS"));

            return Ok("ok");
        }
    }
}