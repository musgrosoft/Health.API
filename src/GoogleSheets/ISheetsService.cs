using System;
using System.Collections.Generic;
using Repositories.Health.Models;

namespace GoogleSheets
{
    public interface ISheetsService
    {
        
        List<Exercise> GetExercises(DateTime fromDate);
        List<Drink> GetDrinks(DateTime fromDate);
    }
}