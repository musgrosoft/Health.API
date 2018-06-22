using System.Collections.Generic;

namespace HealthAPI.Acceptance.Tests.Domain
{
    public class ODataResponse<T>
    {
        public List<T> value { get; set; }
    }
}