using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
            var rows = _sheetsClient.GetRows(_config.DrinksSpreadsheetId, _config.DrinksRange);

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

        public async Task<IEnumerable<Target>> GetTargets()
        {
            var targetsCsvUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vR39rv_K6Lx1Gn-i8BbzOicLdZNm_whlpFgnhGxDC3nh1PUCY04j2Aa3JKN6TU1MS7O8QHEZ7Gn85nE/pub?gid=0&single=true&output=csv";

            var http = new HttpClient();

            var response = await http.GetAsync(targetsCsvUrl);

            var csv = await response.Content.ReadAsStringAsync();

            var lines = csv.Split("/r/n");

            var targets = lines.Skip(1).Select((x) =>
            {
                try
                {
                    var values = x.Split(',');

                    return new Target
                    {
                        Date = DateTime.Parse(values[0]),
                        Kg = double.Parse(values[1])
                    };
                }
                catch (Exception ex)
                {

                }

                return new Target {Date = DateTime.Now.AddDays(1)};

            });

            return targets;

        }

    }
}