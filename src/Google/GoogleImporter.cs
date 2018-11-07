using Services.Health;

namespace Google
{
    public class GoogleImporter : IGoogleImporter
    {
        private readonly IGoogleClient _googleClient;
        private readonly IHealthService _healthService;
        //private readonly ITargetService _targetService;

        public GoogleImporter(IGoogleClient  googleClient, IHealthService healthService)
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
           // alcoholIntakes = _targetService.SetTargets(alcoholIntakes);
            _healthService.UpsertAlcoholIntakes(alcoholIntakes);
        }

        public void MigrateErgos()
        {
            var rows = _googleClient.GetErgos();
            _healthService.UpsertErgos(rows);
        }


    }
}