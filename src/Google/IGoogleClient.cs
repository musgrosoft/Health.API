using System.Collections.Generic;
using Google.Domain;
using AlcoholIntake = Google.Domain.AlcoholIntake;

namespace Google
{
    public interface IGoogleClient
    {
        List<Run> GetRuns();
        List<AlcoholIntake> GetAlcoholIntakes();
        List<Ergo> GetErgos();
    }
}