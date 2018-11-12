using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using NgSchoolsBusinessLayer.Enums.Common;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations.Common
{
    public class CacheService : ICacheService
    {
        #region Ctors and Members

        private const int expirationMinutes = 60;

        private readonly IMemoryCache memoryCache;
        private readonly ILoggerService loggerService;
        private readonly IServiceProvider serviceProvider;

        public CacheService(IMemoryCache memoryCache, ILoggerService loggerService, 
            IServiceProvider serviceProvider)
        {
            this.memoryCache = memoryCache;
            this.loggerService = loggerService;
            this.serviceProvider = serviceProvider;
        }

        private void RefreshCache<T>(object key, object value, EvictionReason reason, object state)
        {
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<T>> SetInCache<T>(T cachedObject)
        {
            try
            {
                var expirationTime = DateTime.Now.AddMinutes(expirationMinutes);
                var expirationToken = new CancellationChangeToken(
                    new CancellationTokenSource(TimeSpan.FromMinutes(expirationMinutes + .01))
                    .Token);

                var objectType = cachedObject.GetType();
                var attribute = objectType.GenericTypeArguments[0].GetCustomAttributes(typeof(Cached), true).FirstOrDefault() as Cached;

                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.High)
                    .SetAbsoluteExpiration(expirationTime)
                    .AddExpirationToken(expirationToken)
                    .RegisterPostEvictionCallback(callback: RefreshCache<T>, state: this);

                memoryCache.Set(attribute.Key, cachedObject);
                return await ActionResponse<T>.ReturnSuccess();
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, cachedObject);
                return await ActionResponse<T>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<T>> RefreshCache<T>()
        {
            var refreshMethod = Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetMethods()
                .Where(m => m.GetCustomAttributes().OfType<CacheRefreshSource>()
                    .Any(a => a.type == typeof(T).GenericTypeArguments[0])
                ).ToList())
                .FirstOrDefault();

            var callingClassTypeInterfaces = refreshMethod.DeclaringType.GetInterfaces();

            var caller = serviceProvider.GetService(callingClassTypeInterfaces.First());
            var result = (ActionResponse<T>)await (dynamic)refreshMethod.Invoke(caller, null);
            return result;
        }

        /// <summary>
        /// Should be used with lists: await cacheService.GetFromCache<List<UserDto>>();
        /// </summary>
        /// <typeparam name="T">List of T</typeparam>
        /// <returns>List of T</returns>
        public async Task<ActionResponse<T>> GetFromCache<T>()
        {
            try
            {
                var attribute = typeof(T).GenericTypeArguments[0]
                    .GetCustomAttributes(typeof(Cached), true).FirstOrDefault() as Cached;
                if (memoryCache.TryGetValue<T>(attribute.Key, out T cachedValue))
                {
                    return await ActionResponse<T>.ReturnSuccess(cachedValue);
                }
                var result = await RefreshCache<T>();
                if (result.ActionResponseType != ActionResponseTypeEnum.Success)
                {
                    return await ActionResponse<T>.ReturnError("Unable to refresh cache!");
                }
                await SetInCache<T>(result.Data);

                return await GetFromCache<T>();
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<T>.ReturnError("Some sort of fuckup. Try again.");
            }
        }
    }
}
