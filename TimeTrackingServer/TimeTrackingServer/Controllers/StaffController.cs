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
    public class StaffController : Controller
    {
        private IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost(nameof(GetList))]
        [Produces("application/json")]
        public async Task<StaffListResponse> GetList([FromBody] TableSortingRequest request)
        {
            return await _staffService.Get(request);
        }

        [HttpPost(nameof(ImportXLSXGetListWithoutFilter))]
        public async Task<ActionResult> ImportXLSXGetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return File(await _staffService.ImportXLSXGetListWithoutFilter(request), "application/ms-excel");
        }

        [HttpPost(nameof(ImportCSVGetListWithoutFilter))]
        public async Task<ActionResult> ImportCSVGetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return File(await _staffService.ImportCSVGetListWithoutFilter(request), "text/csv");
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<Staff> Get(int id)
        {
            return await _staffService.Get(id);
        }

        [HttpPost(nameof(Post))]
        [Produces("application/json")]
        public async Task<Staff> Post([FromBody] Staff staff)
        {
            return await _staffService.Post(staff);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Staff staff)
        {
            await _staffService.Put(id, staff);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _staffService.Delete(id);
        }
    }
}
