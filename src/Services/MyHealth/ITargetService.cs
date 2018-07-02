using System;

namespace Services.MyHealth
{
    public interface ITargetService
    {
        decimal? GetTargetWeight(DateTime dateTime);
    }
}