using Microsoft.Extensions.Caching.Memory;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations.Common
{
    public class CacheService : ICacheService
    {
        private IMemoryCache memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public async Task<ActionResponse<T>> SetInCache<T>(T cachedObject)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            var objectType = cachedObject.GetType();
            var attribute = objectType.GetCustomAttributes(typeof(Cached), true).FirstOrDefault() as Cached;
            memoryCache.Set(attribute.Key, cachedObject);
            return await ActionResponse<T>.ReturnSuccess();
        }

        public async Task<ActionResponse<T>> RefreshCache<T>(T cachedObject)
        {
            return await ActionResponse<T>.ReturnSuccess();
        }
    }
}
