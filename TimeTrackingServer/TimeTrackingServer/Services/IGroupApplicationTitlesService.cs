using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public class GroupApplicationTitlesVM : ApplicationTitleToGroup
    {
        public string ApplicationTitle { get; set; }
        public string ApplicationCaption { get; set; }
    }

    public class GroupApplicationTitleListVM : ListCountResponse
    {
        public List<GroupApplicationTitlesVM> Data { get; set; }
    }

    public interface IGroupApplicationTitlesService
    {
        Task<ApplicationTitleToGroup> Get(int id);
        Task<ApplicationTitleToGroup> Post(ApplicationTitleToGroup group);
        Task Put(int id, ApplicationTitleToGroup group);
        Task Delete(int id);
        Task<GroupApplicationTitleListVM> Get(ApplicationGroupFilterRequest request, bool withSkipTake = true);
        //Task<byte[]> ExportXLSXGetListWithoutFilter(ApplicationGroupFilterRequest request);
        //Task<byte[]> ExportCSVGetListWithoutFilter(ApplicationGroupFilterRequest request);
    }
}
