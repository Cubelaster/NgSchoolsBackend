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
    public class EducationProgramService : IEducationProgramService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPlanService planService;
        private readonly ISubjectService subjectService;
        private readonly IThemeService themeService;
        private readonly ICacheService cacheService;

        public EducationProgramService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork, IPlanService planService, ISubjectService subjectService,
            IThemeService themeService, ICacheService cacheService)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.planService = planService;
            this.subjectService = subjectService;
            this.themeService = themeService;
            this.cacheService = cacheService;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<EducationProgramDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<EducationProgram>()
                    .FindBy(c => c.Id == id, includeProperties: "Subjects.Themes");
                return await ActionResponse<EducationProgramDto>
                    .ReturnSuccess(mapper.Map<EducationProgram, EducationProgramDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom dohvaćanja programa.");
            }
        }

        public async Task<ActionResponse<List<EducationProgramDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<EducationProgram>()
                    .GetAll(includeProperties: "Subjects.Themes");
                return await ActionResponse<List<EducationProgramDto>>
                    .ReturnSuccess(mapper.Map<List<EducationProgram>, List<EducationProgramDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<EducationProgramDto>>.ReturnError("Greška prilikom dohvaćanja svih programa.");
            }
        }

        [CacheRefreshSource(typeof(EducationProgramDto))]
        public async Task<ActionResponse<List<EducationProgramDto>>> GetAllForCache()
        {
            try
            {
                var allEntities = unitOfWork.GetGenericRepository<EducationProgram>()
                    .GetAll(includeProperties: "Subjects.Themes");
                return await ActionResponse<List<EducationProgramDto>>.ReturnSuccess(
                    mapper.Map<List<EducationProgram>, List<EducationProgramDto>>(allEntities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<EducationProgramDto>>.ReturnError("Greška prilikom dohvata svih edukacijskih programa.");
            }
        }

        public async Task<ActionResponse<PagedResult<EducationProgramDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<EducationProgramDto> eduPrograms = new List<EducationProgramDto>();
                var cachedResponse = await cacheService.GetFromCache<List<EducationProgramDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out eduPrograms))
                {
                    eduPrograms = (await GetAll()).GetData();
                }

                var pagedResult = await eduPrograms.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<EducationProgramDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<EducationProgramDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za programe.");
            }
        }

        public async Task<ActionResponse<PagedResult<EducationProgramDto>>> GetBySearchQuery(BasePagedRequest pagedRequest)
        {
            try
            {
                List<EducationProgramDto> eduPrograms = new List<EducationProgramDto>();
                var cachedResponse = await cacheService.GetFromCache<List<EducationProgramDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out eduPrograms))
                {
                    eduPrograms = (await GetAll()).GetData();
                }

                var pagedResult = await eduPrograms.AsQueryable().GetBySearchQuery(pagedRequest);
                return await ActionResponse<PagedResult<EducationProgramDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<EducationProgramDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za programe.");
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> Insert(EducationProgramDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<EducationProgramDto, EducationProgram>(entityDto);
                unitOfWork.GetGenericRepository<EducationProgram>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<EducationProgramDto>
                    .ReturnSuccess(mapper.Map(entityToAdd, entityDto));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom ažuriranja programa.");
            }
            finally
            {
                await cacheService.RefreshCache<List<EducationProgramDto>>();
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> Update(EducationProgramDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<EducationProgramDto, EducationProgram>(entityDto);
                unitOfWork.GetGenericRepository<EducationProgram>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<EducationProgramDto>
                    .ReturnSuccess(mapper.Map(entityToUpdate, entityDto));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom ažuriranja programa.");
            }
            finally
            {
                await cacheService.RefreshCache<List<EducationProgramDto>>();
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> Delete(int id)
        {
            try
            {
                var planEntity = unitOfWork.GetGenericRepository<EducationProgram>().FindBy(p => p.Id == id);
                unitOfWork.GetGenericRepository<EducationProgram>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<EducationProgramDto>.ReturnSuccess(null, "Brisanje programa i povezanog plana uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<EducationProgramDto>.ReturnError("Some sort of fuckup!");
            }
            finally
            {
                await cacheService.RefreshCache<List<EducationProgramDto>>();
            }
        }
    }
}
