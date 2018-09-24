using System;
using System.Threading.Tasks;
using Services.Fitbit.Domain;

namespace Services.Fitbit
{
    public interface IFitbitClient
    {
        Task<FitbitDailyActivity> GetFitbitDailyActivity(DateTime date);
        Task<FitBitActivity> GetMonthOfFitbitActivities(DateTime startDate);
        Task Subscribe();
        Task<FitbitRefreshTokenResponse> GetTokensWithRefreshToken(string refreshToken);
        Task<FitbitAuthTokensResponse> GetTokensWithAuthorizationCode(string authorizationCode);
    }
}