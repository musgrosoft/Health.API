using System;
using System.Collections.Generic;
using Repositories.Models;

namespace Google
{
    public interface IMapper
    {
        Run MapRowToRun(IList<Object> row);
        Ergo MapRowToErgo(IList<Object> row);
        AlcoholIntake MapRowToAlcoholIntake(IList<Object> row);
    }
}