using System;
using System.Threading.Tasks;
using Fitbit.Domain;

namespace Fitbit
{
    public interface IFitbitClient
    {
        Task<FitBitActivity> GetMonthOfFitbitHeartRates(DateTime startDate, string accessToken);
        Task<FitbitSleepsResponse> Get100DaysOfSleeps(DateTime startDate, string accessToken);
        Task<FitbitRefreshTokenResponse> GetTokensWithRefreshToken(string refreshToken);
        Task<FitbitAuthTokensResponse> GetTokensWithAuthorizationCode(string authorizationCode);
    }
}