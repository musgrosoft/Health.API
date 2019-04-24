using System;
using System.Collections.Generic;
using Repositories.Health.Models;

namespace Importer.GoogleSheets
{
    public interface IMapper
    {
        Drink MapRowToDrink(IList<Object> row);
        Exercise MapRowToExerise(IList<Object> row);
    }
}