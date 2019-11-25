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
        private readonly ICalendar _calendar;

        public SheetsService(IConfig config, HttpClient httpClient, ICalendar calendar)
        {
            _config = config;
            _httpClient = httpClient;
            _calendar = calendar;
        }

        public async Task<IEnumerable<Drink>> GetDrinks(DateTime fromDate)
        {
            var response = await _httpClient.GetAsync($"https://docs.google.com/spreadsheets/d/{_config.DrinksSpreadsheetId}/gviz/tq?tqx=out:csv&sheet=Sheet1");

            var csv = await response.Content.ReadAsStringAsync();

            var drinks = csv.FromCSVToIEnumerableOf<Drink>();

            return drinks
                .Where(x => x.CreatedDate.Between(fromDate,_calendar.Now().Date))
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
            var response = await _httpClient.GetAsync($"https://docs.google.com/spreadsheets/d/{_config.ExerciseSpreadsheetId}/gviz/tq?tqx=out:csv&sheet=Sheet1");

            var csv = await response.Content.ReadAsStringAsync();

            var exercises = csv.FromCSVToIEnumerableOf<Exercise>();

            return exercises.Where(x => x.CreatedDate.Between(fromDate, _calendar.Now().Date)).ToList();
        }

        public async Task<IEnumerable<Target>> GetTargets()
        {
            var response = await _httpClient.GetAsync($"https://docs.google.com/spreadsheets/d/{_config.TargetsSpreadsheetId}/gviz/tq?tqx=out:csv&sheet=Sheet1");
            
            var csv = await response.Content.ReadAsStringAsync();

            var targets = csv.FromCSVToIEnumerableOf<Target>();
            
            return targets;
        }

    }
}