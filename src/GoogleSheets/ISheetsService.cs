using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace GoogleSheets
{
    public interface ISheetsService
    {
        
        List<Exercise> GetExercises(DateTime fromDate);
        List<Drink> GetDrinks(DateTime fromDate);
        Task<IEnumerable<Target>> GetTargets();
    }
}