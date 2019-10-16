using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Withings.Domain;

namespace Withings
{
    public interface IWithingsClient
    {
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(string accessToken);
        Task<WithingsTokenResponse> GetTokensByAuthorisationCode(string authorizationCode);
        Task<WithingsTokenResponse> GetTokensByRefreshToken(string refreshToken);
        Task<IEnumerable<Series>> Get7DaysOfSleeps(string accessToken, DateTime startDate);
        Task<List<SSeries>> Get1DayOfDetailedSleepData(DateTime startDate, string accessToken);
    }
}