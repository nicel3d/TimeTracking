using System;

namespace TimeTrackingServer.Stores.Impl
{
    public class ListCountResponse : IListTotalResponse
    {
        public int Total { get; set; }
    }


    public class SearchRequest : ISearchRequest
    {
        public string Search { get; set; }
    }

    public class SkipTakeRequest : ISkipTakeRequest
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }

    public class SortingRequest : ISortingRequest
    {
        public bool? Descending { get; set; }
        public string SortBy { get; set; }
    }

    public class FilterRequest : IFilterRequest
    {
        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BegHour { get; set; }
        public int EndHour { get; set; }
    }

    public class SortingSearchSkipTakeRequest : ISortingSearchSkipTakeRequest
    {
        public SortingRequest Sorting { get; set; }
        public FilterRequest Filter { get; set; }
        public string Search { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
