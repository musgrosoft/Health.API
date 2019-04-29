using System;
using System.Threading.Tasks;
using Importer.Fitbit.Domain;

namespace Importer.Fitbit
{
    public interface IFitbitClient
    {
        //Task<FitbitDailyActivity> GetFitbitDailyActivity(DateTime date);
        Task<FitBitActivity> GetMonthOfFitbitActivities(DateTime startDate, string accessToken);
        Task Subscribe(string accessToken);

        //Task<List<Dataset>> GetDetailedHeartRates(DateTime date);
        Task<FitbitAuthTokensResponse> GetTokensWithAuthorizationCode(string authorizationCode);
        Task<FitbitRefreshTokenResponse> GetTokensWithRefreshToken(string refreshToken);
    }
}