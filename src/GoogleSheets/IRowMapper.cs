using System;
using System.Collections.Generic;

namespace Importer.GoogleSheets
{
    public interface IRowMapper
    {
        List<T> Get<T>(IList<IList<object>> rows, Func<IList<object>, T> mapperFunc);
    }
}