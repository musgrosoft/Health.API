using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
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
        private readonly ILogger _logger;

        public SheetsService(IConfig config, HttpClient httpClient, ICalendar calendar, ILogger logger)
        {
            _config = config;
            _httpClient = httpClient;
            _calendar = calendar;
            _logger = logger;
        }

        public async Task<IEnumerable<Drink>> GetDrinks(DateTime fromDate)
        {
            var response = await _httpClient.GetAsync($"https://docs.google.com/spreadsheets/d/{_config.DrinksSpreadsheetId}/gviz/tq?tqx=out:csv&sheet=Sheet1");

            var csv = await response.Content.ReadAsStringAsync();

            var drinks = await FromCSVToIEnumerableOf<Drink>(csv);

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

            await _logger.LogMessageAsync("Exercise csv is " + csv);

            var exercises = await FromCSVToIEnumerableOf<Exercise>(csv);

            return exercises.Where(x => x.CreatedDate.Between(fromDate, _calendar.Now().Date)).ToList();
        }

        public async Task<IEnumerable<Target>> GetTargets()
        {
            var response = await _httpClient.GetAsync($"https://docs.google.com/spreadsheets/d/{_config.TargetsSpreadsheetId}/gviz/tq?tqx=out:csv&sheet=Sheet1");
            
            var csv = await response.Content.ReadAsStringAsync();

            var targets = await FromCSVToIEnumerableOf<Target>(csv);
            
            return targets;
        }

        public async Task<IEnumerable<GarminRestingHeartRate>> GetGarminRestingHeartRates()
        {
            var response = await _httpClient.GetAsync($"https://docs.google.com/spreadsheets/d/{_config.GarminRestingHeartRateSpreadsheetId}/gviz/tq?tqx=out:csv&sheet=Sheet1");

            var csv = await response.Content.ReadAsStringAsync();

            await _logger.LogMessageAsync("Garming Resting Heart Rate csv is " + csv);

            var garminRestingHeartRates = await FromCSVToIEnumerableOf<GarminRestingHeartRate>(csv);

            return garminRestingHeartRates;
        }

        public async Task<IEnumerable<GarminIntensityMinutes>> GetGarminIntensityMinutes()
        {
            var response = await _httpClient.GetAsync($"https://docs.google.com/spreadsheets/d/{_config.GarminIntensityMinutesSpreadsheetId}/gviz/tq?tqx=out:csv&sheet=Sheet1");

            var csv = await response.Content.ReadAsStringAsync();

            await _logger.LogMessageAsync("Garming Intensity Minutes csv is " + csv);


            var garminIntensityMinutes = await FromCSVToIEnumerableOf<GarminIntensityMinutes>(csv);

            return garminIntensityMinutes;
        }

        private async Task <IEnumerable<T>> FromCSVToIEnumerableOf<T>(String csv) where T : new()
        {
            var listT = new List<T>();

            try
            {


                var lines = csv.Replace("\"", "").Split("\n");

                var propertyNames = lines.First().Split(',');


                Type myType = typeof(T);

                var propertyInfos = new Dictionary<string, PropertyInfo>();


                foreach (var propertyName in propertyNames)
                {
                    PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                    if (myPropInfo != null)
                    {
                        propertyInfos.Add(propertyName, myPropInfo);
                    }
                }



                foreach (var line in lines.Skip(1))
                {
                    try
                    {
                        var values = Regex.Split(line, "(?:^|,)(\"(?:[^\"])*\"|[^,]*)");
                        var elementT = new T();

                        for (int i = 0; i < propertyNames.Length; i++)
                        {
                            if (propertyInfos.ContainsKey(propertyNames[i]))
                            {
                                var value = values[i];

                                if (!string.IsNullOrWhiteSpace(value))
                                {
                                    
                                    var propInfo = propertyInfos[propertyNames[i]];

                                    //await _logger.LogMessageAsync($"Property name is {propInfo.Name} , type is {propInfo.PropertyType} , value being parsed is {value} .");

                                    var typedValue = Convert.ChangeType(value, propInfo.PropertyType);

                                    propInfo.SetValue(elementT, typedValue);
                                }
                            }
                        }

                        listT.Add(elementT);
                    }
                    catch (Exception ex)
                    {
                        await _logger.LogErrorAsync(new Exception("Error parsing individual line ", ex));
                    }

                }
            }
            catch(Exception ex)
            {
                await _logger.LogErrorAsync(ex);
            }


            return listT;
        }


    }
}