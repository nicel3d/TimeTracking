using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Stores
{
    public interface IActiviryStaffStore
    {
        Task<List<ActivityStaff>> GetAll();
    }
}
