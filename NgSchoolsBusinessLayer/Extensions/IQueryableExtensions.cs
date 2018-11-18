using Microsoft.EntityFrameworkCore;
using NgSchoolsBusinessLayer.Enums.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query, BasePagedRequest pagedRequest) where T : class
        {
            Type objectType = query.FirstOrDefault()?.GetType();
            if (objectType != null)
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

                PropertyInfo propertyForSort = null;
                if (!string.IsNullOrEmpty(pagedRequest.OrderBy) && result.RowCount > 1)
                {
                    propertyForSort = objectType.GetProperty(pagedRequest.OrderBy);
                }

                query = query.AsNoTracking();

                if (propertyForSort != null)
                {
                    if (pagedRequest.SortDirection == SortDirectionEnum.ASC)
                    {
                        query = query.OrderBy(q => propertyForSort.GetValue(q));
                    }
                    else
                    {
                        query = query.OrderByDescending(q => propertyForSort.GetValue(q));
                    }
                }

                List<PropertyInfo> searchableProperties;
                if (!string.IsNullOrEmpty(pagedRequest.SearchQuery))
                {
                    searchableProperties = objectType
                        .GetProperties()
                        .Where(p => p.GetCustomAttributes().OfType<Searchable>().Any())
                        .ToList();

                    query = query
                        .Where(q => searchableProperties
                            .Any(p => p.GetValue(q).ToString()
                            .Contains(pagedRequest.SearchQuery, StringComparison.OrdinalIgnoreCase)));
                }

                result.Results = await Task.FromResult(
                    query
                    .Skip(skip)
                    .Take(pagedRequest.PageSize)
                    .ToList()
                );

                return result;
            }

            return new PagedResult<T>
            {
                Results = new List<T>()
            };
        }
    }
}
