using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TimeTrackingServer.Models;
using TimeTrackingServer.Services;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Controllers
{
    [Authorize]
    [ApiController]
    [EnableCors("cors")]
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpPost(nameof(GetList))]
        [Produces("application/json")]
        public async Task<List<Settings>> GetList()
        {
            return await _settingsService.Get();
        }

        [HttpPost(nameof(Post))]
        [Produces("application/json")]
        public async Task<List<Settings>> Post([FromBody] List<Settings> settings)
        {
            return await _settingsService.Post(settings);
        }
    }
}
