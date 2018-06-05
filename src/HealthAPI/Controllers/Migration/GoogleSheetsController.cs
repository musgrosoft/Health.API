using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Repositories;
using Repositories.OAuth;
using Services.Fitbit;
using Services.MyHealth;
using Services.Nokia;
using Services.OAuth;
using Utils;
using Services.Google;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Google")]
    public class GoogleSheetsController : Controller
    {
        private readonly HealthContext _context;

        public GoogleSheetsController(HealthContext context)
        {
            _context = context;
        }

        // POST: api/AlcoholIntakes1
        [HttpGet]
     //   [Route("api/Nokia/Migrate")]
        public async Task<IActionResult> Migrate()
        {

            var logger = new Logger();
            try
            {
                //var logger = context.Logger;
                logger.Log("GOOGLE SHEETS : starting fitbit migrate");

                //logger.Log("STARTING NOKIA MIGRATOR");
                var healthService = HealthServiceFactory.Build( logger, _context);


            //    var cli = new HttpClient();
            //    var urii = "www.googleapis.com/oauth2/v4/token";

            //    var nvc = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("code", "4%2FAABxe0veZtH83UKxMORmVnRiCbTXBGY_jGOo1lzYWER1xFQcrNhU29ko6bVUsBYgoD0F0rDSpPRh7KzVxtONYAg "),
            //    new KeyValuePair<string, string>("redirect_uri", "https://"),
            //    new KeyValuePair<string, string>("refresh_token", refreshToken),
            //    new KeyValuePair<string, string>("client_id", "407408718192.apps.googleusercontent.com"),
            //};

            //    code = 4 % 2FAABxe0veZtH83UKxMORmVnRiCbTXBGY_jGOo1lzYWER1xFQcrNhU29ko6bVUsBYgoD0F0rDSpPRh7KzVxtONYAg 
            //        & redirect_uri = https % 3A % 2F % 2Fdevelopers.google.com % 2Foauthplayground 
            //        & client_id = 627416670233 - kre0k5gg98rn1pv86qmp09bd1fbq2j07.apps.googleusercontent.com 
            //        & client_secret = SHjTFZP4qSPL7aK - 5Ctb1S0Q 
            //        & scope = 
            //        &grant_type = authorization_code



                var oAuthService = new OAuthService(new OAuthTokenRepository(new Config(), logger));
            //    oAuthService.SaveGoogleRefreshToken("1/KBHJU61ZfR3FS7Jx5RVbuqO3szkeU6ejDDjQquX7J7k");

                var v = await oAuthService.GetGoogleRefreshToken();
                //logger.Log("fitbit refresh token is " + v);
              //  return Ok("fitbit refresh token is " + v);

                var googleAuthenticator = new GoogleAuthenticator(oAuthService);
                var googleAccessToken = await googleAuthenticator.GetAccessToken();

                // var healthService = HealthServiceFactory.Build(logger);


                var _httpClient = new HttpClient();
                var uri = "https://sheets.googleapis.com/v4/spreadsheets/15c9GFccexP91E-YmcaGr6spIEeHVFu1APRl0tNVj1io/values/Sheet1!A1:I10";
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + googleAccessToken);

                var response = await _httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<GResponse>(content);
                    var a=1;
                }


         


                //var fitbitClient = new FitbitClient(new System.Net.Http.HttpClient(), new Config(), fitbitAccessToken, new Logger());
                //var fitbitAggregator = new FitbitClientClientAggregator(fitbitClient, logger);
                //var fitbitService = new FitbitService(new Config(), logger, fitbitAggregator);

                //var fitbitMigrator = new FitbitMigrator(healthService, logger, fitbitService, new Calendar());

                //await fitbitMigrator.MigrateHeartSummaries();
                //await fitbitMigrator.MigrateRestingHeartRates();
                //await fitbitMigrator.MigrateStepCounts();
                //await fitbitMigrator.MigrateActivitySummaries();


                //logger.Log("FITBIT : finishing fitbit migrate");

                return Ok();
                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.OK
                //};

            }
            catch (Exception ex)
            {
                
                logger.Error(ex);
                return NotFound(ex.ToString());
                //LambdaLogger.Log(ex.ToString());

                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.InternalServerError,
                //    Body = $"Uh oh, {ex.ToString()}"
                //};

            }
        }

       
    }

    public class GResponse
    {

        public string range { get; set; }
        public List<List<string>> values { get; set; }
        public string majorDimension { get; set; }
    }
}