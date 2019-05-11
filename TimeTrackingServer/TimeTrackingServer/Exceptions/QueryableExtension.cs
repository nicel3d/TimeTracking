using System;
using System.Linq;
using TimeTrackingServer.Stores;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TimeTrackingServer.Data;

namespace TimeTrackingServer.Exceptions
{
    public interface ITWithUpdateAt
    {
        DateTime UpdatedAt { get; set; }
    }
    public interface IIdentifier
    {
        int? Id { get; set; }
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
            return source.Where(x => x.UpdatedAt.Date >= request.BegDate.Date && x.UpdatedAt.Date <= request.EndDate.Date)
                         .Where(x => x.UpdatedAt.Hour >= request.BegHour && x.UpdatedAt.Hour <= request.EndHour);
        }


        public static void DetachLocal<T>(this ApplicationDbContext context, T t, int entryId) where T : class, IIdentifier
        {
            var local = context.Set<T>().Local
                .FirstOrDefault(entry => entry.Id.Equals(entryId));

            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }

            context.Entry(t).State = EntityState.Modified;
        }
    }
}
