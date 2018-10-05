using System.Collections.Generic;
using Repositories.Models;

namespace Google
{
    public interface IGoogleClient
    {
        List<Run> GetRuns();
        List<AlcoholIntake> GetAlcoholIntakes();
        List<Ergo> GetErgos();
    }
}