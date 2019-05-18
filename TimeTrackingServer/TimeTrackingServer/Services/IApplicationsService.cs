using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public class ApplicationsListResponse : ListCountResponse
    {
        public List<Applications> Data { get; set; }
    }

    public class ApplicationsRange : Applications
    {
        public int UserUsed { get; set; }
    }

    public class ApplicationsRangeListResponse : ListCountResponse
    {
        public List<ApplicationsRange> Data { get; set; }
    }

    public interface IApplicationsService
    {
        Task<Applications> Get(int id);
        Task<ApplicationsRangeListResponse> Get(TableSortingWithFilterRequest request);
        Task<ApplicationsListResponse> Get(TableSortingRequest request, bool withSkipTake = true);
        Task<Applications> GetOrAddApplicationByAlias(string applicationAlias);
        Task<byte[]> ExportXLSXGetListWithoutFilter(TableSortingRequest request);
        Task<byte[]> ExportCSVGetListWithoutFilter(TableSortingRequest request);
        Task<Applications> Post(Applications activityStaff);
        Task PutState(int id, StateEnum stateEnum);
        Task Delete(int id);
    }
}
