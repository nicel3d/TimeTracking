using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public class GroupListResponse : ListCountResponse
    {
        public List<Groups> Data { get; set; }
    }
    public class GroupsWithCountUsers : Groups
    {
        public int CountUsers { get; set; }
    }
    public class GroupsListWithCountUsersResponse : ListCountResponse
    {
        public List<GroupsWithCountUsers> Data { get; set; }
    }

    public class TableSortingByIdRequest : TableSortingRequest
    {
        public string Id { get; set; }
    }

    public interface IGroupService
    {
        Task<Groups> Get(int id);
        Task<Groups> Post(Groups group);
        Task Put(int id, Groups group);
        Task Delete(int id);
        Task<GroupListResponse> Get(TableSortingRequest request, bool withSkipTake = true);
        Task<GroupsListWithCountUsersResponse> GetListWithCountUsers(TableSortingRequest request, bool withSkipTake = true);
        //Task<GroupListResponse> GetByUserId(TableSortingByIdRequest request, bool withSkipTake = true);
        Task<byte[]> ImportXLSXGetListWithoutFilter(TableSortingRequest request);
        Task<byte[]> ImportCSVGetListWithoutFilter(TableSortingRequest request);
    }
}
