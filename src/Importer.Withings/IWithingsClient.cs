using System.Collections.Generic;
using System.Threading.Tasks;
using Importer.Withings.Domain;

namespace Importer.Withings
{
    public interface IWithingsClient
    {
        Task Subscribe();
        Task<IEnumerable<Response.Measuregrp>> GetMeasureGroups();
        Task<string> GetWeightSubscription();
        Task<string> GetBloodPressureSubscription();
        Task<WithingsTokenResponse> GetTokensByAuthorisationCode(string authorizationCode);
        Task<WithingsTokenResponse> GetTokensByRefreshToken(string refreshToken);
    }
}