using System;
using System.Collections.Generic;

namespace Importer.GoogleSheets
{
    public interface IGoogleRowCollector
    {
        IList<IList<Object>> GetRows(string sheetId, string range);
    }
}