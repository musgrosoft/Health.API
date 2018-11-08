using System.Collections.Generic;
using Repositories.Health.Models;

namespace Google
{
    public interface IGoogleClient
    {
        List<Run> GetRuns();
        List<AlcoholIntake> GetAlcoholIntakes();
        List<Ergo> GetErgos();
    }
}