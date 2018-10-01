using System;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Repositories.Models;
using Utils;

namespace Google
{
    public class GoogleClient : IGoogleClient
    {
        private readonly IConfig _config;
        private readonly ILogger _logger;
        private String RunSpreadsheetId = "1-S-oI6M61po-TIvBu0JQwDMjzNkarl3SXqiNKUdusfg";
        private String AlcoholSpreadsheetId = "15c9GFccexP91E-YmcaGr6spIEeHVFu1APRl0tNVj1io";
        private String RowSpreadsheetId = "1QL-RYs8STqWCzg_ck4rD_py93l8CzhOpeu58qXFhoXA";

        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "sheetreader";

        public GoogleClient(IConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        private IList<IList<Object>> GetRows(string sheetId, string range)
        {
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

            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(sheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            return values;
        }

        public List<AlcoholIntake> GetAlcoholIntakes()
        {
            var alcoholIntakes = new List<AlcoholIntake>();

            try
            {
                var rows = GetRows(AlcoholSpreadsheetId, "Sheet1!B2:F");
                if (rows != null && rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        try
                        {
                            var date = DateTime.Parse((string)row[0]);
                            var units = Double.Parse((string)row[1]);

                            alcoholIntakes.Add(new AlcoholIntake()
                            {
                                CreatedDate = date,
                                Units = units,

                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogErrorAsync(ex);
                        }

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogErrorAsync(ex);
            }

            return alcoholIntakes;
        }

        public List<Ergo> GetErgos()
        {
            var ergos = new List<Ergo>();

            try
            {
                var rows = GetRows(RowSpreadsheetId, "Sheet1!A2:C");
                if (rows != null && rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        try
                        {
                            var date = DateTime.Parse((string)row[0]);
                            var m = int.Parse((string)row[1]);
                            var time = TimeSpan.Parse((string)row[2]);

                            ergos.Add(new Ergo
                            {
                                CreatedDate = date,
                                Metres = m,
                                Time = time
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogErrorAsync(ex);
                        }

                    }
                }



            }
            catch (Exception ex)
            {
                _logger.LogErrorAsync(ex);
            }

            return ergos;
        }

        public List<Run> GetRuns()
        {
            var runs = new List<Run>();

            try
            {
                var rows = GetRows(RunSpreadsheetId, "Sheet1!A2:C");
                if (rows != null && rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        try
                        {
                            var date = DateTime.Parse((string)row[0]);
                            var m = int.Parse((string)row[1]);
                            var time = TimeSpan.Parse((string)row[2]);

                            runs.Add(new Run
                            {
                                CreatedDate = date,
                                Metres = m,
                                Time = time
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogErrorAsync(ex);
                        }

                    }
                }


                
            }
            catch (Exception ex)
            {
                _logger.LogErrorAsync(ex);
            }

            return runs;
        }
    }
}