using System.Collections.Generic;
using Repositories.Health.Models;

namespace Services.GoogleSheets
{
    public interface IGoogleClient
    {
        //List<Run> GetRuns();
        List<AlcoholIntake> GetAlcoholIntakes();
        //List<Ergo> GetErgos();
        List<Exercise> GetExercises();
        void InsertExercises(Exercise exercise);
    }
}