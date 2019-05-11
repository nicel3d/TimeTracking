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
    public class GroupService : IGroupService
    {
        ApplicationDbContext _dbContext;
        public string[] comlumHeadrs = new string[]
        {
                "Обновлено",
                "Наименование группы",
                "Кол-во участников"
        };

        public GroupService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            _dbContext.Remove(Get(id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Groups> Get(int id)
        {
            return await _dbContext.Groups.FindAsync(id);
        }

        public async Task<GroupListResponse> Get(TableSortingRequest request, bool withSkipTake = true)
        {
            IQueryable<Groups> data = _dbContext.Groups;

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.AsQueryable().Where(x =>
                            x.Name.Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            return new GroupListResponse
            {
                Data = await (withSkipTake ? data.AddSkipTake(request) : data).Sort(request.Sorting).ToListAsync(),
                Total = await data.CountAsync()
            };
        }
        public async Task<GroupsListWithCountUsersResponse> GetListWithCountUsers(TableSortingRequest request, bool withSkipTake = true)
        {
            IQueryable<GroupsWithCountUsers> data = _dbContext.Set<Groups>()
                                            .GroupJoin(_dbContext.StaffToGroup, gr => gr.Id,
                                                stg => stg.GroupId, (gr, stg) => new
                                                {
                                                    gr,
                                                    stg = stg.GroupBy(x => x.StaffId)
                                                }
                                            )
                                            .Select(x => new GroupsWithCountUsers
                                            {
                                                Id = x.gr.Id,
                                                Name = x.gr.Name,
                                                UpdatedAt = x.gr.UpdatedAt,
                                                CountUsers = x.stg.Count()
                                            });

            if (!String.IsNullOrEmpty(request.Search))
            {
                data = data.AsQueryable().Where(x =>
                            x.Name.Contains(request.Search) ||
                            x.CountUsers.ToString().Contains(request.Search) ||
                            x.UpdatedAt.ToString().Contains(request.Search)
                        );
            }

            return new GroupsListWithCountUsersResponse
            {
                Data = await (withSkipTake ? data.AddSkipTake(request) : data).Sort(request.Sorting).ToListAsync(),
                Total = await data.CountAsync()
            };
        }
        public async Task<byte[]> ImportCSVGetListWithoutFilter(TableSortingRequest request)
        {
            var items = await GetListWithCountUsers(request, false);
            var csvStrung = new StringBuilder();
            items.Data.ForEach(line =>
            {
                csvStrung.AppendLine(string.Join(",", new string[] {
                    line.UpdatedAt.ToString("g"),
                    line.Name,
                    line.CountUsers.ToString()
                }));
            });

            return Encoding.UTF8.GetBytes($"{string.Join(",", comlumHeadrs)}\r\n{csvStrung.ToString()}");
        }

        public async Task<byte[]> ImportXLSXGetListWithoutFilter(TableSortingRequest request)
        {
            byte[] result;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Группы");
                using (var cells = worksheet.Cells[1, 1, 1, 3])
                {
                    cells.Style.Font.Bold = true;
                }

                for (var i = 0; i < comlumHeadrs.Count(); i++)
                {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }

                var j = 2;
                var data = await GetListWithCountUsers(request, false);
                foreach (var staff in data.Data)
                {
                    worksheet.Cells["A" + j].Value = staff.UpdatedAt.ToString("g");
                    worksheet.Cells["B" + j].Value = staff.Name;
                    worksheet.Cells["C" + j].Value = staff.CountUsers.ToString();
                    j++;
                }

                worksheet.Cells["A1:K20"].AutoFitColumns();

                result = package.GetAsByteArray();
            }

            return result;
        }

        public async Task<Groups> Post(Groups group)
        {
            _dbContext.Groups.Update(group);
            await _dbContext.SaveChangesAsync();
            return group;
        }

        public async Task Put(int id, Groups group)
        {
            var oldActivityStaff = await _dbContext.Groups.FindAsync(id);
            if (oldActivityStaff != null)
            {
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
