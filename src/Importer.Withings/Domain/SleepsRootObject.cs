﻿using System.Collections.Generic;

namespace Importer.Withings.Domain
{
    

    public class Data
    {
        public int wakeupduration { get; set; }
        public int lightsleepduration { get; set; }
        public int deepsleepduration { get; set; }
        public int wakeupcount { get; set; }
        public int durationtosleep { get; set; }
        public int remsleepduration { get; set; }
        public int durationtowakeup { get; set; }
        public int hr_average { get; set; }
        public int hr_min { get; set; }
        public int hr_max { get; set; }
        public int rr_average { get; set; }
        public int rr_min { get; set; }
        public int rr_max { get; set; }
    }

    public class Series
    {
        public int id { get; set; }
        public string timezone { get; set; }
        public int model { get; set; }
        public int startdate { get; set; }
        public int enddate { get; set; }
        public string date { get; set; }
        public Data data { get; set; }
        public int modified { get; set; }
    }

    public class Body
    {
        public List<Series> series { get; set; }
        public bool more { get; set; }
        public int offset { get; set; }
    }

    public class SleepsRootObject
    {
        public int status { get; set; }
        public Body body { get; set; }
    }
}