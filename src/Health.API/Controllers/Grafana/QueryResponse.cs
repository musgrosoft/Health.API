using System.Collections.Generic;

namespace Health.API.Controllers
{
    public class QueryResponse
    {
        public string Target { get; set; }
        public List<double?[]> Datapoints { get; set; }
    }

//    public class Datapoint
//    {
//        public double Value { get; set; }
//        public double UnixTimestamp { get; set; }
//    }

}