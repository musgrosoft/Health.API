using System;
using System.Collections.Generic;
using Utils;

namespace Importer.GoogleSheets
{
    public class RowMapper : IRowMapper
    {
        private readonly ILogger _logger;

        public RowMapper(ILogger logger)
        {
            _logger = logger;
        }

        public List<T> Get<T>(IList<IList<object>> rows, Func<IList<object>, T> mapperFunc)
        {
            var list = new List<T>();

            if (rows == null || rows.Count == 0)
            {
                return list;
            }


            //    try
            //{
                
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
                



            //}
//            catch (Exception ex)
//            {
//                _logger.LogErrorAsync(ex);
//            }

            return list;
        }
    }
}