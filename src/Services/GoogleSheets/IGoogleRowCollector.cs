using System;
using System.Collections.Generic;

namespace Services.GoogleSheets
{
    public interface IGoogleRowCollector
    {
        IList<IList<Object>> GetRows(string sheetId, string range);
    }
}