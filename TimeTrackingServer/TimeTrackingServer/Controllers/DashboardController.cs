using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Services;

namespace TimeTrackingServer.Controllers
{
    [Authorize]
    [ApiController]
    [EnableCors("cors")]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private IActivityStaffService _activiryStaffService;

        public DashboardController(IActivityStaffService activiryStaffService)
        {
            _activiryStaffService = activiryStaffService;
        }

        [HttpPost(nameof(GetStatisticByDate))]
        [Produces("application/json")]
        public async Task<ActivityStatisticResponse> GetStatisticByDate([FromBody] DateTime request)
        {
            request = request.ToLocalTime();
            return await _activiryStaffService.GetStatisticByDate(request);
        }

        [HttpPost(nameof(GetActivityStaffByDate))]
        [Produces("application/json")]
        public async Task<List<ActivityStaffResponse>> GetActivityStaffByDate([FromBody] DateTime request)
        {
            request = request.ToLocalTime();
            return await _activiryStaffService.GetActivityStaffByDate(request);
        }
    }
}
