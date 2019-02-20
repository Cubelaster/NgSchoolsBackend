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
    public class ClassTypeService : IClassTypeService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;

        public ClassTypeService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<ClassTypeDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<ClassType>().FindBy(c => c.Id == id);
                return await ActionResponse<ClassTypeDto>
                    .ReturnSuccess(mapper.Map<ClassType, ClassTypeDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassTypeDto>.ReturnError("Greška prilikom dohvata vrste nastave.");
            }
        }

        [CacheRefreshSource(typeof(ClassTypeDto))]
        public async Task<ActionResponse<List<ClassTypeDto>>> GetAllForCache()
        {
            try
            {
                var allClassTypes = unitOfWork.GetGenericRepository<ClassType>().GetAll();
                return await ActionResponse<List<ClassTypeDto>>.ReturnSuccess(
                    mapper.Map<List<ClassType>, List<ClassTypeDto>>(allClassTypes));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<ClassTypeDto>>.ReturnError("Greška prilikom dohvata vrsta nastave za brzu memoriju.");
            }
        }

        public async Task<ActionResponse<List<ClassTypeDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<ClassType>().GetAll();
                return await ActionResponse<List<ClassTypeDto>>
                    .ReturnSuccess(mapper.Map<List<ClassType>, List<ClassTypeDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<ClassTypeDto>>.ReturnError("Greška prilikom dohvata svih vrsta nastave.");
            }
        }

        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            try
            {
                return await ActionResponse<int>.ReturnSuccess(unitOfWork.GetGenericRepository<ClassType>().GetAllAsQueryable().Count());
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<int>.ReturnError("Greška prilikom dohvata broja vrsta nastave.");
            }
        }

        public async Task<ActionResponse<PagedResult<ClassTypeDto>>> GetAllClassTypesPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<ClassTypeDto> classTypes = new List<ClassTypeDto>();
                var cachedResponse = await cacheService.GetFromCache<List<ClassTypeDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out classTypes))
                {
                    classTypes = (await GetAll()).GetData();
                }

                var pagedResult = await classTypes.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<ClassTypeDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<ClassTypeDto>>.ReturnError("Greška prilikom dohvata straničnih podataka vrsta nastave.");
            }
        }

        public async Task<ActionResponse<ClassTypeDto>> Insert(ClassTypeDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<ClassTypeDto, ClassType>(entityDto);
                unitOfWork.GetGenericRepository<ClassType>().Add(entityToAdd);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<ClassTypeDto>>();
                return await ActionResponse<ClassTypeDto>
                    .ReturnSuccess(mapper.Map<ClassType, ClassTypeDto>(entityToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassTypeDto>.ReturnError("Greška prilikom upisa vrste nastave.");
            }
        }

        public async Task<ActionResponse<ClassTypeDto>> Update(ClassTypeDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<ClassTypeDto, ClassType>(entityDto);
                unitOfWork.GetGenericRepository<ClassType>().Update(entityToUpdate);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<ClassTypeDto>>();
                return await ActionResponse<ClassTypeDto>
                    .ReturnSuccess(mapper.Map<ClassType, ClassTypeDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassTypeDto>.ReturnError("Greška prilikom ažuriranja vrste nastave.");
            }
        }

        public async Task<ActionResponse<ClassTypeDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<ClassType>().Delete(id);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<ClassTypeDto>>();
                return await ActionResponse<ClassTypeDto>.ReturnSuccess(null, "Brisanje vrste nastave uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ClassTypeDto>.ReturnError("Greška prilikom brisanja vrste nastave.");
            }
        }
    }
}
