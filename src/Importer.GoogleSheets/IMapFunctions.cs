using System;
using System.Collections.Generic;
using Repositories.Health.Models;

namespace Importer.GoogleSheets
{
    public interface IMapFunctions
    {
        Drink MapRowToDrink(IList<Object> row);
        Exercise MapRowToExercise(IList<Object> row);
    }
}