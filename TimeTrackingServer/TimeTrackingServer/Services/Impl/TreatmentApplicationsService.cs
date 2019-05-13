using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Data;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;
using System.Linq.Dynamic;
using TimeTrackingServer.Exceptions;
using System.Text;
using OfficeOpenXml;

namespace TimeTrackingServer.Services.Impl
{
    public class TreatmentApplicationsService : ITreatmentApplicationsService
    {
        ApplicationDbContext _dbContext;
        private string _worksheetTitle = "Ограничения по программам";
        private string[] _comlumHeadrs = new string[]
        {
                "Обновлено",
                "Название приложения",
                "Состояние"
        };

        public TreatmentApplicationsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            var item = await Get(id);
            _dbContext.Remove(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApplicationToGroup> Get(int id)
        {
            return await _dbContext.ApplicationToGroup.FindAsync(id);
        }

        public async Task<VMApplicationGroupListResponse> Get(ApplicationGroupFilterRequest request, bool withSkipTake = true)
        {
            IQueryable<VMApplicationGroup> data = _dbContext.Set<ApplicationToGroup>()
                                                .Join(_dbContext.Applications, appGroup => appGroup.ApplicationId,
                                                        app => app.Id, (appGroup, app) => new
                                                        {
                                                            app,
                                                            appGroup
                                                        }
                                                    )
                                                .Where(x => x.appGroup.GroupId == request.GroupId)
                                                .Select(x => new VMApplicationGroup
                                                {
                                                    Id = x.appGroup.Id,
                                                    ApplicationId = x.appGroup.ApplicationId,
                                                    GroupId = x.appGroup.GroupId,
                                                    State = x.appGroup.State,
                                                    UpdatedAt = x.appGroup.UpdatedAt,
                                                    ApplicationTitle = x.app.Caption
                                                });

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.Where(x =>
                                x.ApplicationTitle.Contains(request.Search) ||
                                x.UpdatedAt.ToString().Contains(request.Search)
                            );
            }

            return new VMApplicationGroupListResponse
            {
                Data = await (withSkipTake ? data.AddSkipTake(request) : data).Sort(request.Sorting).ToListAsync(),
                Total = await data.CountAsync()
            };
        }
        public async Task<byte[]> ImportCSVGetListWithoutFilter(ApplicationGroupFilterRequest request)
        {
            var items = await Get(request, false);
            var csvStrung = new StringBuilder();
            items.Data.ForEach(line =>
            {
                csvStrung.AppendLine(string.Join(",", new string[] {
                    line.UpdatedAt.GetValueOrDefault().ToString("g"),
                    line.ApplicationTitle,
                    line.State.ToString()
                }));
            });

            return Encoding.UTF8.GetBytes($"{string.Join(",", _comlumHeadrs)}\r\n{csvStrung.ToString()}");
        }

        public async Task<byte[]> ImportXLSXGetListWithoutFilter(ApplicationGroupFilterRequest request)
        {
            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(_worksheetTitle);
                using (var cells = worksheet.Cells[1, 1, 1, 3])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < _comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = _comlumHeadrs[i];
                }

                var j = 2;
                var data = await Get(request, false);
                foreach (var item in data.Data)
                {
                    worksheet.Cells["A" + j].Value = item.UpdatedAt.GetValueOrDefault().ToString("g");
                    worksheet.Cells["B" + j].Value = item.ApplicationTitle;
                    worksheet.Cells["C" + j].Value = item.State.ToString();
                    j++;
                }

                worksheet.Cells["A1:K20"].AutoFitColumns();

                result = package.GetAsByteArray();
            }

            return result;
        }
        public async Task<ApplicationToGroup> Post(ApplicationToGroup applicationToGroup)
        {
            _dbContext.ApplicationToGroup.Add(applicationToGroup);
            await _dbContext.SaveChangesAsync();
            return applicationToGroup;
        }
        public async Task Put(int id, ApplicationToGroup group)
        {
            _dbContext.DetachLocal(group, id);
            await _dbContext.SaveChangesAsync();
        }
    }
}
