namespace TimeTrackingServer.Stores
{
    public interface IListCountResponse
    {
        int Count { get; set; }
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
