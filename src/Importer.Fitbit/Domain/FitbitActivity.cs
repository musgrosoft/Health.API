using System.Collections.Generic;
using Newtonsoft.Json;

namespace Importer.Fitbit.Domain
{

    public class FitBitActivity
    {
        [JsonProperty("activities-heart") ]
        public List<ActivitiesHeart> activitiesHeart { get; set; }
    }
    
}
