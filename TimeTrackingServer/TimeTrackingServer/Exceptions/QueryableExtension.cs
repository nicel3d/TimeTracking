using System;
using System.Linq;
using TimeTrackingServer.Stores;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Collections.Generic;

namespace TimeTrackingServer.Exceptions
{
    public interface ITWithUpdateAt
    {
        DateTime UpdatedAt { get; set; }
    }

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
        public static IQueryable<T> WhereDateFilter<T>(this IQueryable<T> source, IFilterRequest request)
            where T : ITWithUpdateAt
        {
            return source.Where(x => x.UpdatedAt.Date.CompareTo(request.BegDate.Date) >= 0 && x.UpdatedAt.Date.CompareTo(request.EndDate.Date) <= 0)
                         .Where(x => x.UpdatedAt.Hour >= request.BegHour && x.UpdatedAt.Hour <= request.EndHour);
        }
        public static IEnumerable<T> WhereDateFilter2<T>(this IEnumerable<T> source, IFilterRequest request)
            where T : ITWithUpdateAt
        {
            return source.Where(x => x.UpdatedAt.Date.CompareTo(request.BegDate.Date) >= 0 && x.UpdatedAt.Date.CompareTo(request.EndDate.Date) <= 0)
                         .Where(x => x.UpdatedAt.Hour >= request.BegHour && x.UpdatedAt.Hour <= request.EndHour);
        }
    }
}
