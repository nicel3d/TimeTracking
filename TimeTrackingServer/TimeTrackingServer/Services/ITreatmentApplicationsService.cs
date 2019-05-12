using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public class VMApplicationGroup : ApplicationToGroup
    {
        public string ApplicationTitle { get; set; }
    }

    public class VMApplicationGroupListResponse : ListCountResponse
    {
        public List<VMApplicationGroup> Data { get; set; }
    }

    public class ApplicationGroupFilterRequest : TableSortingRequest
    {
        public int GroupId { get; set; }
    }
    public interface ITreatmentApplicationsService
    {
        Task<ApplicationToGroup> Get(int id);
        Task<ApplicationToGroup> Post(ApplicationToGroup group);
        Task Put(int id, ApplicationToGroup group);
        Task Delete(int id);
        Task<VMApplicationGroupListResponse> Get(ApplicationGroupFilterRequest request, bool withSkipTake = true);
        Task<byte[]> ImportXLSXGetListWithoutFilter(ApplicationGroupFilterRequest request);
        Task<byte[]> ImportCSVGetListWithoutFilter(ApplicationGroupFilterRequest request);
    }
}
