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
    public class ActivityStaffService : IActivityStaffService
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

        public async Task<List<ActivityStaffFull>> GetAll()
        {

            return await (from activity in _dbContext.ActivityStaff
                          where activity.ApplicationId != null && activity.StaffId != null
                          //join app in _dbContext.Applications on activity.ApplicationId equals app.Id
                          join s in _dbContext.Staff on activity.StaffId equals s.Id
                          select new ActivityStaffFull
                          {
                              ApplicationId = activity.ApplicationId,
                              ApplicationTitle = activity.ApplicationTitle,
                              Id = activity.Id,
                              StaffId = activity.StaffId,
                              Staff = (Staff)s,
                              StaffAlias = s.Caption,
                              ApplicationName = null,
                              ImageOrigin = activity.ImageOrigin,
                              ImageThumb = activity.ImageThumb,
                              Application = null,
                              UpdatedAt = activity.UpdatedAt
                          }
                    )
                        .Skip(0)
                        .Take(3)
                    .ToListAsync();
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
