using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Health.Models;
using Utils;

namespace GoogleSheets
{
    public class SheetsService : ISheetsService
    {
        private readonly IConfig _config;
        private readonly IRowMapper _rowMapper;
        private readonly IMapFunctions _mapFunctions;
        private readonly ISheetsClient _sheetsClient;
        static string[] Scopes = { Google.Apis.Sheets.v4.SheetsService.Scope.Spreadsheets };
        //static string ApplicationName = "sheetreader";

        public SheetsService(IConfig config, IRowMapper rowMapper, IMapFunctions mapFunctions, ISheetsClient sheetsClient)
        {
            _config = config;
            _rowMapper = rowMapper;
            _mapFunctions = mapFunctions;
            _sheetsClient = sheetsClient;
        }

        public List<Drink> GetDrinks(DateTime fromDate)
        {
            var rows = _sheetsClient.GetRows(_config.HistoricAlcoholSpreadsheetId, _config.DrinksRange);

            var drinks = _rowMapper.Get<Drink>(rows, _mapFunctions.MapRowToDrink);

            return drinks
                .Where(x => x.CreatedDate >= fromDate)
                .GroupBy(x => x.CreatedDate)
                .Select(x => new Drink
                {
                    CreatedDate = x.Key,
                    Units = x.Sum(y => y.Units)
                })                
                .ToList();

        }

        public List<Exercise> GetExercises(DateTime fromDate)
        {
            var rows = _sheetsClient.GetRows(_config.ExerciseSpreadsheetId, _config.ExercisesRange);

            var exercises = _rowMapper.Get<Exercise>(rows, _mapFunctions.MapRowToExercise);

            return exercises.Where(x => x.CreatedDate > fromDate).ToList();
        }

    }
}