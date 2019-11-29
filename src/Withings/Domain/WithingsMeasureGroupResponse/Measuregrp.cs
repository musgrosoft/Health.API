using System.Collections.Generic;

namespace Withings.Domain.WithingsMeasureGroupResponse
{

        public class Measuregrp
        {
            //public int grpid { get; set; }
            //public int attrib { get; set; }
            public int date { get; set; }
            //public int category { get; set; }
            public List<Measure> measures { get; set; }
            //public string comment { get; set; }
        }
    
}