﻿using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface ICacheService
    {
        Task<ActionResponse<T>> RefreshCache<T>();
        Task<ActionResponse<T>> SetInCache<T>(T cachedObject);
        Task<ActionResponse<T>> GetFromCache<T>();
    }
}