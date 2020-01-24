using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Health.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly HealthContext _healthContext;

        public DatabaseController(HealthContext healthContext)
        {
            _healthContext = healthContext;
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