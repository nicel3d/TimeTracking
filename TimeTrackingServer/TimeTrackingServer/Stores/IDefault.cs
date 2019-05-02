namespace TimeTrackingServer.Stores
{
    public interface IListTotalResponse
    {
        int Total { get; set; }
    }

    public interface IListTotalResponse
    {
        int Total { get; set; }
    }

    public interface ISkipTakeRequest
    {
        int? Skip { get; set; }
        int? Take { get; set; }
    }

    public interface ISortingRequest
    {
        bool? Descending { get; set; }
        string SortBy { get; set; }
    }

    public interface ISortingAndSkipTakeRequest : ISortingRequest, ISkipTakeRequest
    {
    }
}
