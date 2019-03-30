using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Data;
using TimeTrackingServer.Helpers;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Services.Impl
{
    public class ActivityStaffService : IActiviryStaffService
    {
        ApplicationDbContext _dbContext;

        public ActivityStaffService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            _dbContext.Remove(Get(id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ActivityStaff> Get(int id)
        {
            return await _dbContext.ActivityStaff.FindAsync(id);
        }

        public async Task<List<ActivityStaff>> GetAll()
        {
            return await _dbContext.ActivityStaff.ToListAsync();
        }

        public async Task<ActivityStaff> Post(ActivityStaff activityStaff)
        {
            _dbContext.ActivityStaff.Update(activityStaff);
            await _dbContext.SaveChangesAsync();
            return activityStaff;
        }

        public async Task Put(int id, ActivityStaff activityStaff)
        {
            var oldActivityStaff = await _dbContext.ActivityStaff.FindAsync(id);
            if (oldActivityStaff != null)
            {
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
