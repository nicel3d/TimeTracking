using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TimeTrackingServer.Models;
using TimeTrackingServer.Services;
using TimeTrackingServer.Stores.Impl;
using static TimeTrackingServer.Exceptions.ApiException;

namespace TimeTrackingServer.Controllers
{
    [Authorize]
    [ApiController]
    [EnableCors("cors")]
    [Route("api/[controller]")]
    public class ApplicationTitlesController : Controller
    {
        private IApplicationTitlesService _applicationTitlesService;

        public ApplicationTitlesController(IApplicationTitlesService applicationTitlesService)
        {
            _applicationTitlesService = applicationTitlesService;
        }

        [HttpPost(nameof(GetList))]
        [Produces("application/json")]
        public async Task<ApplicationTitlesListVM> GetList([FromBody] ApplicationGroupFilterRequest request)
        {
            return await _applicationTitlesService.Get(request);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ApplicationTitles> Get(int id)
        {
            return await _applicationTitlesService.Get(id);
        }

        [HttpPost(nameof(Post))]
        [Produces("application/json")]
        public async Task<ApplicationTitles> Post([FromBody] ApplicationTitles request)
        {
            return await _applicationTitlesService.Post(request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ApplicationTitles request)
        {
            if (id != request.Id)
            {
                throw new ApiDontValidIdRequest();
            }
            await _applicationTitlesService.Put(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _applicationTitlesService.Delete(id);
        }
    }
}
