using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public interface ISettingsService
    {
        Task<List<Settings>> Get();
        Task<List<Settings>> Post(List<Settings> settings);
    }
}
