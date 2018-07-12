using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Services.Fitbit.Domain.Detailed
{
    public class HeartRateZone
    {
        public double caloriesOut { get; set; }
        public int max { get; set; }
        public int min { get; set; }
        public int minutes { get; set; }
        public string name { get; set; }
    }

    public class Value
    {
        public List<object> customHeartRateZones { get; set; }
        public List<HeartRateZone> heartRateZones { get; set; }
        public int restingHeartRate { get; set; }
    }

    public class ActivitiesHeart
    {
        public DateTime dateTime { get; set; }
        public Value value { get; set; }
    }

    public class Dataset
    {
        public DateTime time { get; set; }
        public int value { get; set; }
    }

    public class ActivitiesHeartIntraday
    {
        public List<Dataset> dataset { get; set; }
        public int datasetInterval { get; set; }
        public string datasetType { get; set; }
    }

    public class RootObject
    {
        [JsonProperty("activities-heart")]
        public List<ActivitiesHeart> ActivitiesHeart { get; set; }
        [JsonProperty("activities-heart-intraday")]
        public ActivitiesHeartIntraday ActivitiesHeartIntraday { get; set; }
    }
}
