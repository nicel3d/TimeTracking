using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Data;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;
using System.Linq.Dynamic;

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

        public async Task<ActivityStaffListResponse> Get(TableSortingWithFilterRequest request)
        {
            var data = _dbContext.Set<ActivityStaff>()
                                .Include(x => x.Application)
                                .Include(x => x.Staff)
                                .Where(x => x.UpdatedAt > request.Filter.BegDate && x.UpdatedAt < request.Filter.EndDate)
                                .Where(x => x.UpdatedAt.Hour > request.Filter.BegHour && x.UpdatedAt.Hour < request.Filter.EndHour);

            IQueryable<ActivityStaff> dataSearch = null;

            if (!String.IsNullOrEmpty(request.Search))
            {
                dataSearch = data.AsQueryable().Where(x =>
                            x.Application.Caption.Contains(request.Search) ||
                            x.Staff.Caption.Contains(request.Search) ||
                            x.ApplicationTitle.Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            if (request.Sorting.SortBy != null && request.Sorting.Descending != null)
            {
                var order = request.Sorting.Descending != true ? "DESC" : "ASC";
                dataSearch = (dataSearch ?? data).AsQueryable().OrderBy($"{request.Sorting.SortBy} {order}");
            }

            var dataSlipTake = (dataSearch ?? data).Skip(request.Skip ?? 0)
                                .Take(request.Take ?? Int32.MaxValue)
                                .Select(x =>
                                new ActivityStaff
                                {
                                    Id = x.Id,
                                    Staff = x.Staff,
                                    ImageThumb = x.ImageThumb,
                                    UpdatedAt = x.UpdatedAt,
                                    Application = x.Application,
                                    ApplicationTitle = x.ApplicationTitle,
                                    ApplicationId = x.ApplicationId,
                                    StaffId = x.StaffId
                                });

            return new ActivityStaffListResponse
            {
                Data = await dataSlipTake.ToListAsync(),
                Total = await (dataSearch ?? data).CountAsync()
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
