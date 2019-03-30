using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Data;
using TimeTrackingServer.Models;

namespace TimeTrackingServer.Stores.Impl
{
    public class ActiviryStaffStore : IActiviryStaffStore
    {
        private readonly ApplicationDbContext _dbContext;

        public ActiviryStaffStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ActivityStaff>> GetAll()
        {
            return await _dbContext.ActivityStaff.ToListAsync();
            //return await _dbContext.ActivityStaff.Where(x => x. == "Aa").ToListAsync();
        }
    }
}
