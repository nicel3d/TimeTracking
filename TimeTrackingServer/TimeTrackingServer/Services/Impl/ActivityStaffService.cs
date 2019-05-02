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
using TimeTrackingServer.Stores.Impl;

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

        public async Task<ActivityStaffListResponse> Get(SortingAndSkipTakeRequest request)
        {
            var data = _dbContext.Set<ActivityStaff>()
                                .Include(x => x.Application)
                                .ThenInclude(x => x.ActivityStaff)
                                .Include(x => x.Staff)
                                .ThenInclude(x => x.ActivityStaff)
                                .Skip(request.Skip ?? 0)
                                .Take(request.Take ?? Int32.MaxValue);

            var dataCount = _dbContext.ActivityStaff;

            if (request.Descending == false)
            {
                data = data.OrderByDescending(x => x.UpdatedAt);
            }
            else
            {
                data = data.OrderBy(x => x.UpdatedAt);
            }

            return new ActivityStaffListResponse
            {
                Data = await data.Select(x => new ActivityStaff
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
                                .ToListAsync(),
                Count = await dataCount.CountAsync()
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
