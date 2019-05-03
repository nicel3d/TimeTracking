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

    public interface IApplicationsService
    {
        Task<Applications> Get(int id);
        Task<ApplicationsListResponse> Get(SortingSearchSkipTakeRequest request);
        Task<Applications> Post(Applications activityStaff);
        Task PutState(int id, StateEnum stateEnum);
        Task Delete(int id);
    }
}
