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
using TimeTrackingServer.Stores;

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

        public async Task<ActivityStaffListResponse> Get(SkipTakeRequest skipTakeRequest)
        {
            var data = await _dbContext.Set<ActivityStaff>()
                                        .Include(x => x.Application)
                                        .ThenInclude(x => x.ActivityStaff)
                                        .Include(x => x.Staff)
                                        .ThenInclude(x => x.ActivityStaff)
                                        .Skip(skipTakeRequest.Skip ?? 0)
                                        .Take(skipTakeRequest.Take ?? Int32.MaxValue)
                                        .Select(x => new ActivityStaff
                                        {
                                            Id = x.Id,
                                            Staff = x.Staff,
                                            ImageThumb = x.ImageThumb,
                                            UpdatedAt = x.UpdatedAt,
                                            Application = x.Application,
                                            ApplicationTitle = x.ApplicationTitle,
                                            ApplicationId = x.ApplicationId,
                                            StaffId = x.StaffId
                                        })
                                        .ToListAsync();

            var count = await _dbContext.ActivityStaff.CountAsync();

            return new ActivityStaffListResponse
            {
                Data = data,
                Count = count
            };
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
