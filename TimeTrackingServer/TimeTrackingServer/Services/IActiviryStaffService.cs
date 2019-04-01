using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Services
{
    public interface IActivityStaffService
    {
        Task<ActivityStaff> Get(int id);
        Task<ActivityStaff> Post(ActivityStaff activityStaff);
        Task Put(int id, ActivityStaff activityStaff);
        Task Delete(int id);
        Task<List<ActivityStaff>> GetAll();
    }
}
