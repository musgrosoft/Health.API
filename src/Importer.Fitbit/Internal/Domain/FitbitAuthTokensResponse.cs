namespace Importer.Fitbit.Internal.Domain
{
    internal class FitbitAuthTokensResponse
    {
        internal string access_token { get; set; }
        //public int expires_in { get; set; }
        internal string refresh_token { get; set; }
        //public string token_type { get; set; }
        //public string user_id { get; set; }
    }
}