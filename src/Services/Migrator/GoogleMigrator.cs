using System.Collections.Generic;
using AutoMapper;
using Google;
using Repositories.Models;
using Services.Health;

namespace Services.Migrator
{
    public class GoogleMigrator : IGoogleMigrator
    {
        private readonly IGoogleClient _googleClient;
        private readonly IHealthService _healthService;

        public GoogleMigrator(IGoogleClient  googleClient, IHealthService healthService)
        {
            _googleClient = googleClient;
            _healthService = healthService;

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Google.Domain.Run, Repositories.Models.Run>();
                cfg.CreateMap<Google.Domain.AlcoholIntake, Repositories.Models.AlcoholIntake>();
                cfg.CreateMap<Google.Ergo, Repositories.Models.Ergo>();
            });
        }

        public void MigrateRuns()
        {
            var gRuns = _googleClient.GetRuns();
            var runs = Mapper.Map<List<Repositories.Models.Run>>(gRuns);

            _healthService.UpsertRuns(runs);
        }

        public void MigrateAlcoholIntakes()
        {
            var gAlcoholIntakes = _googleClient.GetAlcoholIntakes();
            var alcoholIntakes = Mapper.Map<List<AlcoholIntake>>(gAlcoholIntakes);

            _healthService.UpsertAlcoholIntakes(alcoholIntakes);
        }

        public void MigrateErgos()
        {
            var gErgos = _googleClient.GetErgos();
            var ergos = Mapper.Map<List<Repositories.Models.Ergo>>(gErgos);

            _healthService.UpsertErgos(ergos);
        }


    }
}