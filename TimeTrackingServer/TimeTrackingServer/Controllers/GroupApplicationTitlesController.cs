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
    public class GroupApplicationTitlesController : Controller
    {
        private IGroupApplicationTitlesService _groupApplicationTitlesService;

        public GroupApplicationTitlesController(IGroupApplicationTitlesService groupApplicationTitlesService)
        {
            _groupApplicationTitlesService = groupApplicationTitlesService;
        }

        [HttpPost(nameof(GetList))]
        [Produces("application/json")]
        public async Task<GroupApplicationTitleListVM> GetList([FromBody] ApplicationGroupFilterRequest request)
        {
            return await _groupApplicationTitlesService.Get(request);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ApplicationTitleToGroup> Get(int id)
        {
            return await _groupApplicationTitlesService.Get(id);
        }

        [HttpPost(nameof(Post))]
        [Produces("application/json")]
        public async Task<ApplicationTitleToGroup> Post([FromBody] ApplicationTitleToGroup request)
        {
            return await _groupApplicationTitlesService.Post(request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ApplicationTitleToGroup request)
        {
            if (id != request.Id)
            {
                throw new ApiDontValidIdRequest();
            }
            await _groupApplicationTitlesService.Put(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _groupApplicationTitlesService.Delete(id);
        }
    }
}
