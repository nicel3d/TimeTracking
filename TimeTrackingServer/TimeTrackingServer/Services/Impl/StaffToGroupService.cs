using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Data;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;
using System.Linq.Dynamic;
using TimeTrackingServer.Exceptions;
using System.Text;
using OfficeOpenXml;

namespace TimeTrackingServer.Services.Impl
{
    public class StaffToGroupService : IStaffToGroupService
    {
        ApplicationDbContext _dbContext;

        public StaffToGroupService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<StaffToGroup> Post(StaffToGroup staffToGroup)
        {
            _dbContext.StaffToGroup.Add(staffToGroup);
            await _dbContext.SaveChangesAsync();
            return staffToGroup;
        }
        public async Task Delete(StaffToGroup staffToGroup)
        {
            var item = await _dbContext.StaffToGroup
                .Where(x => x.StaffId == staffToGroup.StaffId)
                .Where(x => x.GroupId == staffToGroup.GroupId)
                .FirstAsync();

            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
