namespace Google
{
    public interface IGoogleMigrator
    {
        void MigrateRuns();
        void MigrateAlcoholIntakes();
        void MigrateErgos();
    }
}