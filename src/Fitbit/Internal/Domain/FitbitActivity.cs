using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fitbit.Internal.Domain
{

    public class FitBitActivity
    {
        [JsonProperty("activities-heart") ]
        public List<ActivitiesHeart> activitiesHeart { get; set; }
    }
    
}
