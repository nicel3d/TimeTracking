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
        public async Task<StaffWithTimeActivityListResponse> GetList([FromBody] TableSortingByGroupIdRequest request)
        {
            return await _staffService.GetListWithTimeActivity(request);
        }

        [HttpPost(nameof(GetListFull))]
        [Produces("application/json")]
        public async Task<List<StaffWithTimeActivity>> GetListFull([FromBody] TableSortingByGroupIdRequest request)
        {
            return (await _staffService.GetListWithTimeActivity(request, false)).Data;
        }

        [HttpPost(nameof(GetListOnlyByGropupId))]
        [Produces("application/json")]
        public async Task<List<Staff>> GetListOnlyByGropupId([FromBody] int groupId)
        {
            return await _staffService.GetListOnlyByGropupId(groupId);
        }

        [HttpPost(nameof(ImportXLSXGetListWithoutFilter))]
        public async Task<ActionResult> ImportXLSXGetListWithoutFilter([FromBody] TableSortingByGroupIdRequest request)
        {
            return File(await _staffService.ImportXLSXGetListWithoutFilter(request), "application/ms-excel");
        }

        [HttpPost(nameof(ImportCSVGetListWithoutFilter))]
        public async Task<ActionResult> ImportCSVGetListWithoutFilter([FromBody] TableSortingByGroupIdRequest request)
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

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _staffService.Delete(id);
        }
    }
}
