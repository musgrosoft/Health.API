using System;
using System.Collections.Generic;

namespace Google
{
    public interface IGoogleRowCollector
    {
        IList<IList<Object>> GetRows(string sheetId, string range);
    }
}