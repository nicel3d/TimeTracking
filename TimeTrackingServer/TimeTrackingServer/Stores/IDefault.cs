using System;

namespace TimeTrackingServer.Stores
{
    public interface IListTotalResponse
    {
        int Total { get; set; }
    }

    public interface ISkipTakeRequest
    {
        int? Skip { get; set; }
        int? Take { get; set; }
    }

    public interface ISearchRequest
    {
        string Search { get; set; }
    }

    public interface ISortingRequest
    {
        bool? Descending { get; set; }
        string SortBy { get; set; }
    }

    public interface IFilterRequest
    {
        DateTime BegDate { get; set; }
        DateTime EndDate { get; set; }
        int BegHour { get; set; }
        int EndHour { get; set; }
    }

    public interface ISortingSkipTakeRequest : ISkipTakeRequest, ISearchRequest
    {
    }
}
