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
using System.Collections.Generic;

namespace TimeTrackingServer.Services.Impl
{
    public class ActivityStaffService : IActivityStaffService
    {
        ApplicationDbContext _dbContext;
        private IStaffService _staffService;

        public ActivityStaffService(ApplicationDbContext dbContext, IStaffService staffService)
        {
            _dbContext = dbContext;
            _staffService = staffService;
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


        public async Task<ActivityStatisticResponse> GetStatisticByDate(DateTime request)
        {
            List<ActivityStaff> activityStaff = await _dbContext.ActivityStaff
                .Where(x => x.UpdatedAt.Date == request.Date)
                .OrderBy(x => x.UpdatedAt)
                .Include(x => x.Application).ThenInclude(x => x.ApplicationToGroup)
                .Include(x => x.Staff).ThenInclude(x => x.StaffToGroup)
                .Select(x => new ActivityStaff
                {
                    Application = new Applications {
                        State = x.Application.State,
                        ApplicationToGroup = x.Application.ApplicationToGroup
                    },
                    Staff = new Staff
                    {
                        StaffToGroup = x.Staff.StaffToGroup,
                    }
                })
                .ToListAsync();

            int timeAllApplication = 0;
            int timeAllowedApplication = 0;
            int timeForbiddenApplication = 0;

            foreach (ActivityStaff item in activityStaff)
            {
                bool success = false;
                if (item.Application != null)
                {
                    if (item.Application.ApplicationToGroup != null)
                    {
                        foreach (ApplicationToGroup group in item.Application.ApplicationToGroup)
                        {
                            if (item.Staff.StaffToGroup.Any(x => x.GroupId == group.GroupId))
                            {
                                success = true;
                                switch (group.State)
                                {
                                    case Constants.StateEnum.Allowed:
                                        timeAllowedApplication += 5;
                                        break;
                                    case Constants.StateEnum.Forbidden:
                                        timeForbiddenApplication += 5;
                                        break;
                                    default:
                                        break;
                                }

                                timeAllApplication += 5;
                                break;
                            }
                        }
                    }

                    if (!success)
                    {
                        switch (item.Application.State)
                        {
                            case Constants.StateEnum.Allowed:
                                timeAllowedApplication += 5;
                                break;
                            case Constants.StateEnum.Forbidden:
                                timeForbiddenApplication += 5;
                                break;
                            default:
                                break;
                        }

                        timeAllApplication += 5;
                    }
                }
            }

            return new ActivityStatisticResponse
            {
                TimeAllApplication = _staffService.GetHMS(timeAllApplication),
                TimeAllowedApplication = _staffService.GetHMS(timeAllowedApplication),
                TimeForbiddenApplication = _staffService.GetHMS(timeForbiddenApplication)
            };
        }

        public async Task<List<ActivityStaffResponse>> GetActivityStaffByDate(DateTime request)
        {
            List<ActivityStaff> activityStaff = await _dbContext.Set<ActivityStaff>()
                .Include(x => x.Staff).ThenInclude(x => x.StaffToGroup)
                .Include(z => z.Application).ThenInclude(x => x.ApplicationToGroup)
                .Where(x => x.UpdatedAt.Date == request.Date)
                .OrderBy(x => x.UpdatedAt)
                .Select(x => new ActivityStaff
                {
                    UpdatedAt = x.UpdatedAt,
                    Application = new Applications
                    {
                        State = x.Application.State,
                        Caption = x.Application.Caption,
                        ApplicationToGroup = x.Application.ApplicationToGroup
                    },
                    Staff = new Staff
                    {
                        Caption = x.Staff.Caption,
                        StaffToGroup = x.Staff.StaffToGroup,
                    }
                })
                .ToListAsync();

            List<ActivityStaffResponse> activityStaffList = new List<ActivityStaffResponse>();

            const int iterval = 5;
            int timeAllApplicationOld = 0;
            int index = 0;

            foreach (ActivityStaff item in activityStaff)
            {
                int timeAllApplication = 0;
                bool success = false;
                if (item.Application != null)
                {
                    if (item.Application.ApplicationToGroup != null)
                    {
                        foreach (ApplicationToGroup group in item.Application.ApplicationToGroup)
                        {
                            if (item.Staff.StaffToGroup.Any(x => x.GroupId == group.GroupId))
                            {
                                success = true;

                                if (activityStaffList.Count() == index + 1 && activityStaffList[index].StatusApplication != group.State)
                                {
                                    index++;
                                }

                                if (activityStaffList.Count() < index + 1)
                                {
                                    activityStaffList.Add(new ActivityStaffResponse
                                    {
                                        BegDate = item.UpdatedAt,
                                        EndDate = item.UpdatedAt,
                                        StaffAlias = item.Staff.Caption,
                                        StatusApplication = group.State,
                                        StaffId = item.Staff.Id
                                    });
                                }
                                else
                                {
                                    activityStaffList[index].EndDate = item.UpdatedAt;
                                }

                                timeAllApplication += iterval;
                                break;
                            }
                        }
                    }

                    if (!success && item.Application.State != Constants.StateEnum.Neutral && item.Application.State != Constants.StateEnum.Forbidden)
                    {
                        //timeAllApplication += iterval;
                    }

                    if (timeAllApplicationOld == timeAllApplication)
                    {
                        index++;
                    }
                }
            }

            return activityStaffList;
        }
    }
}
