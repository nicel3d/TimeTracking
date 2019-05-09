using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public class ActivityStaffListResponse : ListCountResponse
    {
        public List<ActivityStaff> Data { get; set; }
    }

    public interface IActivityStaffService
    {
        Task<ActivityStaff> Get(int id);
        Task<ActivityStaff> Post(ActivityStaff activityStaff);
        Task Put(int id, ActivityStaff activityStaff);
        Task Delete(int id);
        Task<ActivityStaffListResponse> Get(TableSortingWithFilterRequest request);
    }
}
