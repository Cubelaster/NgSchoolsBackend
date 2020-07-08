﻿using ExpressionPredicateBuilder;
using Microsoft.EntityFrameworkCore;
using NgSchoolsBusinessLayer.Enums.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using NgSchoolsDataLayer.Models.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

                if (pagedRequest.Where != null)
                {
                    var where = pagedRequest.Where.ToObject<T>();
                    query = query.FilterObject(where);
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

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, BasePagedRequest pagedRequest)
        {
            var currentPage = pagedRequest.PageIndex;
            int skip = (currentPage - 1) * pagedRequest.PageSize;
            return query.Skip(skip).Take(pagedRequest.PageSize);
        }

        private static Dictionary<string, PropertyInfo> GetSearchablePropertyNames(List<PropertyInfo> searchableProperties)
        {
            Type objectType = searchableProperties.First().DeclaringType;
            Dictionary<string, PropertyInfo> searchablePropertyNames = new Dictionary<string, PropertyInfo>();

            if (searchableProperties.Any())
            {
                searchableProperties.ForEach(sp =>
                {
                    if (sp.DeclaringType.Name != objectType.Name
                        && !sp.DeclaringType.Name.Contains(nameof(DatabaseEntity)))
                    {
                        //Ignoring for now because implementing custom Filters is easier
                        //objectType.GetPropKeyNameValue(sp)
                        //    .ToList()
                        //    .ForEach(x => searchablePropertyNames.Add(x.Key, x.Value));
                    }
                    else
                    {
                        searchablePropertyNames.Add(sp.Name, sp);
                    }
                });
            }

            return searchablePropertyNames;
        }

        private static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source,
            string propertyName, string methodName)
        {
            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == methodName && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TSource>)result;
        }

        private static IQueryable<T> SortObject<T>(Dictionary<string, SortDirectionEnum> sortPairs, IQueryable<T> query) where T : class
        {
            var sorts = sortPairs.ToList();

            if (sorts.Count > 0)
            {
                var firstSort = sorts.First();

                IOrderedQueryable<T> orderedQuery;

                var methodName = firstSort.Value.GetDescription();
                orderedQuery = query.OrderBy(firstSort.Key, methodName);

                foreach (var sort in sorts.Skip(1))
                {
                    methodName = $"ThenBy{(sort.Value.GetDescription() == SortDirectionEnum.ASC.GetDescription() ? "" : "Descending")}";
                    orderedQuery = query.OrderBy(firstSort.Key, methodName);
                }

                return orderedQuery;
            }

            return query;
        }

        public static IQueryable<T> FilterObject<T>(this IQueryable<T> query, T filterObject) where T : class
        {
            var filterValues = filterObject.GetObjectPropertiesWithValue();

            filterValues.Keys.ToList().ForEach(fv =>
            {
                var predicate = ExpressionBuilder.BuildPredicate<T>(filterValues[fv], OperatorComparer.Equals, fv);
                query = query.Where(predicate);
            });

            return query;
        }
    }
}
