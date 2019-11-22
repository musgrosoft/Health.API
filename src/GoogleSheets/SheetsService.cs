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

        private readonly HttpClient _httpClient;

        public SheetsService(IConfig config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Drink>> GetDrinks(DateTime fromDate)
        {
            var response = await _httpClient.GetAsync(_config.DrinksCsvUrl);

            var csv = await response.Content.ReadAsStringAsync();

            var drinks = csv.FromCSVToIEnumerableOf<Drink>();

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

        public async Task<IEnumerable<Exercise>> GetExercises(DateTime fromDate)
        {
            var response = await _httpClient.GetAsync(_config.ExercisesCsvUrl);

            var csv = await response.Content.ReadAsStringAsync();

            var exercises = csv.FromCSVToIEnumerableOf<Exercise>();

//            var rows = _sheetsClient.GetRows(_config.ExerciseSpreadsheetId, _config.ExercisesRange);
//
//            var exercises = _rowMapper.Get<Exercise>(rows, _mapFunctions.MapRowToExercise);

            return exercises.Where(x => x.CreatedDate > fromDate).ToList();
        }

        public async Task<IEnumerable<Target>> GetTargets()
        {
            var response = await _httpClient.GetAsync(_config.TargetsCsvUrl);

            var csv = await response.Content.ReadAsStringAsync();

            var targets = csv.FromCSVToIEnumerableOf<Target>();

//            var lines = csv.Split("\r\n");
//
//            var targets = new List<Target>();
//
//            foreach (var values in lines.Skip(1).Select(x=>x.Split(',')))
//            {
//                try
//                {
//                    //var values = line.Split(',');
//
//                    double.TryParse(values[1], out var kg);
//                    int.TryParse(values[2], out var diastolic);
//                    int.TryParse(values[3], out var systolic);
//                    int.TryParse(values[4], out var units);
//                    int.TryParse(values[5], out var cardioMinutes);
//                    int.TryParse(values[6], out var metresErgo15Minutes);
//                    int.TryParse(values[7], out var metresTreadmill30Minutes);
//
//
//                    var target = new Target
//                    {
//                        Date = DateTime.Parse(values[0]),
//                        Kg = kg,
//                        Diastolic = diastolic,
//                        Systolic = systolic,
//                        Units = units,
//                        CardioMinutes = cardioMinutes,
//                        MetresErgo15Minutes = metresErgo15Minutes,
//                        MetresTreadmill30Minutes = metresTreadmill30Minutes
//                    };
//
//                    targets.Add(target);
//                }
//                catch (Exception ex)
//                {
//                    //log
//                }
//            }
            
            return targets;

        }

    }
}