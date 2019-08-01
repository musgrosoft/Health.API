using System.Collections.Generic;
using Newtonsoft.Json;

namespace Importer.Fitbit.Internal.Domain
{

    public class FitBitActivity
    {
        [JsonProperty("activities-heart") ]
        public List<ActivitiesHeart> activitiesHeart { get; set; }
    }
    
}
