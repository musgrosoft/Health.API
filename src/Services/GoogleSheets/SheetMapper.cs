using System;
using System.Collections.Generic;
using Utils;

namespace Google
{
    public class SheetMapper : ISheetMapper
    {
        private readonly IGoogleRowCollector _googleRowCollector;
        private readonly IConfig _config;
        private readonly ILogger _logger;

        public SheetMapper(IGoogleRowCollector googleRowCollector, IConfig config, ILogger logger)
        {
            _googleRowCollector = googleRowCollector;
            _config = config;
            _logger = logger;
        }

        public List<T> Get<T>(string sheetId, string range, Func<IList<object>, T> mapperFunc)
        {
            var list = new List<T>();

            try
            {
                var rows = _googleRowCollector.GetRows(sheetId, range);
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