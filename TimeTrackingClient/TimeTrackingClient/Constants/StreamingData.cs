using System;

namespace TimeTrackingClient.Constants
{
    public class StaffConnection
    {
        public string StaffAlias { get; set; }
        public bool Connection { get; set; }
    }

    class StreamingData : ApplicationStreamingData
    {
        public string StaffAlias { get; set; }
        public Int32 ActivityTime { get; set; }
    }

    public class ApplicationStreamingData
    {
        public string ApplicationAlias { get; set; }
        public string ApplicationTitle { get; set; }
        public byte[] ApplicationImage { get; set; }
    }
}
