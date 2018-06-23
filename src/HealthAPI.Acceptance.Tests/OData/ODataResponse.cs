using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAPI.Acceptance.Tests.OData
{
    public class ODataResponse<T>
    {
        public List<T> value { get; set; }
    }
}
