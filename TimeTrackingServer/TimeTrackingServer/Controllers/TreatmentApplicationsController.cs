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
    public class TreatmentApplicationsController : Controller
    {
        private ITreatmentApplicationsService _treatmentApplicationsService;

        public TreatmentApplicationsController(ITreatmentApplicationsService treatmentApplicationsService)
        {
            _treatmentApplicationsService = treatmentApplicationsService;
        }

        [HttpPost(nameof(GetList))]
        [Produces("application/json")]
        public async Task<VMApplicationGroupListResponse> GetList([FromBody] ApplicationGroupFilterRequest request)
        {
            return await _treatmentApplicationsService.Get(request);
        }

        [HttpPost(nameof(ExportXLSXGetListWithoutFilter))]
        public async Task<ActionResult> ExportXLSXGetListWithoutFilter([FromBody] ApplicationGroupFilterRequest request)
        {
            return File(await _treatmentApplicationsService.ExportXLSXGetListWithoutFilter(request), "application/ms-excel");
        }

        [HttpPost(nameof(ExportCSVGetListWithoutFilter))]
        public async Task<ActionResult> ExportCSVGetListWithoutFilter([FromBody] ApplicationGroupFilterRequest request)
        {
            return File(await _treatmentApplicationsService.ExportCSVGetListWithoutFilter(request), "text/csv");
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ApplicationToGroup> Get(int id)
        {
            return await _treatmentApplicationsService.Get(id);
        }

        [HttpPost(nameof(Post))]
        [Produces("application/json")]
        public async Task<ApplicationToGroup> Post([FromBody] ApplicationToGroup request)
        {
            return await _treatmentApplicationsService.Post(request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ApplicationToGroup request)
        {
            if (id != request.Id)
            {
                throw new ApiDontValidIdRequest();
            }
            await _treatmentApplicationsService.Put(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _treatmentApplicationsService.Delete(id);
        }
    }
}
