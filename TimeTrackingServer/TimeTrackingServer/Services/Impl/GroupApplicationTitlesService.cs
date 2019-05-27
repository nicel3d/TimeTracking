using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Data;
using TimeTrackingServer.Models;
using System.Linq.Dynamic;
using TimeTrackingServer.Exceptions;

namespace TimeTrackingServer.Services.Impl
{
    public class GroupApplicationTitlesService : IGroupApplicationTitlesService
    {
        ApplicationDbContext _dbContext;

        public GroupApplicationTitlesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            var item = await Get(id);
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApplicationTitleToGroup> Get(int id)
        {
            return await _dbContext.ApplicationTitleToGroup.FindAsync(id);
        }

        public async Task<GroupApplicationTitleListVM> Get(ApplicationGroupFilterRequest request, bool withSkipTake = true)
        {
            IQueryable<GroupApplicationTitlesVM> data = _dbContext.Set<ApplicationTitleToGroup>()
                                                .Where(x => x.GroupId == request.GroupId)
                                                .Include(x => x.ApplicationTitle).ThenInclude(x => x.Application)
                                                .Select(x => new GroupApplicationTitlesVM
                                                {
                                                    Id = x.Id,
                                                    ApplicationId = x.ApplicationTitle.ApplicationId,
                                                    GroupId = x.GroupId,
                                                    State = x.State,
                                                    UpdatedAt = x.UpdatedAt,
                                                    ApplicationTitle = x.ApplicationTitle.Title,
                                                    ApplicationCaption = x.ApplicationTitle.Application.Caption
                                                });

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.Where(x =>
                                x.ApplicationTitle.Contains(request.Search) ||
                                x.ApplicationCaption.Contains(request.Search) ||
                                x.UpdatedAt.ToString().Contains(request.Search)
                            );
            }

            return new GroupApplicationTitleListVM
            {
                Data = await (withSkipTake ? data.AddSkipTake(request) : data).Sort(request.Sorting).ToListAsync(),
                Total = await data.CountAsync()
            };
        }
        public async Task<ApplicationTitleToGroup> Post(ApplicationTitleToGroup applicationTitleToGroup)
        {
            _dbContext.ApplicationTitleToGroup.Add(applicationTitleToGroup);
            await _dbContext.SaveChangesAsync();
            return applicationTitleToGroup;
        }
        public async Task Put(int id, ApplicationTitleToGroup group)
        {
            _dbContext.DetachLocal(group, id);
            await _dbContext.SaveChangesAsync();
        }
    }
}
