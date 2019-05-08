using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TimeTrackingServer.Constants;
using TimeTrackingServer.Data;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;
using TimeTrackingServer.Exceptions;
using OfficeOpenXml;
using System.Text;

namespace TimeTrackingServer.Services.Impl
{
    public class ApplicationsService : IApplicationsService
    {
        private ApplicationDbContext _dbContext;
        public string[] comlumHeadrs = new string[]
        {
                "Обновлено",
                "Название приложения",
                "Состояние"
        };


        public ApplicationsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            _dbContext.Remove(Get(id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Applications> Get(int id)
        {
            return await _dbContext.Applications.FindAsync(id);
        }

        public async Task<ApplicationsRangeListResponse> Get(TableSortingWithFilterRequest request)
        {
            IQueryable<ApplicationsRange> data = _dbContext.Set<Applications>()
                                .GroupJoin(_dbContext.ActivityStaff, app => app.Id,
                                            f => f.ApplicationId, (app, f) => new
                                            {
                                                app,
                                                act = f
                                                .Where(x => x.UpdatedAt > request.Filter.BegDate && x.UpdatedAt < request.Filter.EndDate)
                                                .Where(x => x.UpdatedAt.Hour > request.Filter.BegHour && x.UpdatedAt.Hour < request.Filter.EndHour)
                                                .GroupBy(x => x.StaffId)
                                            }
                                )
                                .Select(x => new ApplicationsRange
                                {
                                    Id = x.app.Id,
                                    Caption = x.app.Caption,
                                    State = x.app.State,
                                    UpdatedAt = x.app.UpdatedAt,
                                    UserUsed = x.act.Count()
                                })
                                .Where(x => x.UserUsed > 0);

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.AsQueryable().Where(x =>
                            x.Caption.Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            return new ApplicationsRangeListResponse
            {
                Data = await data.Sort(request.Sorting).AddSkipTake(request).ToListAsync(),
                Total = await data.CountAsync()
            };
        }

        public async Task<ApplicationsListResponse> Get(TableSortingRequest request, bool withSkipTake = true)
        {
            IQueryable<Applications> data = _dbContext.Applications;

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.AsQueryable().Where(x =>
                            x.Caption.Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            return new ApplicationsListResponse
            {
                Data = await (withSkipTake == true ? data.AddSkipTake(request) : data).ToListAsync(),
                Total = await data.CountAsync()
            };
        }

        public async Task<byte[]> ImportXLSXGetListWithoutFilter(TableSortingRequest request)
        {
            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Приложения");
                using (var cells = worksheet.Cells[1, 1, 1, 3])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }

                var j = 2;
                var applications = await Get(request, false);
                foreach (var application in applications.Data)
                {
                    worksheet.Cells["A" + j].Value = application.UpdatedAt.ToString("dd.MM./yyyy");
                    worksheet.Cells["B" + j].Value = application.Caption;
                    worksheet.Cells["C" + j].Value = application.State;
                    j++;
                }

                worksheet.Cells["A1:K20"].AutoFitColumns();

                result = package.GetAsByteArray();
            }

            return result;
        }
        public async Task<byte[]> ImportCSVGetListWithoutFilter(TableSortingRequest request)
        {
            var applications = await Get(request, false);
            var csvStrung = new StringBuilder();
            applications.Data.ForEach(line =>
            {
                csvStrung.AppendLine(string.Join(",", new string[] {
                    line.UpdatedAt.ToString("dd.MM./yyyy"),
                    line.Caption,
                    line.State.ToString()
                }));
            });

            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.UTF8.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{csvStrung.ToString()}");
        }

        public async Task<Applications> Post(Applications applications)
        {
            _dbContext.Applications.Add(applications);
            await _dbContext.SaveChangesAsync();
            return applications;
        }

        public async Task PutState(int id, StateEnum stateEnum)
        {
            var applications = await _dbContext.Applications.FindAsync(id);
            _dbContext.Applications.Attach(applications);
            applications.State = stateEnum;
            await _dbContext.SaveChangesAsync();
        }
    }
}
