using System;
using System.Linq;
using TimeTrackingServer.Stores;
using System.Linq.Dynamic;

namespace TimeTrackingServer.Exceptions
{
    public static class QueryableExtension
    {
        public static IQueryable<T> AddSkipTake<T>(this IQueryable<T> source, ISkipTakeRequest request)
        {
            return source.Skip(request.Skip ?? 0).Take(request.Take ?? Int32.MaxValue);
        }
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, ISortingRequest request)
        {
            if (request.SortBy != null && request.Descending != null)
            {
                var order = request.Descending != true ? "DESC" : "ASC";
                return source.AsQueryable().OrderBy($"{request.SortBy} {order}");
            }

            return source;
        }
    }
}
