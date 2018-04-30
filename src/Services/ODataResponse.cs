using System.Collections.Generic;

namespace Services
{
    public class ODataResponse<T>
    {
        public List<T> value { get; set; }
        
    }
}