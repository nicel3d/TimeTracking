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
    public class ApplicationsModeController : Controller
    {
        private IApplicationsModeService _applicationsModeService;

        public ApplicationsModeController(IApplicationsModeService applicationsModeService)
        {
            _applicationsModeService = applicationsModeService;
        }

        [HttpPost(nameof(GetList))]
        [Produces("application/json")]
        public async Task<VMApplicationGroupListResponse> GetList([FromBody] ApplicationGroupFilterRequest request)
        {
            return await _applicationsModeService.Get(request);
        }

        [HttpPost(nameof(ExportXLSXGetListWithoutFilter))]
        public async Task<ActionResult> ExportXLSXGetListWithoutFilter([FromBody] ApplicationGroupFilterRequest request)
        {
            return File(await _applicationsModeService.ExportXLSXGetListWithoutFilter(request), "application/ms-excel");
        }

        [HttpPost(nameof(ExportCSVGetListWithoutFilter))]
        public async Task<ActionResult> ExportCSVGetListWithoutFilter([FromBody] ApplicationGroupFilterRequest request)
        {
            return File(await _applicationsModeService.ExportCSVGetListWithoutFilter(request), "text/csv");
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ApplicationToGroup> Get(int id)
        {
            return await _applicationsModeService.Get(id);
        }

        [HttpPost(nameof(Post))]
        [Produces("application/json")]
        public async Task<ApplicationToGroup> Post([FromBody] ApplicationToGroup request)
        {
            return await _applicationsModeService.Post(request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ApplicationToGroup request)
        {
            if (id != request.Id)
            {
                throw new ApiDontValidIdRequest();
            }
            await _applicationsModeService.Put(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _applicationsModeService.Delete(id);
        }
    }
}
