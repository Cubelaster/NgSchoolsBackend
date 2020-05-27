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
        public static async Task<PagedResult<T>> GetPaged<T>(this IQueryable<T> query, BasePagedRequest pagedRequest, bool onlyQueryable = false) where T : class
        {
            Type objectType = query.FirstOrDefault()?.GetType();

            if (objectType != null)
            {
                var currentPage = pagedRequest.PageIndex + 1;
                var result = new PagedResult<T>
                {
                    CurrentPage = currentPage,
                    PageSize = pagedRequest.PageSize
                };

                var totalCount = query.Count();

                int skip = (currentPage - 1) * pagedRequest.PageSize;

                if (!string.IsNullOrEmpty(pagedRequest.OrderBy) && totalCount > 1)
                {
                    if (pagedRequest.SortDirection == SortDirectionEnum.ASC)
                    {
                        query = query.OrderBy(q => q.GetPropValue(pagedRequest.OrderBy));
                    }
                    else
                    {
                        query = query.OrderByDescending(q => q.GetPropValue(pagedRequest.OrderBy));
                    }
                }

                query = query.AsNoTracking();

                List<PropertyInfo> searchableProperties;
                if (!string.IsNullOrEmpty(pagedRequest.SearchQuery))
                {
                    searchableProperties = objectType.GetSearchableProperties();

                    List<string> searchablePropertyNames = new List<string>();

                    if (searchableProperties.Any())
                    {
                        searchableProperties.ForEach(sp =>
                        {
                            if (sp.DeclaringType.Name != objectType.Name)
                            {
                                searchablePropertyNames.AddRange(objectType.GetPropertiesByTypeNameForValueFetch(sp));
                            }
                            else
                            {
                                searchablePropertyNames.Add(sp.Name);
                            }
                        });

                        query = query
                        .Where(q => searchablePropertyNames
                            .Any(p => (q.GetPropValue(p) != null ? q.GetPropValue(p).ToString() : "")
                            .Contains(pagedRequest.SearchQuery, StringComparison.OrdinalIgnoreCase)));
                    }
                }

                result.RowCount = query.Count();
                result.PageCount = (int)Math.Ceiling((double)result.RowCount / pagedRequest.PageSize);

                result.ResultQuery = query
                    .Skip(skip)
                    .Take(pagedRequest.PageSize);

                if (onlyQueryable)
                {
                    return result;
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
                Results = new List<T>(),
                ResultQuery = query
            };
        }

        public static async Task<PagedResult<T>> GetBySearchQuery<T>(this IQueryable<T> query, BasePagedRequest pagedRequest) where T : class
        {
            Type objectType = query.FirstOrDefault()?.GetType();
            if (objectType != null)
            {
                List<PropertyInfo> searchableProperties;
                if (!string.IsNullOrEmpty(pagedRequest.SearchQuery))
                {
                    searchableProperties = objectType
                        .GetProperties()
                        .Where(p => p.GetCustomAttributes().OfType<Searchable>().Any())
                        .ToList();

                    if (searchableProperties.Any())
                    {
                        query = query
                        .Where(q => searchableProperties
                            .Any(p => p.GetValue(q).ToString()
                            .Contains(pagedRequest.SearchQuery, StringComparison.OrdinalIgnoreCase)));
                    }
                }

                var result = new PagedResult<T>
                {
                    RowCount = query.Count()
                };

                result.Results = await Task.FromResult(query.ToList());
                return result;
            }

            return new PagedResult<T>
            {
                Results = new List<T>()
            };
        }
    }
}
