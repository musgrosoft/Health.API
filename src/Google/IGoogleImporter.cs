namespace Google
{
    public interface IGoogleImporter
    {
        void MigrateRuns();
        void MigrateAlcoholIntakes();
        void MigrateErgos();
    }
}