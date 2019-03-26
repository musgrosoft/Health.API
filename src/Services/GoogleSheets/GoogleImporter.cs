using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Repositories.Health.Models;
using Services.Health;
using Utils;

namespace Services.GoogleSheets
{

    public class GoogleImporter : IGoogleImporter
    {
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "sheetreader";

        private readonly IGoogleClient _googleClient;
        private readonly IHealthService _healthService;
        private readonly IConfig _config;

        public GoogleImporter(IGoogleClient  googleClient, IHealthService healthService, IConfig config)
        {
            _googleClient = googleClient;
            _healthService = healthService;
            _config = config;
        }

        //public void MigrateRuns()
        //{
        //    var runs = _googleClient.GetRuns();
        //    _healthService.UpsertRuns(runs);
        //}

        public void MigrateAlcoholIntakes()
        {
            var alcoholIntakes = _googleClient.GetAlcoholIntakes();
            _healthService.UpsertAlcoholIntakes(alcoholIntakes);
        }

        //public void MigrateErgos()
        //{
        //    var rows = _googleClient.GetErgos();
        //    _healthService.UpsertErgos(rows);
        //}

        public void ImportExercises()
        {
            var rows = _googleClient.GetExercises();
            _healthService.UpsertExercises(rows);

        }

        //public void InsertExercises(Exercise exercise)
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

            var service = new SheetsService(new BaseClientService.Initializer()
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