using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.ViewModels.Locations;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;
        private readonly ILocationService locationService;

        public ClassLocationsService(IMapper mapper, IUnitOfWork unitOfWork,
            ICacheService cacheService, ILocationService locationService)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
            this.locationService = locationService;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<ClassLocationsDto>> GetById(int id)
        {
            try
            {
                var query = unitOfWork.GetGenericRepository<ClassLocations>()
                    .ReadAll()
                    .Where(e => e.Id == id);

                var entities = mapper.ProjectTo<ClassLocationsDto>(query).ToList();

                await locationService.AttachLocations(entities);

                return await ActionResponse<ClassLocationsDto>.ReturnSuccess(entities.Single());
            }
            catch (Exception)
            {
                return await ActionResponse<ClassLocationsDto>.ReturnError("Greška prilikom dohvata mjesta izvođenja.");
            }
        }

        public async Task<ActionResponse<List<ClassLocationsDto>>> GetAll()
        {
            try
            {
                var allOfThem = new List<ClassLocationsDto>();
                var cachedResponse = await cacheService.GetFromCache<List<ClassLocationsDto>>();
                if (cachedResponse.IsSuccessAndHasData(out allOfThem))
                {
                    return await ActionResponse<List<ClassLocationsDto>>.ReturnSuccess(allOfThem);
                }

                var query = unitOfWork.GetGenericRepository<ClassLocations>().ReadAll();

                var classLocations = mapper.ProjectTo<ClassLocationsDto>(query).ToList();

                await locationService.AttachLocations(classLocations);

                return await ActionResponse<List<ClassLocationsDto>>.ReturnSuccess(classLocations);
            }
            catch (Exception)
            {
                return await ActionResponse<List<ClassLocationsDto>>.ReturnError("Greška prilikom dohvata svih mjesta izvođenja.");
            }
        }

        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            try
            {
                return await ActionResponse<int>.ReturnSuccess(unitOfWork.GetGenericRepository<ClassLocations>().GetAllAsQueryable().Count());
            }
            catch (Exception)
            {
                return await ActionResponse<int>.ReturnError("Greška prilikom dohvata broja mjesta izvođenja.");
            }
        }

        [CacheRefreshSource(typeof(ClassLocationsDto))]
        public async Task<ActionResponse<List<ClassLocationsDto>>> GetAllForCache()
        {
            try
            {
                var query = unitOfWork.GetGenericRepository<ClassLocations>()
                    .ReadAll();

                var classLocations = mapper.ProjectTo<ClassLocationsDto>(query).ToList();

                await locationService.AttachLocations(classLocations);

                return await ActionResponse<List<ClassLocationsDto>>.ReturnSuccess(classLocations);
            }
            catch (Exception)
            {
                return await ActionResponse<List<ClassLocationsDto>>.ReturnError("Greška prilikom dohvata mjesta izvođenja za brzu memoriju.");
            }
        }

        public async Task<ActionResponse<PagedResult<ClassLocationsDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<ClassLocationsDto> classLocations = new List<ClassLocationsDto>();
                var cachedResponse = await cacheService.GetFromCache<List<ClassLocationsDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out classLocations))
                {
                    classLocations = (await GetAll()).GetData();
                }

                var pagedResult = await classLocations.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<ClassLocationsDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<ClassLocationsDto>>.ReturnError("Greška prilikom dohvata straničnih podataka mjesta izvođenja.");
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
            catch (Exception)
            {
                return await ActionResponse<ClassLocationsDto>.ReturnError("Greška prilikom upisa mjesta izvođenja.");
            }
            finally
            {
                await cacheService.RefreshCache<List<ClassLocationsDto>>();
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
            catch (Exception)
            {
                return await ActionResponse<ClassLocationsDto>.ReturnError("Greška prilikom ažuriranja mjesta izvođenja.");
            }
            finally
            {
                await cacheService.RefreshCache<List<ClassLocationsDto>>();
            }
        }

        public async Task<ActionResponse<ClassLocationsDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<ClassLocations>().Delete(id);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<ClassLocationsDto>>();
                return await ActionResponse<ClassLocationsDto>.ReturnSuccess(null, "Brisanje mjesta izvođenja uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<ClassLocationsDto>.ReturnError("Greška prilikom brisanja mjesta izvođenja!");
            }
            finally
            {
                await cacheService.RefreshCache<List<ClassLocationsDto>>();
            }
        }
    }
}
