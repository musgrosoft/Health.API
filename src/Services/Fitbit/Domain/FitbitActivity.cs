﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Services.Fitbit.Domain
{

    public class FitBitActivity
    {
        [JsonProperty("activities-heart") ]
        public List<ActivitiesHeart> activitiesHeart { get; set; }
    }
    
}
