using System;

namespace Google.Domain
{
    public class Run
    {
        public DateTime CreatedDate { get; internal set; }
        public int Metres { get; internal set; }
        public TimeSpan Time { get; internal set; }
    }
}