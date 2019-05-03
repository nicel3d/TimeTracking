using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Data;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

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

        public async Task<ApplicationsListResponse> Get(SortingSearchSkipTakeRequest request)
        {
            return new ApplicationsListResponse
            {
                Data = await _dbContext.Applications.ToListAsync(),
                Total = await _dbContext.Applications.CountAsync()
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
