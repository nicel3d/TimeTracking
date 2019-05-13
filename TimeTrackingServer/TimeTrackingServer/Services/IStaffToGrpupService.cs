using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public interface IStaffToGroupService
    {
        Task<StaffToGroup> Post(StaffToGroup staffToGroup);
        Task Delete(StaffToGroup staffToGroup);
    }
}
