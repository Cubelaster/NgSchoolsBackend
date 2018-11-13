using Microsoft.EntityFrameworkCore;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query, BasePagedRequest pagedRequest) where T : class
        {
            var currentPage = pagedRequest.PageIndex + 1;
            var result = new PagedResult<T>
            {
                CurrentPage = currentPage,
                PageSize = pagedRequest.PageSize,
                RowCount = query.Count()
            };

            result.PageCount = (int)Math.Ceiling((double)result.RowCount / pagedRequest.PageSize);

            int skip = (currentPage - 1) * pagedRequest.PageSize;

            result.Results = await Task.FromResult(query
                .AsNoTracking()
                .Skip(skip)
                .Take(pagedRequest.PageSize)
                .ToList()
            );

            return result;
        }
    }
}
