using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Utils;
using Withings.Domain.WithingsMeasureGroupResponse;
using Withings.Domain.WithingsTokenResponse;


namespace Withings
{
    public class WithingsClient : IWithingsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IConfig _config;
        
        public WithingsClient(HttpClient httpClient, ILogger logger, IConfig config)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
        }

        public async Task<WithingsTokenResponse> GetTokensByAuthorisationCode(string authorizationCode)
        {

                var url = $"{_config.WithingsAccountBaseUrl}/oauth2/token";

                var nvc = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("client_id", _config.WithingsClientId),
                    new KeyValuePair<string, string>("client_secret", _config.WithingsClientSecret),
                    new KeyValuePair<string, string>("code", authorizationCode),
                    new KeyValuePair<string, string>("redirect_uri", _config.WithingsRedirectUrl)
                };

                await _logger.LogMessageAsync("~~~ trying to get tokens by authorisation code");

                var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(nvc));

                await _logger.LogMessageAsync("~~~ received response from Withings");

                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogMessageAsync("~~~ Success Status Code");

                    var tokenResponse = JsonConvert.DeserializeObject<WithingsTokenResponse>(responseBody);

                    if (String.IsNullOrEmpty(tokenResponse.access_token) ||
                        String.IsNullOrEmpty(tokenResponse.refresh_token))
                    {
                        throw new Exception($"Access token or Refresh token is empty : {(int)response.StatusCode} , content: {responseBody}");
                    }

                    return tokenResponse;

                }
                else
                {
                    await _logger.LogMessageAsync("~~~ Non Success Status Code");

                    throw  new Exception($"non success status code : {(int)response.StatusCode} , content: {responseBody}");

                }
            

        }

        public async Task<WithingsTokenResponse> GetTokensByRefreshToken(string refreshToken)
        {
            var url = $"{_config.WithingsAccountBaseUrl}/oauth2/token";

            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("client_id", _config.WithingsClientId),
                new KeyValuePair<string, string>("client_secret", _config.WithingsClientSecret),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("redirect_uri", _config.WithingsRedirectUrl)
            };

            await _logger.LogMessageAsync("~~~ trying to get tokens by refresh token");

            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(nvc));

            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                await _logger.LogMessageAsync("~~~ Success Status Code");

                var tokenResponse = JsonConvert.DeserializeObject<WithingsTokenResponse>(responseBody);

                if (String.IsNullOrEmpty(tokenResponse.access_token) ||
                    String.IsNullOrEmpty(tokenResponse.refresh_token))
                {
                    throw new Exception($"Access token or Refresh token is empty : {(int)response.StatusCode} , content: {responseBody}");
                }

                return tokenResponse;
            }
            else
            {
                await _logger.LogMessageAsync("~~~ Non Success Status Code");

                throw new Exception($"non success status code : {(int)response.StatusCode} , content: {responseBody}");
            }


        }

        public async Task<IEnumerable<Measuregrp>> GetMeasureGroups(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            //todo add lastupdate - to getmodified since
            var response = await _httpClient.GetAsync($"{_config.WithingsApiBaseUrl}/measure?action=getmeas");//&access_token={accessToken}");

            //TODO : success status code, does not indicate lack of error
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<Domain.WithingsMeasureGroupResponse.WithingsMeasureGroupResponse>(content);
                return data.body.measuregrps;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                
                throw new Exception($"Error calling withings api , status code is {(int)response.StatusCode} , and content is {content}");
            }
        }

    }
}