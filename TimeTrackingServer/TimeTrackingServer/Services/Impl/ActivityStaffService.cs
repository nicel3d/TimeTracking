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
            var item = await Get(id);
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ActivityStaff> Get(int id)
        {
            return await _dbContext.ActivityStaff.FindAsync(id);
        }

        public async Task<ActivityStaffListResponse> Get(TableSortingWithFilterRequest request)
        {
            IQueryable<ActivityStaff> data = _dbContext.Set<ActivityStaff>()
                                .WhereDateFilter(request.Filter)
                                .Include(x => x.Application)
                                .Include(x => x.Staff);

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.Where(x =>
                            x.Application.Caption.Contains(request.Search) ||
                            x.Staff.Caption.Contains(request.Search) ||
                            x.ApplicationTitle.Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            return new ActivityStaffListResponse
            {
                Data = await data.Sort(request.Sorting)
                                .AddSkipTake(request)
                                .Select(x =>
                                new ActivityStaffThumb
                                {
                                    Id = x.Id,
                                    Staff = x.Staff,
                                    ImageThumb = Convert.ToBase64String(x.ImageThumb),
                                    ImageOrigin = null,
                                    UpdatedAt = x.UpdatedAt,
                                    Application = x.Application,
                                    ApplicationTitle = x.ApplicationTitle,
                                    ApplicationId = x.ApplicationId,
                                    StaffId = x.StaffId
                                }).ToListAsync(),
                Total = await data.CountAsync()
            };
        }

        public async Task<ActivityStaff> Post(ActivityStaff activityStaff)
        {
            _dbContext.ActivityStaff.Add(activityStaff);
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
