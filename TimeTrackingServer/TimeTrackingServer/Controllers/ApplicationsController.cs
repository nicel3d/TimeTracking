using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TimeTrackingServer.Constants;
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
    public class ApplicationsController : Controller
    {
        private IApplicationsService _applicationsService;
        private IHostingEnvironment _hostingEnvironment;

        public ApplicationsController(IApplicationsService applicationsService, IHostingEnvironment hostingEnvironment)
        {
            _applicationsService = applicationsService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost(nameof(GetRangeList))]
        [Produces("application/json")]
        public async Task<ApplicationsRangeListResponse> GetRangeList([FromBody] TableSortingWithFilterRequest request)
        {
            return await _applicationsService.Get(request);
        }

        [HttpPost(nameof(GetListWithoutFilter))]
        [Produces("application/json")]
        public async Task<ApplicationsListResponse> GetListWithoutFilter([FromBody] TableSortingRequest request)
        {
            return await _applicationsService.Get(request);
        }

        [HttpPost(nameof(ImportGetListWithoutFilter))]
        public async Task<IActionResult> ImportGetListWithoutFilter([FromBody] TableSortingRequest value)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"demo.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Demo");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("ID");
                row.CreateCell(1).SetCellValue("Name");
                row.CreateCell(2).SetCellValue("Age");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue(1);
                row.CreateCell(1).SetCellValue("Kane Williamson");
                row.CreateCell(2).SetCellValue(29);

                row = excelSheet.CreateRow(2);
                row.CreateCell(0).SetCellValue(2);
                row.CreateCell(1).SetCellValue("Martin Guptil");
                row.CreateCell(2).SetCellValue(33);

                row = excelSheet.CreateRow(3);
                row.CreateCell(0).SetCellValue(3);
                row.CreateCell(1).SetCellValue("Colin Munro");
                row.CreateCell(2).SetCellValue(23);

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
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
