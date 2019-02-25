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
        private readonly IUnitOfWork unitOfWork;
        private readonly IPlanService planService;
        private readonly ISubjectService subjectService;
        private readonly IThemeService themeService;
        private readonly ICacheService cacheService;

        public EducationProgramService(IMapper mapper,
            IUnitOfWork unitOfWork, IPlanService planService, ISubjectService subjectService,
            IThemeService themeService, ICacheService cacheService)
        {
            this.mapper = mapper;
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
                    .FindBy(c => c.Id == id,
                    includeProperties: "EducationGroup,Subjects.Themes,EducationProgramClassTypes.ClassType,Plan");
                return await ActionResponse<EducationProgramDto>
                    .ReturnSuccess(mapper.Map<EducationProgram, EducationProgramDto>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom dohvaćanja programa.");
            }
        }

        public async Task<ActionResponse<List<EducationProgramDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<EducationProgram>()
                    .GetAll(includeProperties: "EducationGroup,Subjects.Themes,EducationProgramClassTypes.ClassType,Plan");
                return await ActionResponse<List<EducationProgramDto>>
                    .ReturnSuccess(mapper.Map<List<EducationProgram>, List<EducationProgramDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<EducationProgramDto>>.ReturnError("Greška prilikom dohvaćanja svih programa.");
            }
        }

        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            try
            {
                return await ActionResponse<int>.ReturnSuccess(unitOfWork.GetGenericRepository<EducationProgram>().GetAllAsQueryable().Count());
            }
            catch (Exception)
            {
                return await ActionResponse<int>.ReturnError("Greška prilikom dohvata broja programa.");
            }
        }

        [CacheRefreshSource(typeof(EducationProgramDto))]
        public async Task<ActionResponse<List<EducationProgramDto>>> GetAllForCache()
        {
            try
            {
                var allEntities = unitOfWork.GetGenericRepository<EducationProgram>()
                    .GetAll(includeProperties: "EducationGroup,Subjects.Themes,EducationProgramClassTypes.ClassType,Plan");
                return await ActionResponse<List<EducationProgramDto>>.ReturnSuccess(
                    mapper.Map<List<EducationProgram>, List<EducationProgramDto>>(allEntities));
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<PagedResult<EducationProgramDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za programe.");
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> Insert(EducationProgramDto entityDto)
        {
            try
            {
                List<ClassTypeDto> classTypes = entityDto.ClassTypes != null ?
                    new List<ClassTypeDto>(entityDto.ClassTypes) : new List<ClassTypeDto>();

                var entityToAdd = mapper.Map<EducationProgramDto, EducationProgram>(entityDto);
                unitOfWork.GetGenericRepository<EducationProgram>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);
                entityDto.ClassTypes = classTypes;

                if ((await ModifyClassTypes(entityDto))
                    .IsNotSuccess(out ActionResponse<EducationProgramDto> ctResponse, out entityDto))
                {
                    return ctResponse;
                }

                entityToAdd = unitOfWork.GetGenericRepository<EducationProgram>()
                    .FindBy(e => e.Id == entityToAdd.Id,
                    includeProperties: "EducationGroup,Subjects.Themes,EducationProgramClassTypes.ClassType");

                return await ActionResponse<EducationProgramDto>.ReturnSuccess(mapper.Map(entityToAdd, entityDto));
            }
            catch (Exception)
            {
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom upisa programa.");
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
                List<ClassTypeDto> classTypes = entityDto.ClassTypes != null ?
                    new List<ClassTypeDto>(entityDto.ClassTypes) : new List<ClassTypeDto>();

                var entityToUpdate = mapper.Map<EducationProgramDto, EducationProgram>(entityDto);
                unitOfWork.GetGenericRepository<EducationProgram>().Update(entityToUpdate);
                unitOfWork.Save();

                mapper.Map(entityToUpdate, entityDto);
                entityDto.ClassTypes = classTypes;

                if ((await ModifyClassTypes(entityDto))
                    .IsNotSuccess(out ActionResponse<EducationProgramDto> ctResponse, out entityDto))
                {
                    return ctResponse;
                }

                entityToUpdate = unitOfWork.GetGenericRepository<EducationProgram>()
                    .FindBy(e => e.Id == entityToUpdate.Id,
                    includeProperties: "EducationGroup,Subjects.Themes,EducationProgramClassTypes.ClassType");

                return await ActionResponse<EducationProgramDto>.ReturnSuccess(mapper.Map(entityToUpdate, entityDto));
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom brisanja programa.");
            }
            finally
            {
                await cacheService.RefreshCache<List<EducationProgramDto>>();
            }
        }

        public async Task<ActionResponse<EducationProgramDto>> ModifyClassTypes(EducationProgramDto entityDto)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<EducationProgram>()
                    .FindBy(e => e.Id == entityDto.Id, includeProperties: "EducationProgramClassTypes.ClassType");

                var currentClassTypes = mapper.Map<List<EducationProgramClassType>, List<EducationProgramClassTypeDto>>(
                    entity.EducationProgramClassTypes.ToList());

                var newClassTypes = entityDto.ClassTypes;

                var classTypesToRemove = currentClassTypes
                    .Where(cet => !newClassTypes.Select(f => f.Id).Contains(cet.ClassTypeId))
                    .ToList();

                var classTypesToAdd = newClassTypes
                    .Where(nt => !currentClassTypes.Select(uec => uec.ClassTypeId).Contains(nt.Id))
                    .Select(sf => new EducationProgramClassTypeDto
                    {
                        ClassTypeId = sf.Id,
                        EducationProgramId = entityDto.Id
                    }).ToList();

                if ((await RemoveClassTypes(classTypesToRemove))
                    .IsNotSuccess(out ActionResponse<List<EducationProgramClassTypeDto>> removeResponse))
                {
                    return await ActionResponse<EducationProgramDto>.ReturnError("Neuspješno micanje tipova nastave s edukacijskog programa.");
                }

                if ((await AddClassTypes(classTypesToAdd)).IsNotSuccess(out ActionResponse<List<EducationProgramClassTypeDto>> addResponse, out classTypesToAdd))
                {
                    return await ActionResponse<EducationProgramDto>.ReturnError("Neuspješno dodavanje tipova nastave edukacijskom programu.");
                }
                return await ActionResponse<EducationProgramDto>.ReturnSuccess(entityDto, "Uspješno izmijenjeni tipovi nastave edukacijskog programa.");
            }
            catch (Exception)
            {
                return await ActionResponse<EducationProgramDto>.ReturnError("Greška prilikom izmjene tipova nastave edukacijskog programa.");
            }
        }

        public async Task<ActionResponse<List<EducationProgramClassTypeDto>>> RemoveClassTypes(List<EducationProgramClassTypeDto> entities)
        {
            try
            {
                var response = await ActionResponse<List<EducationProgramClassTypeDto>>.ReturnSuccess(null, "Datoteke uspješno maknute sa studenta.");
                entities.ForEach(async ct =>
                {
                    if ((await RemoveClassType(ct))
                        .IsNotSuccess(out ActionResponse<EducationProgramClassTypeDto> actionResponse))
                    {
                        response = await ActionResponse<List<EducationProgramClassTypeDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<EducationProgramClassTypeDto>>.ReturnError("Greška prilikom micanja vrsta nastave s edukacijskog programa.");
            }
        }

        public async Task<ActionResponse<EducationProgramClassTypeDto>> RemoveClassType(EducationProgramClassTypeDto entity)
        {
            try
            {
                unitOfWork.GetGenericRepository<EducationProgramClassType>().Delete(entity.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<EducationProgramClassTypeDto>.ReturnSuccess(null, "Vrsta nastave uspješno izbrisana iz edukacijskog programa.");
            }
            catch (Exception)
            {
                return await ActionResponse<EducationProgramClassTypeDto>.ReturnError("Greška prilikom brisanja vrste nastave iz edukacijskog programa.");
            }
        }

        public async Task<ActionResponse<List<EducationProgramClassTypeDto>>> AddClassTypes(List<EducationProgramClassTypeDto> entities)
        {
            try
            {
                var response = await ActionResponse<List<EducationProgramClassTypeDto>>.ReturnSuccess(null, "Vrste nastave uspješno dodane edukacijskom progreamu.");
                entities.ForEach(async s =>
                {
                    if ((await AddClassType(s))
                        .IsNotSuccess(out ActionResponse<EducationProgramClassTypeDto> actionResponse, out s))
                    {
                        response = await ActionResponse<List<EducationProgramClassTypeDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<EducationProgramClassTypeDto>>.ReturnError("Greška prilikom dodavanja vrsta nastave edukacijskom programu.");
            }
        }

        public async Task<ActionResponse<EducationProgramClassTypeDto>> AddClassType(EducationProgramClassTypeDto eduCt)
        {
            try
            {
                var entityToAdd = mapper.Map<EducationProgramClassTypeDto, EducationProgramClassType>(eduCt);
                unitOfWork.GetGenericRepository<EducationProgramClassType>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<EducationProgramClassTypeDto>
                    .ReturnSuccess(mapper.Map<EducationProgramClassType, EducationProgramClassTypeDto>(entityToAdd),
                    "Vrsta nastave uspješno dodana edukacijskom programu.");
            }
            catch (Exception)
            {
                return await ActionResponse<EducationProgramClassTypeDto>.ReturnError("Greška prilikom dodavanja vrste nastave edukacijskom programu.");
            }
        }
    }
}
