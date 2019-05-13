﻿using Microsoft.EntityFrameworkCore;
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
        private string _worksheetTitle = "Пользователи";
        private string[] _comlumHeadrs = new string[]
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

        public async Task<StaffListResponse> Get(TableSortingByGroupIdRequest request, bool withSkipTake = true)
        {
            IQueryable<Staff> data = _dbContext.Set<Staff>();

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.Where(x =>
                            x.Caption.Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            if (request.GroupId != null)
            {
                data = data.Include(x => x.StaffToGroup)
                         .Select(x => new
                         {
                             p = x,
                             r = x.StaffToGroup.Where(f => f.GroupId == request.GroupId)
                         })
                         .Where(x => x.r.Count() > 0)
                         .Select(x => x.p)
                         .Select(x => new Staff()
                         {
                             Id = x.Id,
                             ActivityFirst = x.ActivityFirst,
                             ActivityLast = x.ActivityLast,
                             StaffToGroup = null,
                             Caption = x.Caption,
                             UpdatedAt = x.UpdatedAt,
                             Status = x.Status
                         });
            }

            return new StaffListResponse
            {
                Data = await (withSkipTake ? data.AddSkipTake(request) : data).Sort(request.Sorting).ToListAsync(),
                Total = await data.CountAsync()
            };
        }

        public async Task<List<Staff>> GetListOnlyByGropupId(int groupId)
        {
            return await _dbContext.StaffToGroup
                                .Where(x => x.GroupId == groupId)
                                .Include(x => x.Staff)
                                .Select(x => x.Staff)
                                .ToListAsync();
        }

        public async Task<byte[]> ImportCSVGetListWithoutFilter(TableSortingByGroupIdRequest request)
        {
            var staff = await Get(request, false);
            var csvStrung = new StringBuilder();
            staff.Data.ForEach(line =>
            {
                csvStrung.AppendLine(string.Join(",", new string[] {
                    line.UpdatedAt.GetValueOrDefault().ToString("g"),
                    line.Caption,
                    line.ActivityFirst?.ToString(),
                    line.ActivityFirst != null && line.ActivityLast != null ?
                        (line.ActivityFirst - line.ActivityLast).ToString() : "0:00",
                    line.ActivityLast?.ToString("g")
                }));
            });

            return Encoding.UTF8.GetBytes($"{string.Join(",", _comlumHeadrs)}\r\n{csvStrung.ToString()}");
        }

        public async Task<byte[]> ImportXLSXGetListWithoutFilter(TableSortingByGroupIdRequest request)
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
                foreach (var staff in data.Data)
                {
                    worksheet.Cells["A" + j].Value = staff.UpdatedAt.GetValueOrDefault().ToString("g");
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

        public async Task<Staff> GetOrAddStaffByAlias(string staffAlias)
        {
            Staff staff = await _dbContext.Staff
                .Where(x => x.Caption == staffAlias)
                .FirstOrDefaultAsync();

            if (staff == null)
            {
                staff = new Staff()
                {
                    Caption = staffAlias,
                    Status = true
                };
                _dbContext.Staff.Add(staff);
                await _dbContext.SaveChangesAsync();
            }

            return staff;
        }

        public async Task<Staff> Post(Staff staff)
        {
            _dbContext.Staff.Add(staff);
            await _dbContext.SaveChangesAsync();
            return staff;
        }
    }
}
