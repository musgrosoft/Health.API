using Repositories.Health.Models;

namespace Google
{
    public interface IGoogleImporter
    {
        //void MigrateRuns();
        void MigrateAlcoholIntakes();
        //void MigrateErgos();
        void ImportExercises();

        void InsertExercises(Exercise exercise);
    }
}