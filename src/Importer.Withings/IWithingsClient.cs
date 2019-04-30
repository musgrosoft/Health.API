using System.Collections.Generic;
using System.Threading.Tasks;
using Importer.Withings.Domain;

namespace Importer.Withings
{
    public interface IWithingsClient
    {
        Task Subscribe(string accessToken);
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups(string accessToken);
        Task<string> GetWeightSubscription(string accessToken);
        Task<string> GetBloodPressureSubscription(string accessToken);
        Task<WithingsTokenResponse> GetTokensByAuthorisationCode(string authorizationCode);
        Task<WithingsTokenResponse> GetTokensByRefreshToken(string refreshToken);
    }
}