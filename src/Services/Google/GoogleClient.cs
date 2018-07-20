using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Repositories.Models;
using Utils;

namespace Services.Google
{
    public class GoogleClient : IGoogleClient
    {
        private readonly IConfig _config;
        private readonly ILogger _logger;

        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "sheetreader";

        public GoogleClient(IConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public List<Run> GetRuns()
        {
            var runs = new List<Run>();

            try
            {
                var id = _config.GoogleClientId;
                var secret = _config.GoogleClientSecret;

                var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(id)
                {
                    Scopes = Scopes
                }.FromPrivateKey($"-----BEGIN PRIVATE KEY-----\n{secret}\n-----END PRIVATE KEY-----\n"));

                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                String spreadsheetId = "1-S-oI6M61po-TIvBu0JQwDMjzNkarl3SXqiNKUdusfg";
                String range = "Sheet1!A2:C";
                SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

                // Prints the names and majors of students in a sample spreadsheet:
                // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
                ValueRange response = request.Execute();
                IList<IList<Object>> values = response.Values;
                if (values != null && values.Count > 0)
                {
                    foreach (var row in values)
                    {
                        var date = DateTime.Parse(row[0].ToString());
                        var km = int.Parse(row[1].ToString());
                        var time = TimeSpan.Parse(row[2].ToString());

                        runs.Add(new Run
                        {
                            CreatedDate = date,
                            Distance = km,
                            Time = time
                        });

                        // Print columns A and E, which correspond to indices 0 and 4.
                        Console.WriteLine($"{date}, {km}, {time}");
                    }
                }


                
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return runs;
        }
    }
}