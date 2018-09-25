using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fitbit.Domain
{
//        public class Dataset
//        {
//            public string time { get; set; }
//            public int value { get; set; }
//
//        public string theDateTime { get; set; }
//        }

//        public class ActivitiesHeartIntraday
//        {
//            public List<Dataset> dataset { get; set; }
//            public int datasetInterval { get; set; }
//            public string datasetType { get; set; }
//        }

        public class FitBitActivity
        {
            [JsonProperty("activities-heart") ]
            public List<ActivitiesHeart> activitiesHeart { get; set; }

//            [JsonProperty("activities-heart-intraday")]
//            public ActivitiesHeartIntraday activitiesHeartIntraday { get; set; }
        }
    
}
