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
    public class ApplicationTitlesService : IApplicationTitlesService
    {
        ApplicationDbContext _dbContext;

        public ApplicationTitlesService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            var item = await Get(id);
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApplicationTitles> Get(int id)
        {
            return await _dbContext.ApplicationTitles.FindAsync(id);
        }

        public async Task<ApplicationTitlesListVM> Get(ApplicationTitlesFilterRequest request, bool withSkipTake = true)
        {
            IQueryable<ApplicationTitles> data = _dbContext.Set<ApplicationTitles>()
                                                .Where(x => x.ApplicationId == request.ApplicationId)
                                                .Include(x => x.Application);

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.Where(x =>
                                x.Application.Caption.Contains(request.Search) ||
                                x.UpdatedAt.ToString().Contains(request.Search)
                            );
            }

            return new ApplicationTitlesListVM
            {
                Data = await (withSkipTake ? data.AddSkipTake(request) : data).Sort(request.Sorting).ToListAsync(),
                Total = await data.CountAsync()
            };
        }
        public async Task<ApplicationTitles> Post(ApplicationTitles applicationTitles)
        {
            _dbContext.ApplicationTitles.Add(applicationTitles);
            await _dbContext.SaveChangesAsync();
            return applicationTitles;
        }
        public async Task Put(int id, ApplicationTitles group)
        {
            _dbContext.DetachLocal(group, id);
            await _dbContext.SaveChangesAsync();
        }
    }
}
