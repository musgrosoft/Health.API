﻿namespace Services.Migrator
{
    public interface IGoogleMigrator
    {
        void MigrateRuns();
        void MigrateAlcoholIntakes();
        void MigrateErgos();
    }
}