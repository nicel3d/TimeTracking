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
using System.Collections.Generic;

namespace TimeTrackingServer.Services.Impl
{
    public class StaffService : IStaffService
    {
        ApplicationDbContext _dbContext;
        public string[] comlumHeadrs = new string[]
        {
                "Обновлено",
                "Пользователь",
                "Последнее подключение",
                "Продолжительность последнего сеанса",
                "Последнее отключение"
        };

        public StaffService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            _dbContext.Remove(Get(id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Staff> Get(int id)
        {
            return await _dbContext.Staff.FindAsync(id);
        }

        public async Task<StaffListResponse> Get(TableSortingRequest request, bool withSkipTake = true)
        {
            IQueryable<Staff> data = _dbContext.Set<Staff>();

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.AsQueryable().Where(x =>
                            x.Caption.Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            return new StaffListResponse
            {
                Data = await data.Sort(request.Sorting)
                                .AddSkipTake(request)
                                .ToListAsync(),
                Total = await data.CountAsync()
            };
        }

        public async Task<List<Staff>> GetListByGropupId(TableSortingByGroupIdRequest request)
        {
            return await _dbContext.StaffToGroup
                                .Where(x => x.GroupId == request.GroupId)
                                .Include(x => x.Staff)
                                .Select(x => x.Staff)
                                .ToListAsync();
        }

        public async Task<byte[]> ImportCSVGetListWithoutFilter(TableSortingRequest request)
        {
            var staff = await Get(request, false);
            var csvStrung = new StringBuilder();
            staff.Data.ForEach(line =>
            {
                csvStrung.AppendLine(string.Join(",", new string[] {
                    line.UpdatedAt.ToString("g"),
                    line.Caption,
                    line.ActivityFirst?.ToString(),
                    line.ActivityFirst != null && line.ActivityLast != null ?
                        (line.ActivityFirst - line.ActivityLast).ToString() : "0:00",
                    line.ActivityLast?.ToString("g")
                }));
            });
            
            return Encoding.UTF8.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{csvStrung.ToString()}");
        }

        public async Task<byte[]> ImportXLSXGetListWithoutFilter(TableSortingRequest request)
        {
            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Пользователи");
                using (var cells = worksheet.Cells[1, 1, 1, 3])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }

                var j = 2;
                var data = await Get(request, false);
                foreach (var staff in data.Data)
                {
                    worksheet.Cells["A" + j].Value = staff.UpdatedAt.ToString("g");
                    worksheet.Cells["B" + j].Value = staff.Caption;
                    worksheet.Cells["C" + j].Value = staff.ActivityFirst?.ToString("g");
                    worksheet.Cells["D" + j].Value = staff.ActivityFirst != null && staff.ActivityLast != null ?
                        (staff.ActivityFirst - staff.ActivityLast).ToString() : "0:00";
                    worksheet.Cells["F" + j].Value = staff.ActivityLast?.ToString("g");
                    j++;
                }

                worksheet.Cells["A1:K20"].AutoFitColumns();

                result = package.GetAsByteArray();
            }

            return result;
        }

        public async Task<Staff> Post(Staff staff)
        {
            _dbContext.Staff.Update(staff);
            await _dbContext.SaveChangesAsync();
            return staff;
        }

        public async Task Put(int id, Staff staff)
        {
            var oldActivityStaff = await _dbContext.ActivityStaff.FindAsync(id);
            if (oldActivityStaff != null)
            {
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
