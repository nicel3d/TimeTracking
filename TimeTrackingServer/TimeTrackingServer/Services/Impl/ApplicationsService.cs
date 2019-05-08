using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Data;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;
using System.Linq.Dynamic;

namespace TimeTrackingServer.Services.Impl
{
    public class ApplicationsService : IApplicationsService
    {
        ApplicationDbContext _dbContext;

        public ApplicationsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            _dbContext.Remove(Get(id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Applications> Get(int id)
        {
            return await _dbContext.Applications.FindAsync(id);
        }

        public async Task<ApplicationsRangeListResponse> Get(TableSortingWithFilterRequest request)
        {
            var data = _dbContext.Set<Applications>()
                                .GroupJoin(_dbContext.ActivityStaff, app => app.Id,
                                            f => f.ApplicationId, (app, f) => new
                                            {
                                                app,
                                                act = f
                                                .Where(x => x.UpdatedAt > request.Filter.BegDate && x.UpdatedAt < request.Filter.EndDate)
                                                .Where(x => x.UpdatedAt.Hour > request.Filter.BegHour && x.UpdatedAt.Hour < request.Filter.EndHour)
                                                .GroupBy(x => x.StaffId)
                                            }
                                )
                                .Select(x => new ApplicationsRange
                                {
                                    Id = x.app.Id,
                                    Caption = x.app.Caption,
                                    State = x.app.State,
                                    UpdatedAt = x.app.UpdatedAt,
                                    UserUsed = x.act.Count()
                                })
                                .Where(x => x.UserUsed > 0);

            IQueryable<ApplicationsRange> dataSearch = null;

            if (!String.IsNullOrEmpty(request.Search))
            {
                dataSearch = data.AsQueryable().Where(x =>
                            x.Caption.Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            if (request.Sorting.SortBy != null && request.Sorting.Descending != null)
            {
                var order = request.Sorting.Descending != true ? "DESC" : "ASC";
                dataSearch = (dataSearch ?? data).AsQueryable().OrderBy($"{request.Sorting.SortBy} {order}");
            }

            var dataSlipTake = (dataSearch ?? data).Skip(request.Skip ?? 0)
                                .Take(request.Take ?? Int32.MaxValue);

            return new ApplicationsRangeListResponse
            {
                Data = await dataSlipTake.ToListAsync(),
                Total = await (dataSearch ?? data).CountAsync()
            };
        }

        public async Task<ApplicationsListResponse> Get(TableSortingRequest request)
        {
            var data = _dbContext.Applications;

            IQueryable<Applications> dataSearch = null;

            if (!String.IsNullOrEmpty(request.Search))
            {
                dataSearch = data.AsQueryable().Where(x =>
                            x.Caption.Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            if (request.Sorting.SortBy != null && request.Sorting.Descending != null)
            {
                var order = request.Sorting.Descending != true ? "DESC" : "ASC";
                dataSearch = (dataSearch ?? data).AsQueryable().OrderBy($"{request.Sorting.SortBy} {order}");
            }

            var dataSlipTake = (dataSearch ?? data).Skip(request.Skip ?? 0)
                                .Take(request.Take ?? Int32.MaxValue);

            return new ApplicationsListResponse
            {
                Data = await dataSlipTake.ToListAsync(),
                Total = await (dataSearch ?? data).CountAsync()
            };
        }

        public async Task<Applications> Post(Applications applications)
        {
            _dbContext.Applications.Add(applications);
            await _dbContext.SaveChangesAsync();
            return applications;
        }

        public async Task PutState(int id, StateEnum stateEnum)
        {
            var applications = await _dbContext.Applications.FindAsync(id);
            _dbContext.Applications.Attach(applications);
            applications.State = stateEnum;
            await _dbContext.SaveChangesAsync();
        }
    }
}
