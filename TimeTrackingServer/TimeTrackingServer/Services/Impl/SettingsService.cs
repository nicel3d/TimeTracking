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
using System.Collections.Generic;

namespace TimeTrackingServer.Services.Impl
{
    public class SettingsService : ISettingsService
    {
        ApplicationDbContext _dbContext;

        public SettingsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Settings>> Get()
        {
            return await _dbContext.Settings.ToListAsync();
        }

        public async Task<List<Settings>> Post(List<Settings> settings)
        {
            //await _dbContext.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE Settings");
            var settingsDelete = await _dbContext.Settings.ToListAsync();
            settingsDelete.ForEach(x => _dbContext.Remove(x));
            await _dbContext.SaveChangesAsync();
            await _dbContext.Settings.AddRangeAsync(settings);
            await _dbContext.SaveChangesAsync();
            return await Get();
        }
    }
}
