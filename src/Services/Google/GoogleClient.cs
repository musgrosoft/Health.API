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

        public List<AlcoholIntake> GetAlcoholIntakes()
        {
            var alcoholIntakes = new List<AlcoholIntake>();

            try
            {
                var id = _config.GoogleClientId;
                var secret = _config.GoogleClientSecret;

                _logger.Log($"secret is {secret}");

                var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(id)
                {
                    Scopes = Scopes
                }.FromPrivateKey(secret));

                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                String spreadsheetId = "15c9GFccexP91E-YmcaGr6spIEeHVFu1APRl0tNVj1io";
                String range = "Sheet1!E2:F";
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
                        try
                        {
                            var date = DateTime.Parse(row[0].ToString());
                            var units = double.Parse(row[1].ToString());

                            alcoholIntakes.Add(new AlcoholIntake()
                            {
                                CreatedDate = date,
                                Units = units,

                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex);
                        }

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return alcoholIntakes;
        }

        public List<Run> GetRuns()
        {
            var runs = new List<Run>();

            try
            {
                var id = _config.GoogleClientId;
                var secret = _config.GoogleClientSecret;

                _logger.Log($"secret is {secret}");

                var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(id)
                {
                    Scopes = Scopes
                }.FromPrivateKey(secret));

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
                        try { 
                            var date = DateTime.Parse(row[0].ToString());
                            var km = int.Parse(row[1].ToString());
                            var time = TimeSpan.Parse(row[2].ToString());

                            runs.Add(new Run
                            {
                                CreatedDate = date,
                                Distance = km,
                                Time = time
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(ex);
                        }

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