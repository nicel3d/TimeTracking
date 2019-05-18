﻿using System.Collections.Generic;
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

        [HttpPost(nameof(GetListWithRange))]
        [Produces("application/json")]
        public async Task<ApplicationsRangeListResponse> GetListWithRange([FromBody] TableSortingWithFilterRequest request)
        {
            request.Filter.BegDate = request.Filter.BegDate.ToLocalTime();
            request.Filter.EndDate = request.Filter.EndDate.ToLocalTime();
            return await _applicationsService.Get(request);
        }

        [HttpPost(nameof(GetList))]
        [Produces("application/json")]
        public async Task<ApplicationsListResponse> GetList([FromBody] TableSortingRequest request)
        {
            return await _applicationsService.Get(request);
        }

        [HttpPost(nameof(GetListFull))]
        [Produces("application/json")]
        public async Task<List<Applications>> GetListFull([FromBody] TableSortingByGroupIdRequest request)
        {
            return (await _applicationsService.Get(request, false)).Data;
        }

        [HttpPost(nameof(ExportXLSXGetListWithoutFilter))]
        public async Task<ActionResult> ExportXLSXGetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return File(await _applicationsService.ExportXLSXGetListWithoutFilter(request), "application/ms-excel");
        }

        [HttpPost(nameof(ExportCSVGetListWithoutFilter))]
        public async Task<ActionResult> ExportCSVGetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return File(await _applicationsService.ExportCSVGetListWithoutFilter(request), "text/csv");
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
