using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrackingClient.Constants
{
    class StreamingData : ApplicationStreamingData
    {
        public string StaffAlias { get; set; }
        public Int32 ActivityTime { get; set; }
    }

    public class ApplicationStreamingData
    {
        public string ApplicationAlias { get; set; }
        public string ApplicationTitle { get; set; }
        public string ApplicationImage { get; set; }
    }
}
