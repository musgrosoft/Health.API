using System;
using System.Collections.Generic;

namespace Importer.GoogleSheets
{
    public interface ISheetsClient
    {
        IList<IList<Object>> GetRows(string sheetId, string range);
    }
}