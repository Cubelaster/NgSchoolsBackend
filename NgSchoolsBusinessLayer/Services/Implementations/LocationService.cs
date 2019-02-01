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

        public async Task<ActionResponse<CountryDto>> DeleteCountry(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Country>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<CountryDto>.ReturnSuccess(null, "Brisanje države uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<CountryDto>.ReturnError("Greška prilikom brisanja države.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
            }
        }

        public async Task<ActionResponse<List<CountryDto>>> GetAllCountries()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Country>()
                    .GetAll(includeProperties: "Regions.Cities,Cities");
                return await ActionResponse<List<CountryDto>>
                    .ReturnSuccess(mapper.Map<List<Country>, List<CountryDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<CountryDto>>.ReturnError("Greška prilikom dohvata svih država.");
            }
        }

        public async Task<ActionResponse<PagedResult<CountryDto>>> GetAllCountriesPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<CountryDto> countries = new List<CountryDto>();
                var cachedResponse = await cacheService.GetFromCache<List<CountryDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out countries))
                {
                    countries = (await GetAllCountries()).GetData();
                }

                var pagedResult = await countries.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<CountryDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<CountryDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za države.");
            }
        }

        public async Task<ActionResponse<PagedResult<CountryDto>>> GetCountriesBySearchQuery(BasePagedRequest pagedRequest)
        {
            try
            {
                List<CountryDto> countries = new List<CountryDto>();
                var cachedResponse = await cacheService.GetFromCache<List<CountryDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out countries))
                {
                    countries = (await GetAllCountries()).GetData();
                }

                var pagedResult = await countries.AsQueryable().GetBySearchQuery(pagedRequest);
                return await ActionResponse<PagedResult<CountryDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<CountryDto>>.ReturnError("Greška prilikom dohvata straničnih podataka država.");
            }
        }

        public async Task<ActionResponse<CountryDto>> GetCountryById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Country>()
                    .FindBy(c => c.Id == id, includeProperties: "Regions.Cities,Cities");
                return await ActionResponse<CountryDto>
                    .ReturnSuccess(mapper.Map<Country, CountryDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<CountryDto>.ReturnError("Greška prilikom dohvata države.");
            }
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
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
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
                mapper.Map(entityToUpdate, entityDto);

                return await ActionResponse<CountryDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<CountryDto>.ReturnError("Greška prilikom ažuriranja države.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
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

        public async Task<ActionResponse<RegionDto>> DeleteRegion(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Region>().Delete(id);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<RegionDto>>();
                return await ActionResponse<RegionDto>.ReturnSuccess(null, "Brisanje regije uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<RegionDto>.ReturnError("Greška prilikom brisanja regije.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
            }
        }

        public async Task<ActionResponse<List<RegionDto>>> GetAllRegions()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Region>()
                    .GetAll(includeProperties: "Cities");
                return await ActionResponse<List<RegionDto>>
                    .ReturnSuccess(mapper.Map<List<Region>, List<RegionDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<RegionDto>>.ReturnError("Greška prilikom dohvata svih županija.");
            }
        }

        public async Task<ActionResponse<PagedResult<RegionDto>>> GetAllRegionsPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<RegionDto> regions = new List<RegionDto>();
                var cachedResponse = await cacheService.GetFromCache<List<RegionDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out regions))
                {
                    regions = (await GetAllRegions()).GetData();
                }

                var pagedResult = await regions.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<RegionDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<RegionDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za gradove.");
            }
        }

        public async Task<ActionResponse<RegionDto>> GetRegionById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Region>()
                    .FindBy(c => c.Id == id, includeProperties: "Cities");
                return await ActionResponse<RegionDto>
                    .ReturnSuccess(mapper.Map<Region, RegionDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<RegionDto>.ReturnError("Greška prilikom dohvata države.");
            }
        }

        public async Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsBySearchQuery(BasePagedRequest pagedRequest)
        {
            try
            {
                List<RegionDto> regions = new List<RegionDto>();
                var cachedResponse = await cacheService.GetFromCache<List<RegionDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out regions))
                {
                    regions = (await GetAllRegions()).GetData();
                }

                var pagedResult = await regions.AsQueryable().GetBySearchQuery(pagedRequest);
                return await ActionResponse<PagedResult<RegionDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<RegionDto>>.ReturnError("Greška prilikom dohvata straničnih podataka država.");
            }
        }

        public async Task<ActionResponse<RegionDto>> UpdateRegion(RegionDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<RegionDto, Region>(entityDto);
                unitOfWork.GetGenericRepository<Region>().Update(entityToUpdate);
                unitOfWork.Save();
                unitOfWork.GetContext().Entry(entityToUpdate).Reference(p => p.Cities).Load();
                mapper.Map(entityToUpdate, entityDto);

                return await ActionResponse<RegionDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<RegionDto>.ReturnError($"Greška prilikom ažuriranja regije.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
            }
        }

        public async Task<ActionResponse<RegionDto>> InsertRegion(RegionDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<RegionDto, Region>(entityDto);
                unitOfWork.GetGenericRepository<Region>().Add(entityToAdd);
                unitOfWork.Save();

                mapper.Map(entityToAdd, entityDto);
                return await ActionResponse<RegionDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<RegionDto>.ReturnError($"Greška prilikom upisa regije.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
            }
        }

        [CacheRefreshSource(typeof(RegionDto))]
        public async Task<ActionResponse<List<RegionDto>>> GetAllRegionsForCache()
        {
            try
            {
                var allEntities = unitOfWork.GetGenericRepository<Region>()
                    .GetAll(includeProperties: "Cities");
                return await ActionResponse<List<RegionDto>>.ReturnSuccess(
                    mapper.Map<List<Region>, List<RegionDto>>(allEntities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<RegionDto>>.ReturnError("Greška prilikom dohvata svih gradova.");
            }
        }

        #endregion Region

        #region City

        public async Task<ActionResponse<CityDto>> DeleteCity(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<City>().Delete(id);
                unitOfWork.Save();
                await cacheService.RefreshCache<List<CityDto>>();
                return await ActionResponse<CityDto>.ReturnSuccess(null, "Brisanje grada uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<CityDto>.ReturnError("Greška prilikom brisanja grada.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
            }
        }

        public async Task<ActionResponse<List<CityDto>>> GetAllCities()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<City>().GetAll();
                return await ActionResponse<List<CityDto>>
                    .ReturnSuccess(mapper.Map<List<City>, List<CityDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<CityDto>>.ReturnError("Greška prilikom dohvata svih gradova.");
            }
        }

        public async Task<ActionResponse<PagedResult<CityDto>>> GetAllCitiesPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<CityDto> cities = new List<CityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<CityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out cities))
                {
                    cities = (await GetAllCities()).GetData();
                }

                var pagedResult = await cities.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<CityDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<CityDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za gradove.");
            }
        }

        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesBySearchQuery(BasePagedRequest pagedRequest)
        {
            try
            {
                List<CityDto> countries = new List<CityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<CityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out countries))
                {
                    countries = (await GetAllCities()).GetData();
                }

                var pagedResult = await countries.AsQueryable().GetBySearchQuery(pagedRequest);
                return await ActionResponse<PagedResult<CityDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<CityDto>>.ReturnError("Greška prilikom dohvata straničnih podataka gradova.");
            }
        }

        public async Task<ActionResponse<CityDto>> GetCityById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<City>()
                    .FindBy(c => c.Id == id);
                return await ActionResponse<CityDto>
                    .ReturnSuccess(mapper.Map<City, CityDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<CityDto>.ReturnError("Greška prilikom dohvata države.");
            }
        }

        public async Task<ActionResponse<CityDto>> InsertCity(CityDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<CityDto, City>(entityDto);
                unitOfWork.GetGenericRepository<City>().Add(entityToAdd);
                unitOfWork.Save();

                mapper.Map(entityToAdd, entityDto);
                return await ActionResponse<CityDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<CityDto>.ReturnError($"Greška prilikom upisa grada.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
            }
        }

        public async Task<ActionResponse<CityDto>> UpdateCity(CityDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<CityDto, City>(entityDto);
                unitOfWork.GetGenericRepository<City>().Update(entityToUpdate);
                unitOfWork.Save();
                mapper.Map(entityToUpdate, entityDto);

                return await ActionResponse<CityDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<CityDto>.ReturnError($"Greška prilikom ažuriranja regije.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
            }
        }

        [CacheRefreshSource(typeof(CityDto))]
        public async Task<ActionResponse<List<CityDto>>> GetAllCitiessForCache()
        {
            try
            {
                var allEntities = unitOfWork.GetGenericRepository<City>().GetAll();
                return await ActionResponse<List<CityDto>>.ReturnSuccess(
                    mapper.Map<List<City>, List<CityDto>>(allEntities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<CityDto>>.ReturnError("Greška prilikom dohvata svih gradova.");
            }
        }

        #endregion City
    }
}
