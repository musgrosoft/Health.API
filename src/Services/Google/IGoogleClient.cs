using System.Collections.Generic;
using Repositories.Models;

namespace Services.Google
{
    public interface IGoogleClient
    {
        List<Run> GetRuns();
    }
}