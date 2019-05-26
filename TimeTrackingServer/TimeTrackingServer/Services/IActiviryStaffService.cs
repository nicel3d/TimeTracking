using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Constants;
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

    public class ActivityStatisticResponse
    {
        public string TimeAllowedApplication { get; set; }
        public string TimeForbiddenApplication { get; set; }
        public string TimeAllApplication { get; set; }
    }

    public class ActivityStaffResponse
    {
        public int StaffId { get; set; }
        public string StaffAlias { get; set; }
        public StateEnum StatusApplication { get; set; }
        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public interface IActivityStaffService
    {
        Task<ActivityStaff> Get(int id);
        Task<ActivityStaff> Post(ActivityStaff activityStaff);
        Task Put(int id, ActivityStaff activityStaff);
        Task Delete(int id);
        Task<ActivityStaffListResponse> Get(TableSortingWithFilterRequest request);
        Task<ActivityStatisticResponse> GetStatisticByDate(DateTime request);
        Task<List<ActivityStaffResponse>> GetActivityStaffByDate(DateTime request);
    }
}
