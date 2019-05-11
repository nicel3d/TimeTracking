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
        public int GroupId { get; set; }
    }

    public interface IStaffService
    {
        Task<Staff> Get(int id);
        Task<Staff> Post(Staff staff);
        Task Put(int id, Staff staff);
        Task Delete(int id);
        Task<StaffListResponse> Get(TableSortingRequest request, bool withSkipTake = true);
        Task<List<Staff>> GetListByGropupId(TableSortingByGroupIdRequest request);
        Task<byte[]> ImportXLSXGetListWithoutFilter(TableSortingRequest request);
        Task<byte[]> ImportCSVGetListWithoutFilter(TableSortingRequest request);
    }
}
