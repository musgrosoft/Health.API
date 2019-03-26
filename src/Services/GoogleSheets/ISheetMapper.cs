using System;
using System.Collections.Generic;

namespace Services.GoogleSheets
{
    public interface ISheetMapper
    {
        List<T> Get<T>(string sheetId, string range, Func<IList<object>, T> mapperFunc);
    }
}