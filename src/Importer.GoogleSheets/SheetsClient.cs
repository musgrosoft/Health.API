using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Utils;

namespace Importer.GoogleSheets
{
    public class SheetsClient : ISheetsClient
    {
        static string[] Scopes = { Google.Apis.Sheets.v4.SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "sheetreader";

        private readonly IConfig _config;

        public SheetsClient(IConfig config)
        {
            _config = config;
        }

        public IList<IList<object>> GetRows(string sheetId, string range)
        {
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

            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(sheetId, range);

            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;

            return values;
        }
    }
}