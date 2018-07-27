﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Repositories.Models;
using Services.Nokia.Domain;
using Utils;
using JetBrains.Annotations;

namespace Services.Nokia
{
    public class NokiaClient : INokiaClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly INokiaAuthenticator _nokiaAuthenticator;
        private const int WeightKgMeasureTypeId = 1;
        private const int FatRatioPercentageMeasureTypeId = 6;
        private const int DiastolicBloodPressureMeasureTypeId = 9;
        private const int SystolicBloodPressureMeasureTypeId = 10;
        private const int SubscribeBloodPressureId = 4;

        private const string NOKIA_BASE_URL = "http://api.health.nokia.com";

        public NokiaClient(HttpClient httpClient, ILogger logger, INokiaAuthenticator nokiaAuthenticator)
        {
            _httpClient = httpClient;
            _logger = logger;
            _nokiaAuthenticator = nokiaAuthenticator;
        }

        public async Task Subscribe()
        {
            var accessToken = _nokiaAuthenticator.GetAccessToken();
            var callback = "http://musgrosoft-health-api.azurewebsites.net/api/nokia";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();

            var uri = NOKIA_BASE_URL + $"/notify?action=subscribe&access_token={accessToken}&callback={callback}&appli={WeightKgMeasureTypeId}";

            var response = await _httpClient.GetAsync(uri);

            _logger.Log("Status code ::: " + response.StatusCode);
            _logger.Log("content ::: " + response.Content);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();

             uri = NOKIA_BASE_URL + $"/notify?action=subscribe&access_token={accessToken}&callback={callback}&appli={SubscribeBloodPressureId}";

             response = await _httpClient.GetAsync(uri);

            _logger.Log("Status code ::: " + response.StatusCode);
            _logger.Log("content ::: " + response.Content);

        }

        public async Task<IEnumerable<Weight>> GetWeights(DateTime sinceDateTime)
        {
            var accessToken = _nokiaAuthenticator.GetAccessToken();

            //return null;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();

            //https://api.health.nokia.com/measure?action=getmeas&access_token = YOUR - ACCESS - TOKEN

            //var response = await _httpClient.GetAsync($"{NOKIA_BASE_URL}/measure?action=getmeas&oauth_consumer_key=ebb1cbd42bb69687cb85ccb20919b0ff006208b79c387059123344b921837d8d&oauth_nonce=742bef6a3da52fbf004573d18b8f04cf&oauth_signature=cgO95H%2Fg2qx0VQ9ma2k8qeHronM%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1503326610&oauth_token=7f027003b78369d415bd0ee8e91fdd43408896616108b72b97fd7c153685f&oauth_version=1.0&userid=8792669");
            var response = await _httpClient.GetAsync($"{NOKIA_BASE_URL}/measure?action=getmeas&access_token={accessToken}");
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                
                var data = JsonConvert.DeserializeObject<Response.RootObject>(content);
                
                var weights = new List<Weight>();

                var dateFilteredMeasures = data.body.measuregrps.Where(x => x.date.ToDateFromUnixTime() >= sinceDateTime);
                var weightMeasures = dateFilteredMeasures.Where(x => x.measures.Any(y => y.type == WeightKgMeasureTypeId)).ToList();

                foreach (var weightMeasure in weightMeasures)
                {
                    weights.Add(new Weight
                    {
                        CreatedDate = weightMeasure.date.ToDateFromUnixTime(),
                        Kg =  weightMeasure.measures.First(x => x.type == WeightKgMeasureTypeId).value * Math.Pow(10, weightMeasure.measures.First(x => x.type == WeightKgMeasureTypeId).unit),

                        //todo set if available
                       // FatRatioPercentage = (Decimal)(weightMeasure.measures.FirstOrDefault(x => x.type == FatRatioPercentageMeasureTypeId).value * Math.Pow(10, weightMeasure.measures.FirstOrDefault(x => x.type == FatRatioPercentageMeasureTypeId).unit)),
                    });
                }

                return weights;
            }
            else
            {
                throw new Exception($"Error calling nokia api , status code is {response.StatusCode} , and content is {content}");
            }
        }

        public  async Task<IEnumerable<BloodPressure>> GetBloodPressures(DateTime sinceDateTime)
        {
            var accessToken = _nokiaAuthenticator.GetAccessToken();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Clear();

            //var response = await _httpClient.GetAsync($"{NOKIA_BASE_URL}/measure?action=getmeas&oauth_consumer_key=ebb1cbd42bb69687cb85ccb20919b0ff006208b79c387059123344b921837d8d&oauth_nonce=742bef6a3da52fbf004573d18b8f04cf&oauth_signature=cgO95H%2Fg2qx0VQ9ma2k8qeHronM%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1503326610&oauth_token=7f027003b78369d415bd0ee8e91fdd43408896616108b72b97fd7c153685f&oauth_version=1.0&userid=8792669");

            var response = await _httpClient.GetAsync($"{NOKIA_BASE_URL}/measure?action=getmeas&access_token={accessToken}");


            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Response.RootObject>(content);

                var bloodPressures = new List<BloodPressure>();

                var dateFilteredMeasures = data.body.measuregrps.Where(x => x.date.ToDateFromUnixTime() >= sinceDateTime);

                var bloodPressureMeasures = dateFilteredMeasures.Where(x =>
                    x.measures.Any(y => y.type == DiastolicBloodPressureMeasureTypeId) &&
                    x.measures.Any(y => y.type == SystolicBloodPressureMeasureTypeId)).ToList();

                foreach (var bloodPressureMeasure in bloodPressureMeasures)
                {
                    bloodPressures.Add(new BloodPressure
                    {
                        CreatedDate = bloodPressureMeasure.date.ToDateFromUnixTime(),
                        Diastolic = (int)( bloodPressureMeasure.measures.First(x => x.type == DiastolicBloodPressureMeasureTypeId).value * Math.Pow(10, bloodPressureMeasure.measures.First(x => x.type == DiastolicBloodPressureMeasureTypeId).unit)),
                        Systolic = (int)( bloodPressureMeasure.measures.First(x => x.type == SystolicBloodPressureMeasureTypeId).value * Math.Pow(10, bloodPressureMeasure.measures.First(x => x.type == SystolicBloodPressureMeasureTypeId).unit)),
                    });
                }

                return bloodPressures;
            }
            else
            {
                throw new Exception($"Error calling nokia api , status code is {response.StatusCode} , and content is {await response.Content.ReadAsStringAsync()}");
            }
        }




    }
}