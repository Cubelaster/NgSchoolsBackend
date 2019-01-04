using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class ClassLocationsService : IClassLocationsService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;

        public ClassLocationsService(IMapper mapper, ILoggerService loggerService, 
            IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<ClassLocationsDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<ClassLocations>().FindBy(c => c.Id == id);
                return await ActionResponse<ClassLocationsDto>
                    .ReturnSuccess(mapper.Map<ClassLocations, ClassLocationsDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassLocationsDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<List<ClassLocationsDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<ClassLocations>().GetAll();
                return await ActionResponse<List<ClassLocationsDto>>
                    .ReturnSuccess(mapper.Map<List<ClassLocations>, List<ClassLocationsDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<ClassLocationsDto>>.ReturnError("Some sort of fuckup!");
            }
        }

        [CacheRefreshSource(typeof(ClassLocationsDto))]
        public async Task<ActionResponse<List<ClassLocationsDto>>> GetAllUsersForCache()
        {
            try
            {
                var allClassLocations = unitOfWork.GetGenericRepository<ClassLocations>().GetAll();
                return await ActionResponse<List<ClassLocationsDto>>.ReturnSuccess(
                    mapper.Map<List<ClassLocations>, List<ClassLocationsDto>>(allClassLocations));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<ClassLocationsDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<PagedResult<ClassLocationsDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<ClassLocationsDto> classTypes = new List<ClassLocationsDto>();
                var cachedResponse = await cacheService.GetFromCache<List<ClassLocationsDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out classTypes))
                {
                    classTypes = (await GetAll()).GetData();
                }

                var pagedResult = await classTypes.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<ClassLocationsDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<ClassLocationsDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<ClassLocationsDto>> Insert(ClassLocationsDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<ClassLocationsDto, ClassLocations>(entityDto);
                unitOfWork.GetGenericRepository<ClassLocations>().Add(entityToAdd);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<ClassLocationsDto>>();
                return await ActionResponse<ClassLocationsDto>
                    .ReturnSuccess(mapper.Map<ClassLocations, ClassLocationsDto>(entityToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassLocationsDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<ClassLocationsDto>> Update(ClassLocationsDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<ClassLocationsDto, ClassLocations>(entityDto);
                unitOfWork.GetGenericRepository<ClassLocations>().Update(entityToUpdate);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<ClassLocationsDto>>();
                return await ActionResponse<ClassLocationsDto>
                    .ReturnSuccess(mapper.Map<ClassLocations, ClassLocationsDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassLocationsDto>.ReturnError("Some sort of fuckup!");
            }
        }

        public async Task<ActionResponse<ClassLocationsDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<ClassLocations>().Delete(id);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<ClassLocationsDto>>();
                return await ActionResponse<ClassLocationsDto>.ReturnSuccess(null, "Delete successful.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassLocationsDto>.ReturnError("Some sort of fuckup!");
            }
        }
    }
}
