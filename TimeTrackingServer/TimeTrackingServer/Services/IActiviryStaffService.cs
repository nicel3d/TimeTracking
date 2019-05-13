using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public class ActivityStaffListResponse : ListCountResponse
    {
        public List<ActivityStaffThumb> Data { get; set; }
    }

    public class ActivityStaffThumb : ActivityStaff
    {
        public new string ImageThumb { get; set; }
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
