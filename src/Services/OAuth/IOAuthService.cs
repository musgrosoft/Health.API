using System.Threading.Tasks;

namespace Services.OAuth
{
    public interface IOAuthService
    {
        Task<string> GetFitbitRefreshToken();
        Task SaveFitbitAccessToken(string token);
        Task SaveFitbitRefreshToken(string token);

        Task<string> GetNokiaRefreshToken();
        Task SaveNokiaAccessToken(string token);
        Task SaveNokiaRefreshToken(string token);


        //Task<string> GetGoogleRefreshToken();
        //        Task SaveGoogleAccessToken(string token);
        //        Task SaveGoogleRefreshToken(string token);


    }
}