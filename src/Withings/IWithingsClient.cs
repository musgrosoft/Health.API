using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Withings.Domain;
using Withings.Domain.WithingsMeasureGroupResponse;
using Withings.Domain.WithingsTokenResponse;
using Response = Withings.Domain.WithingsTokenResponse.Response;

namespace Withings
{
    public interface IWithingsClient
    {
        Task<IEnumerable<Measuregrp>> GetMeasureGroups(string accessToken);
        Task<Response> GetTokensByAuthorisationCode(string authorizationCode);
        Task<Response> GetTokensByRefreshToken(string refreshToken);
//        Task<IEnumerable<Series>> Get7DaysOfSleeps(string accessToken, DateTime startDate);
//        Task<List<SSeries>> Get1DayOfDetailedSleepData(DateTime startDate, string accessToken);
    }
}