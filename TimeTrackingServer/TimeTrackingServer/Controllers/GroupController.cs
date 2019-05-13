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
    public class GroupController : Controller
    {
        private IGroupService _groupService;
        private IStaffToGroupService _staffToGroup;

        public GroupController(IGroupService groupService, IStaffToGroupService staffToGroup)
        {
            _groupService = groupService;
            _staffToGroup = staffToGroup;
        }

        [HttpPost(nameof(GetList))]
        [Produces("application/json")]
        public async Task<GroupListResponse> GetList([FromBody] TableSortingRequest request)
        {
            return await _groupService.Get(request);
        }

        [HttpPost(nameof(GetListWithCountUsers))]
        [Produces("application/json")]
        public async Task<GroupsListWithCountUsersResponse> GetListWithCountUsers([FromBody] TableSortingRequest request)
        {
            return await _groupService.GetListWithCountUsers(request);
        }

        [HttpPost(nameof(ImportXLSXGetListWithoutFilter))]
        public async Task<ActionResult> ImportXLSXGetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return File(await _groupService.ImportXLSXGetListWithoutFilter(request), "application/ms-excel");
        }

        [HttpPost(nameof(ImportCSVGetListWithoutFilter))]
        public async Task<ActionResult> ImportCSVGetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return File(await _groupService.ImportCSVGetListWithoutFilter(request), "text/csv");
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<Groups> Get(int id)
        {
            return await _groupService.Get(id);
        }

        [HttpPost(nameof(Post))]
        [Produces("application/json")]
        public async Task<Groups> Post([FromBody] Groups group)
        {
            return await _groupService.Post(group);
        }

        [HttpPost(nameof(PostStaffToGroup))]
        [Produces("application/json")]
        public async Task<StaffToGroup> PostStaffToGroup([FromBody] StaffToGroup staffToGroup)
        {
            return await _staffToGroup.Post(staffToGroup);
        }

        [HttpDelete(nameof(DeleteStaffToGroup))]
        public async Task DeleteStaffToGroup(StaffToGroup staffToGroup)
        {
            await _staffToGroup.Delete(staffToGroup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Groups group)
        {
            if (id != group.Id)
            {
                throw new ApiDontValidIdRequest();
            }
            await _groupService.Put(id, group);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _groupService.Delete(id);
        }
    }
}
