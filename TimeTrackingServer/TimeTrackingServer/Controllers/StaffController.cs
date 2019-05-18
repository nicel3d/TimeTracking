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
    public class StaffController : Controller
    {
        private IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost(nameof(GetList))]
        [Produces("application/json")]
        public async Task<StaffListResponse> GetList([FromBody] TableSortingByGroupIdRequest request)
        {
            return await _staffService.Get(request);
        }

        [HttpPost(nameof(GetListFull))]
        [Produces("application/json")]
        public async Task<List<Staff>> GetListFull([FromBody] TableSortingByGroupIdRequest request)
        {
            return (await _staffService.Get(request, false)).Data;
        }

        [HttpPost(nameof(GetListOnlyByGropupId))]
        [Produces("application/json")]
        public async Task<List<Staff>> GetListOnlyByGropupId([FromBody] int groupId)
        {
            return await _staffService.GetListOnlyByGropupId(groupId);
        }

        [HttpPost(nameof(ExportXLSXGetListWithoutFilter))]
        public async Task<ActionResult> ExportXLSXGetListWithoutFilter([FromBody] TableSortingByGroupIdRequest request)
        {
            return File(await _staffService.ExportXLSXGetListWithoutFilter(request), "application/ms-excel");
        }

        [HttpPost(nameof(ExportCSVGetListWithoutFilter))]
        public async Task<ActionResult> ExportCSVGetListWithoutFilter([FromBody] TableSortingByGroupIdRequest request)
        {
            return File(await _staffService.ExportCSVGetListWithoutFilter(request), "text/csv");
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

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _staffService.Delete(id);
        }
    }
}
