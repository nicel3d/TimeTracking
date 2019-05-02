﻿using System.Threading.Tasks;
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
    public class ActivityStaffController : Controller
    {
        private IActivityStaffService _activiryStaffService;

        public ActivityStaffController(IActivityStaffService activiryStaffService)
        {
            _activiryStaffService = activiryStaffService;
        }

        [HttpPost(nameof(GetList))]
        public async Task<ActivityStaffListResponse> GetList([FromBody] SortingAndSkipTakeRequest request)
        {
            return await _activiryStaffService.Get(request);
        }

        [HttpGet("{id}", Name = "Get")]
        [Produces("application/json")]
        public async Task<ActivityStaff> Get(int id)
        {
            return await _activiryStaffService.Get(id);
        }

        [HttpPost(nameof(Post))]
        [Produces("application/json")]
        public async Task<ActivityStaff> Post([FromBody] ActivityStaff activityStaff)
        {
            //activityStaff.Id = null;
            return await _activiryStaffService.Post(activityStaff);
        }

        [HttpPut("{id}", Name = "Put")]
        public async Task Put(int id, [FromBody]ActivityStaff activityStaff)
        {
            await _activiryStaffService.Put(id, activityStaff);
        }

        [HttpDelete("{id}", Name = "Delete")]
        public async Task Delete(int id)
        {
            await _activiryStaffService.Delete(id);
        }
    }
}
