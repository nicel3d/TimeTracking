using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public class StaffListResponse : ListCountResponse
    {
        public List<Staff> Data { get; set; }
    }

    public class TableSortingByGroupIdRequest : TableSortingRequest
    {
        public int? GroupId { get; set; }
    }

    public interface IStaffService
    {
        Task<Staff> Get(int id);
        Task<Staff> Post(Staff staff);
        Task<Staff> GetOrAddStaffByAlias(string staffAlias);
        Task Delete(int id);
        Task<StaffListResponse> Get(TableSortingByGroupIdRequest request, bool withSkipTake = true);
        Task<List<Staff>> GetListOnlyByGropupId(int groupId);
        Task<byte[]> ExportXLSXGetListWithoutFilter(TableSortingByGroupIdRequest request);
        Task<byte[]> ExportCSVGetListWithoutFilter(TableSortingByGroupIdRequest request);
        Task SetTimeConnectingStaffByStaffAlias(string staffAlias);
        Task SetTimeDisconectingStaffByStaffAlias(string staffAlias);
        string GetHMS(Int32 int32);
    }
}
