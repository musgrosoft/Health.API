using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Repositories.Health.Models;
using Utils;

namespace Importer.GoogleSheets
{
    public class SheetsService : ISheetsService
    {
        private readonly IConfig _config;
        private readonly ISheetMapper _sheetMapper;
        private readonly IMapFunctions _mapFunctions;
        static string[] Scopes = { Google.Apis.Sheets.v4.SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "sheetreader";

        public SheetsService(IConfig config, ISheetMapper sheetMapper, IMapFunctions mapFunctions)
        {
            _config = config;
            _sheetMapper = sheetMapper;
            _mapFunctions = mapFunctions;
        }

        public List<Drink> GetHistoricDrinks()
        {
            return _sheetMapper.Get<Drink>(_config.HistoricAlcoholSpreadsheetId, "Sheet1!A2:C", _mapFunctions.MapRowToDrink);
        }

        public List<Drink> GetDrinks()
        {
            var drinks = _sheetMapper.Get<Drink>(_config.AlcoholSpreadsheetId, "Sheet1!A2:C", _mapFunctions.MapRowToDrink);

            return drinks
                .GroupBy(x => x.CreatedDate)
                .Select(x => new Drink
                {
                    CreatedDate = x.Key,
                    Units = x.Sum(y => y.Units)
                }).ToList();
        }

        public List<Exercise> GetExercises()
        {
            return _sheetMapper.Get<Exercise>(_config.ExerciseSpreadsheetId, "Sheet1!A2:E", _mapFunctions.MapRowToExerise);
        }

        public void InsertExercises(Exercise exercise)
        {
            var sheetId = "12-TLRiHDo0c5-HebyBR3vX5tQ1OY5RGHQego2qe18h4";
            var range = "Sheet1!A:D";

            var id = _config.GoogleClientId;
            var secret = _config.GoogleClientSecret;

            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(id)
            {
                Scopes = Scopes
            }.FromPrivateKey(secret));

            var service = new Google.Apis.Sheets.v4.SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            IList<IList<Object>> values = new List<IList<object>>
            {
                new List<object>
                {
                    exercise.CreatedDate,
                    exercise.Description,
                    exercise.TotalSeconds,
                    exercise.Metres
                }
            };

            SpreadsheetsResource.ValuesResource.AppendRequest request =
                service.Spreadsheets.Values.Append(new ValueRange() { Values = values }, sheetId, range);

            request.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;

            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var response = request.Execute();

        }
    }
}