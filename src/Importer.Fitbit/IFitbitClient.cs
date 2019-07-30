//using System;
//using System.Threading.Tasks;
//using Importer.Fitbit.Domain;
//
//namespace Importer.Fitbit
//{
//    public interface IFitbitClient
//    {
//        Task<FitBitActivity> GetMonthOfFitbitActivities(DateTime startDate, string accessToken);
//        Task<FitbitAuthTokensResponse> GetTokensWithAuthorizationCode(string authorizationCode);
//        Task<FitbitRefreshTokenResponse> GetTokensWithRefreshToken(string refreshToken);
//        Task<FitbitSleeps> Get100DaysOfSleeps(DateTime startDate, string accessToken);
//    }
//}