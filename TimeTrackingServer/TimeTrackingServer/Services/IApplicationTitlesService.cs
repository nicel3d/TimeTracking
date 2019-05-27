using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTrackingServer.Models;
using TimeTrackingServer.Stores.Impl;

namespace TimeTrackingServer.Services
{
    public class ApplicationTitlesListVM : ListCountResponse
    {
        public List<ApplicationTitles> Data { get; set; }
    }


    public class ApplicationTitlesFilterRequest : TableSortingRequest
    {
        public int ApplicationId { get; set; }
    }

    public interface IApplicationTitlesService
    {
        Task<ApplicationTitles> Get(int id);
        Task<ApplicationTitles> Post(ApplicationTitles group);
        Task Put(int id, ApplicationTitles group);
        Task Delete(int id);
        Task<ApplicationTitlesListVM> Get(ApplicationTitlesFilterRequest request, bool withSkipTake = true);
        //Task<byte[]> ExportXLSXGetListWithoutFilter(ApplicationGroupFilterRequest request);
        //Task<byte[]> ExportCSVGetListWithoutFilter(ApplicationGroupFilterRequest request);
    }
}
