using System;
using System.Collections.Generic;
using Utils;

namespace Importer.GoogleSheets
{
    public class SheetMapper : ISheetMapper
    {
        private readonly ISheetsClient _sheetsClient;
        private readonly IConfig _config;
        private readonly ILogger _logger;

        public SheetMapper(ISheetsClient sheetsClient, IConfig config, ILogger logger)
        {
            _sheetsClient = sheetsClient;
            _config = config;
            _logger = logger;
        }

        public List<T> Get<T>(string sheetId, string range, Func<IList<object>, T> mapperFunc)
        {
            var list = new List<T>();

            try
            {
                var rows = _sheetsClient.GetRows(sheetId, range);
                if (rows != null && rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        try
                        {
                            list.Add(mapperFunc(row));
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

            return list;
        }
    }
}