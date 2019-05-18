using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Services
{
    public enum SocketStatusEnum
    {
        Disconect,
        Connect
    }

    public class StaffWithSocketStatus
    {
        public string StaffAlias { get; set; }
        public SocketStatusEnum StaffStatus { get; set; }
    }

    public class StreamingDataRequest
    {
        public string ApplicationAlias { get; set; }
        public string ApplicationTitle { get; set; }
        public byte[] ApplicationImage { get; set; }
        public string StaffAlias { get; set; }
        public Int32 ActivityTime { get; set; }
    }

    public interface IStreamingDataService
    {
        Task AddActivity(StreamingDataRequest editActivityStaffRequest);
    }
}
