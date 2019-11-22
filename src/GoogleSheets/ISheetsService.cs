using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace GoogleSheets
{
    public interface ISheetsService
    {
        Task<IEnumerable<Exercise>> GetExercises(DateTime fromDate);
        Task<IEnumerable<Drink>> GetDrinks(DateTime fromDate);
        Task<IEnumerable<Target>> GetTargets();
    }
}