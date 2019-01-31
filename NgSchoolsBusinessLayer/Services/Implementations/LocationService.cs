using AutoMapper;
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
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class LocationService : ILocationService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;

        public LocationService(IMapper mapper, ILoggerService loggerService, IUnitOfWork unitOfWork,
            ICacheService cacheService)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
        }

        #endregion Ctors and Members

        #region Country

        public Task<ActionResponse<CountryDto>> DeleteCountry(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<List<CountryDto>>> GetAllCountries()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<PagedResult<CountryDto>>> GetAllCountriesPaged(BasePagedRequest pagedRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<PagedResult<CountryDto>>> GetCountriesBySearchQuery(BasePagedRequest pagedRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<CountryDto>> GetCountryById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResponse<CountryDto>> InsertCountry(CountryDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<CountryDto, Country>(entityDto);
                unitOfWork.GetGenericRepository<Country>().Add(entityToAdd);
                unitOfWork.Save();

                mapper.Map(entityToAdd, entityDto);
                return await ActionResponse<CountryDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<CountryDto>.ReturnError($"Greška prilikom upisa države.");
            }
            finally
            {
                await cacheService.RefreshCache<List<CountryDto>>();
            }
        }

        public async Task<ActionResponse<CountryDto>> UpdateCountry(CountryDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<CountryDto, Country>(entityDto);
                unitOfWork.GetGenericRepository<Country>().Update(entityToUpdate);
                unitOfWork.Save();
                unitOfWork.GetContext().Entry(entityToUpdate).Reference(p => p.Cities).Load();
                unitOfWork.GetContext().Entry(entityToUpdate).Reference(p => p.Regions).Load();

                return await ActionResponse<CountryDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<CountryDto>.ReturnError("Greška prilikom ažuriranja države.");
            }
            finally
            {
                await cacheService.RefreshCache<List<CountryDto>>();
            }
        }

        [CacheRefreshSource(typeof(CountryDto))]
        public async Task<ActionResponse<List<CountryDto>>> GetAllCountriesForCache()
        {
            try
            {
                var allEntities = unitOfWork.GetGenericRepository<Country>().GetAll(includeProperties: "Regions.Cities,Cities");
                return await ActionResponse<List<CountryDto>>.ReturnSuccess(
                    mapper.Map<List<Country>, List<CountryDto>>(allEntities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<CountryDto>>.ReturnError("Greška prilikom dohvata svih studenata.");
            }
        }

        #endregion Country

        #region Region

        public Task<ActionResponse<RegionDto>> DeleteRegion(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<List<RegionDto>>> GetAllRegions()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<PagedResult<RegionDto>>> GetAllRegionsPaged(BasePagedRequest pagedRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<RegionDto>> GetRegionById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsBySearchQuery(BasePagedRequest pagedRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<RegionDto>> UpdateRegion(RegionDto entityDto)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<RegionDto>> InsertRegion(RegionDto entityDto)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<List<RegionDto>>> GetAllRegionsForCache()
        {
            throw new NotImplementedException();
        }

        #endregion Region

        #region City

        public Task<ActionResponse<CityDto>> DeleteCity(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<List<CityDto>>> GetAllCities()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<PagedResult<CityDto>>> GetAllCitiesPaged(BasePagedRequest pagedRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<PagedResult<CityDto>>> GetCitiesBySearchQuery(BasePagedRequest pagedRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<CityDto>> GetCityById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<CityDto>> InsertCity(CityDto entityDto)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<CityDto>> UpdateCity(CityDto entityDto)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<List<CityDto>>> GetAllCitiessForCache()
        {
            throw new NotImplementedException();
        }

        #endregion City
    }
}
