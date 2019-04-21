using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Services
{
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
        Task<Staff> AddAndGetStaff(string staffAlias);
        Task<Applications> AddAndGetApplication(string applicationAlias);
        Task AddActivity(StreamingDataRequest editActivityStaffRequest);
    }
}
