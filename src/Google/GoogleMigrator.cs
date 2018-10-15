﻿using Services.Health;

namespace Google
{
    public class GoogleMigrator : IGoogleMigrator
    {
        private readonly IGoogleClient _googleClient;
        private readonly IHealthService _healthService;

        public GoogleMigrator(IGoogleClient  googleClient, IHealthService healthService)
        {
            _googleClient = googleClient;
            _healthService = healthService;
        }

        public void MigrateRuns()
        {
            var runs = _googleClient.GetRuns();
            _healthService.UpsertRuns(runs);
        }

        public void MigrateAlcoholIntakes()
        {
            var alcoholIntakes = _googleClient.GetAlcoholIntakes();
            _healthService.UpsertAlcoholIntakes(alcoholIntakes);
        }

        public void MigrateErgos()
        {
            var rows = _googleClient.GetErgos();
            _healthService.UpsertErgos(rows);
        }


    }
}