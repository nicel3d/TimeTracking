using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Models;
using TimeTrackingServer.Services;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Controllers
{
    [Authorize]
    [ApiController]
    [EnableCors("cors")]
    [Route("api/[controller]")]
    public class ApplicationsController : Controller
    {
        private IApplicationsService _applicationsService;

        public ApplicationsController(IApplicationsService applicationsService)
        {
            _applicationsService = applicationsService;
        }

        [HttpPost(nameof(GetRangeList))]
        [Produces("application/json")]
        public async Task<ApplicationsRangeListResponse> GetRangeList([FromBody] TableSortingWithFilterRequest request)
        {
            request.Filter.BegDate = request.Filter.BegDate.ToLocalTime();
            request.Filter.EndDate = request.Filter.EndDate.ToLocalTime();
            return await _applicationsService.Get(request);
        }

        [HttpPost(nameof(GetListWithoutFilter))]
        [Produces("application/json")]
        public async Task<ApplicationsListResponse> GetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return await _applicationsService.Get(request);
        }

        [HttpPost(nameof(ImportXLSXGetListWithoutFilter))]
        public async Task<ActionResult> ImportXLSXGetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return File(await _applicationsService.ImportXLSXGetListWithoutFilter(request), "application/ms-excel");
        }

        [HttpPost(nameof(ImportCSVGetListWithoutFilter))]
        public async Task<ActionResult> ImportCSVGetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return File(await _applicationsService.ImportCSVGetListWithoutFilter(request), "text/csv");
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<Applications> Get(int id)
        {
            return await _applicationsService.Get(id);
        }

        [HttpPost(nameof(Post))]
        [Produces("application/json")]
        public async Task<Applications> Post([FromBody] Applications applications)
        {
            return await _applicationsService.Post(applications);
        }

        [HttpPut("{id}")]
        public async Task PutState(int id, StateEnum stateEnum)
        {
            //if (id != applications.Id)
            //{
            //    throw new ApiBadRequest();
            //}

            await _applicationsService.PutState(id, stateEnum);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _applicationsService.Delete(id);
        }
    }
}
