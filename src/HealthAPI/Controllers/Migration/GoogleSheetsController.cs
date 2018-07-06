//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Migrators;
//using Repositories;
//using Repositories.OAuth;
//using Services.Fitbit;
//using Services.MyHealth;
//using Services.Nokia;
//using Services.OAuth;
//using Utils;
//using Services.Google;
//using System.Net.Http;
//using System.Collections.Generic;
//using Newtonsoft.Json;
//
//namespace HealthAPI.Controllers.Migration
//{
//    [Produces("application/json")]
//    [Route("api/Google")]
//    public class GoogleSheetsController : Controller
//    {
//        private readonly IHealthService _healthService;
//
//        public GoogleSheetsController(IHealthService healthService)
//        {
//            _healthService = healthService;
//        }
//
//        [HttpGet]
//        public async Task<IActionResult> Migrate()
//        {
//
//            var logger = new Logger();
//            try
//            {
//                //var logger = context.Logger;
//                logger.Log("GOOGLE SHEETS : starting fitbit migrate");
//
//                //logger.Log("STARTING NOKIA MIGRATOR");
//                
//                _healthService.UpsertAlcoholIntakes();
//
//
//              //  var oAuthService = new OAuthService(new OAuthTokenRepository(new Config(), logger));
//            //    oAuthService.SaveGoogleRefreshToken("1/KBHJU61ZfR3FS7Jx5RVbuqO3szkeU6ejDDjQquX7J7k");
//
//                //var v = await oAuthService.GetGoogleRefreshToken();
//              
//                //var googleAuthenticator = new GoogleAuthenticator(oAuthService);
//                //var googleAccessToken = await googleAuthenticator.GetAccessToken();
//                
//                //var _httpClient = new HttpClient();
//                //var uri = "https://sheets.googleapis.com/v4/spreadsheets/15c9GFccexP91E-YmcaGr6spIEeHVFu1APRl0tNVj1io/values/Sheet1!A1:I10";
//                //_httpClient.DefaultRequestHeaders.Clear();
//                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + googleAccessToken);
//
//                //var response = await _httpClient.GetAsync(uri);
//                //if (response.IsSuccessStatusCode)
//                //{
//                //    var content = await response.Content.ReadAsStringAsync();
//                //    var data = JsonConvert.DeserializeObject<GResponse>(content);
//                //    var a=1;
//                //}
//
//
//         
//
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                
//                logger.Error(ex);
//                //todo return error
//                return NotFound(ex.ToString());
//
//            }
//        }
//
//       
//    }
//
//    public class GResponse
//    {
//
//        public string range { get; set; }
//        public List<List<string>> values { get; set; }
//        public string majorDimension { get; set; }
//    }
//}