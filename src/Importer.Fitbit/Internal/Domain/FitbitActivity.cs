using System.Collections.Generic;
using Newtonsoft.Json;

namespace Importer.Fitbit.Internal.Domain
{

    internal class FitBitActivity
    {
        [JsonProperty("activities-heart") ]
        internal List<ActivitiesHeart> activitiesHeart { get; set; }
    }
    
}
