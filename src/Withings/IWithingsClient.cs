using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Withings.Domain.WithingsMeasureGroupResponse;
using Withings.Domain.WithingsTokenResponse;

namespace Withings
{
    public interface IWithingsClient
    {
        Task<IEnumerable<Measuregrp>> GetMeasureGroups(string accessToken, DateTime lastUpdatedDate);
        Task<WithingsTokenResponse> GetTokensByAuthorisationCode(string authorizationCode);
        Task<WithingsTokenResponse> GetTokensByRefreshToken(string refreshToken);
    }
}