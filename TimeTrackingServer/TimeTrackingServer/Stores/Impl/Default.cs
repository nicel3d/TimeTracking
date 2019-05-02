namespace TimeTrackingServer.Stores.Impl
{
    public class ListCountResponse : IListTotalResponse
    {
        public int Total { get; set; }
    }

    public class SkipTakeRequest : ISkipTakeRequest
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }

    public class DataTablePaginationRequest : ISortingRequest
    {
        public bool? Descending { get; set; }
        public int? Page { get; set; }
        public int? RowsPerPage { get; set; }
        public string SortBy { get; set; }
        public int? TotalItems { get; set; }
    }

    public class SortingAndSkipTakeRequest : ISortingAndSkipTakeRequest
    {
        public bool? Descending { get; set; }
        public int? Page { get; set; }
        public int? RowsPerPage { get; set; }
        public string SortBy { get; set; }
        public int? TotalItems { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
