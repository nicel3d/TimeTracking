using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public class TableSortingByGroupIdRequest : TableSortingRequest
    {
        public int? GroupId { get; set; }
    }
    public class StaffWithTimeActivity : Staff
    {
        public string TimeActivity { get; set; }
    }
    public class StaffWithTimeActivityListResponse : ListCountResponse
    {
        public List<StaffWithTimeActivity> Data { get; set; }
    }
    public interface IStaffService
    {
        Task<Staff> Get(int id);
        Task<Staff> Post(Staff staff);
        Task<Staff> GetOrAddStaffByAlias(string staffAlias);
        Task Delete(int id);
        Task<StaffWithTimeActivityListResponse> GetListWithTimeActivity(TableSortingByGroupIdRequest request, bool withSkipTake = true);
        Task<List<Staff>> GetListOnlyByGropupId(int groupId);
        Task<byte[]> ImportXLSXGetListWithoutFilter(TableSortingByGroupIdRequest request);
        Task<byte[]> ImportCSVGetListWithoutFilter(TableSortingByGroupIdRequest request);
        Task SetTimeConnectingStaffByStaffAlias(string staffAlias);
        Task SetTimeDisconectingStaffByStaffAlias(string staffAlias);
    }
}
