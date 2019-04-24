using System.Collections.Generic;
using Repositories.Health.Models;

namespace Importer.GoogleSheets
{
    public interface IGoogleClient
    {
        //List<Run> GetRuns();
        List<Drink> GetDrinks();
        //List<Ergo> GetErgos();
        List<Exercise> GetExercises();
        void InsertExercises(Exercise exercise);
        List<Drink> GetHistoricAlcoholIntakes();
    }
}