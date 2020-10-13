using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.ViewModels.Locations;
using NgSchoolsBusinessLayer.Services.Contracts;
using Core.Utilities.Attributes;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class LocationService : ILocationService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICacheService cacheService;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string battutaBaseUrl = "http://battuta.medunes.net/api/";
        private readonly string battutaApiKey = "key=ee64538820e420c329c5f164c550336b";
        private readonly object myLock = new object();
        private const string countryInclude = "Regions.Cities,Regions.Municipalities,Municipalities,Cities";
        private const string regionInclude = "Cities,Municipalities";
        private const string municipalityInclude = "Cities";

        public LocationService(IMapper mapper, IUnitOfWork unitOfWork,
            ICacheService cacheService, IHttpClientFactory httpClientFactory)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.cacheService = cacheService;
            this.httpClientFactory = httpClientFactory;
        }

        private async Task RefreshAllCache()
        {
            var countryTask = cacheService.RefreshCache<List<CountryDto>>();
            var regionTask = cacheService.RefreshCache<List<RegionDto>>();
            var municipalityTask = cacheService.RefreshCache<List<MunicipalityDto>>();
            var cityTask = cacheService.RefreshCache<List<CityDto>>();

            await Task.WhenAll(countryTask, regionTask, cityTask, municipalityTask);
        }

        #endregion Ctors and Members

        #region Cache Getter

        public async Task<ActionResponse<(List<CountryDto> Countries, List<RegionDto> Regions, List<MunicipalityDto> Municipalities, List<CityDto> Cities)>> GetAllFromCache()
        {
            try
            {
                var countryTask = cacheService.GetFromCache<List<CountryDto>>();
                var municipalityTask = cacheService.GetFromCache<List<MunicipalityDto>>();
                var regionTask = cacheService.GetFromCache<List<RegionDto>>();
                var cityTask = cacheService.GetFromCache<List<CityDto>>();

                await Task.WhenAll(countryTask, municipalityTask, regionTask, cityTask);

                var countryResult = await countryTask;
                var municipalityResult = await municipalityTask;
                var regionResult = await regionTask;
                var cityResult = await cityTask;

                if (countryResult.IsNotSuccess() || municipalityResult.IsNotSuccess() || regionResult.IsNotSuccess() || cityResult.IsNotSuccess())
                {
                    return await ActionResponse<(List<CountryDto>, List<RegionDto>, List<MunicipalityDto>, List<CityDto>)>
                        .ReturnError("Greška prilikom dohvata lokacijskih podataka iz brze memorije.");
                }

                var data = (Countries: countryResult.Data, Regions: regionResult.Data, Municipalities: municipalityResult.Data, Cities: cityResult.Data);
                return await ActionResponse<(List<CountryDto>, List<RegionDto>, List<MunicipalityDto>, List<CityDto>)>
                    .ReturnSuccess(data);
            }
            catch (Exception)
            {
                return await ActionResponse<(List<CountryDto>, List<RegionDto>, List<MunicipalityDto>, List<CityDto>)>
                    .ReturnError("Greška prilikom dohvata lokacijskih podataka.");
            }
        }

        #endregion Cache Getter

        #region Country

        public async Task<ActionResponse<CountryDto>> DeleteCountry(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Country>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<CountryDto>.ReturnSuccess(null, "Brisanje države uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<CountryDto>.ReturnError("Greška prilikom brisanja države.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();
                var municipalityTask = cacheService.RefreshCache<List<MunicipalityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
            }
        }

        public async Task<ActionResponse<List<CountryDto>>> GetAllCountries()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Country>()
                    .GetAll(includeProperties: countryInclude);
                return await ActionResponse<List<CountryDto>>
                    .ReturnSuccess(mapper.Map<List<Country>, List<CountryDto>>(entities));
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<PagedResult<CountryDto>>.ReturnError("Greška prilikom dohvata straničnih podataka država.");
            }
        }

        public async Task<ActionResponse<CountryDto>> GetCountryById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Country>()
                    .FindBy(c => c.Id == id, includeProperties: countryInclude);
                return await ActionResponse<CountryDto>
                    .ReturnSuccess(mapper.Map<Country, CountryDto>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<CountryDto>.ReturnError("Greška prilikom dohvata države.");
            }
        }

        public async Task<ActionResponse<CountryDto>> GetCountryByCode(string alpha2Code)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Country>()
                    .FindBy(c => c.Alpha2Code == alpha2Code, includeProperties: countryInclude);
                return await ActionResponse<CountryDto>
                    .ReturnSuccess(mapper.Map<Country, CountryDto>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<CountryDto>.ReturnError("Greška prilikom dohvata države.");
            }
        }

        public async Task<ActionResponse<List<CountryDto>>> InsertCountries(List<CountryDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<CountryDto>>.ReturnSuccess(entityDtos, "Države uspješno dodane.");
                entityDtos.ForEach(async country =>
                {
                    if ((await InsertCountry(country))
                        .IsNotSuccess(out ActionResponse<CountryDto> actionResponse, out country))
                    {
                        response = await ActionResponse<List<CountryDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<CountryDto>>.ReturnError($"Greška prilikom upisa države.");
            }
            finally
            {
                var countryTask = cacheService.RefreshCache<List<CountryDto>>();
                var regionTask = cacheService.RefreshCache<List<RegionDto>>();
                var cityTask = cacheService.RefreshCache<List<CityDto>>();

                await Task.WhenAll(countryTask, regionTask, cityTask);
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
            catch (Exception)
            {
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
                unitOfWork.GetContext().Entry(entityToUpdate).Collection(p => p.Cities).Load();
                unitOfWork.GetContext().Entry(entityToUpdate).Collection(p => p.Regions).Load();
                mapper.Map(entityToUpdate, entityDto);

                return await ActionResponse<CountryDto>.ReturnSuccess(entityDto);
            }
            catch (Exception)
            {
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
                var allEntities = unitOfWork.GetGenericRepository<Country>().GetAll(includeProperties: countryInclude);
                return await ActionResponse<List<CountryDto>>.ReturnSuccess(
                    mapper.Map<List<Country>, List<CountryDto>>(allEntities));
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<RegionDto>.ReturnError("Greška prilikom brisanja regije.");
            }
            finally
            {
                await RefreshAllCache();
            }
        }

        public async Task<ActionResponse<List<RegionDto>>> GetAllRegions()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Region>()
                    .GetAll(includeProperties: regionInclude);
                return await ActionResponse<List<RegionDto>>
                    .ReturnSuccess(mapper.Map<List<Region>, List<RegionDto>>(entities));
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<PagedResult<RegionDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za županije.");
            }
        }

        public async Task<ActionResponse<RegionDto>> GetRegionById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Region>()
                    .FindBy(c => c.Id == id, includeProperties: regionInclude);
                return await ActionResponse<RegionDto>
                    .ReturnSuccess(mapper.Map<Region, RegionDto>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<RegionDto>.ReturnError("Greška prilikom dohvata županije.");
            }
        }

        public async Task<ActionResponse<List<RegionDto>>> GetRegionsByCountryId(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Region>()
                    .GetAll(c => c.CountryId == id, includeProperties: regionInclude);
                return await ActionResponse<List<RegionDto>>
                    .ReturnSuccess(mapper.Map<List<Region>, List<RegionDto>>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<List<RegionDto>>.ReturnError("Greška prilikom dohvata regija za državu.");
            }
        }

        public async Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsByCountryIdPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<RegionDto> regions = new List<RegionDto>();
                var cachedResponse = await cacheService.GetFromCache<List<RegionDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out regions))
                {
                    regions = (await GetAllRegions()).GetData();
                }

                var pagedResult = await regions
                    .Where(r => r.CountryId == pagedRequest.AdditionalParams.Id)
                    .AsQueryable()
                    .GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<RegionDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<RegionDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za regije po državi.");
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
            catch (Exception)
            {
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
                unitOfWork.GetContext().Entry(entityToUpdate).Collection(p => p.Cities).Load();
                mapper.Map(entityToUpdate, entityDto);

                return await ActionResponse<RegionDto>.ReturnSuccess(entityDto);
            }
            catch (Exception)
            {
                return await ActionResponse<RegionDto>.ReturnError($"Greška prilikom ažuriranja regije.");
            }
            finally
            {
                await RefreshAllCache();
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
            catch (Exception)
            {
                return await ActionResponse<RegionDto>.ReturnError($"Greška prilikom upisa regije.");
            }
            finally
            {
                await RefreshAllCache();
            }
        }

        public async Task<ActionResponse<List<RegionDto>>> InsertRegions(List<RegionDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<RegionDto>>.ReturnSuccess(entityDtos, "Regije uspješno dodane.");
                entityDtos.ForEach(async region =>
                {
                    if ((await InsertRegion(region))
                        .IsNotSuccess(out ActionResponse<RegionDto> actionResponse, out region))
                    {
                        response = await ActionResponse<List<RegionDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<RegionDto>>.ReturnError($"Greška prilikom upisa regija.");
            }
            finally
            {
                await RefreshAllCache();
            }
        }

        [CacheRefreshSource(typeof(RegionDto))]
        public async Task<ActionResponse<List<RegionDto>>> GetAllRegionsForCache()
        {
            try
            {
                var allEntities = unitOfWork.GetGenericRepository<Region>()
                    .GetAll(includeProperties: regionInclude);
                return await ActionResponse<List<RegionDto>>.ReturnSuccess(
                    mapper.Map<List<Region>, List<RegionDto>>(allEntities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<RegionDto>>.ReturnError("Greška prilikom dohvata svih županija za brzu memoriju.");
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
            catch (Exception)
            {
                return await ActionResponse<CityDto>.ReturnError("Greška prilikom brisanja grada.");
            }
            finally
            {
                await RefreshAllCache();
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<CityDto>.ReturnError("Greška prilikom dohvata grada.");
            }
        }

        public async Task<ActionResponse<List<CityDto>>> GetCitiesByRegionId(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<City>().GetAll(c => c.RegionId == id);
                return await ActionResponse<List<CityDto>>
                    .ReturnSuccess(mapper.Map<List<City>, List<CityDto>>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<List<CityDto>>.ReturnError("Greška prilikom dohvata gradova za regiju.");
            }
        }

        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByRegionIdPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<CityDto> cities = new List<CityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<CityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out cities))
                {
                    cities = (await GetAllCities()).GetData();
                }

                var pagedResult = await cities
                    .Where(r => r.RegionId == pagedRequest.AdditionalParams.Id)
                    .AsQueryable()
                    .GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<CityDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<CityDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za gradove po regiji.");
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
            catch (Exception)
            {
                return await ActionResponse<CityDto>.ReturnError($"Greška prilikom upisa grada.");
            }
            finally
            {
                await RefreshAllCache();
            }
        }

        public async Task<ActionResponse<List<CityDto>>> InsertCitiesForRegion(RegionDto region)
        {
            try
            {
                if ((await GetCitiesForCountryCodeAndRegionBattuta("hr", region.Region))
                    .IsNotSuccess(out ActionResponse<List<CityDto>> citiesResponse, out List<CityDto> cities))
                {
                    return await ActionResponse<List<CityDto>>.ReturnError(citiesResponse.Message);
                }

                cities.ForEach(city =>
                {
                    city.CountryId = region.CountryId;
                    city.RegionId = region.Id;
                });

                return await InsertCities(cities);
            }
            catch (Exception)
            {
                return await ActionResponse<List<CityDto>>.ReturnError($"Greška prilikom upisa gradova za regiju.");
            }
        }

        public async Task<ActionResponse<List<CityDto>>> InsertCities(List<CityDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<CityDto>>.ReturnSuccess(entityDtos, "Gradovi uspješno dodani.");
                entityDtos.ForEach(async city =>
                {
                    if ((await InsertCity(city))
                        .IsNotSuccess(out ActionResponse<CityDto> actionResponse, out city))
                    {
                        response = await ActionResponse<List<CityDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<CityDto>>.ReturnError($"Greška prilikom upisa gradova.");
            }
            finally
            {
                await RefreshAllCache();
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
            catch (Exception)
            {
                return await ActionResponse<CityDto>.ReturnError($"Greška prilikom ažuriranja regije.");
            }
            finally
            {
                await RefreshAllCache();
            }
        }

        [CacheRefreshSource(typeof(CityDto))]
        public async Task<ActionResponse<List<CityDto>>> GetAllCitiesForCache()
        {
            try
            {
                var allEntities = unitOfWork.GetGenericRepository<City>().GetAll();
                return await ActionResponse<List<CityDto>>.ReturnSuccess(
                    mapper.Map<List<City>, List<CityDto>>(allEntities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<CityDto>>.ReturnError("Greška prilikom dohvata svih gradova.");
            }
        }

        public async Task<ActionResponse<List<CityDto>>> GetCitiesByCountryId(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<City>().GetAll(c => c.CountryId == id);
                return await ActionResponse<List<CityDto>>
                    .ReturnSuccess(mapper.Map<List<City>, List<CityDto>>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<List<CityDto>>.ReturnError("Greška prilikom dohvata gradova za državu.");
            }
        }

        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByCountryIdPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<CityDto> cities = new List<CityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<CityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out cities))
                {
                    cities = (await GetAllCities()).GetData();
                }

                var pagedResult = await cities
                    .Where(r => r.CountryId == pagedRequest.AdditionalParams.Id)
                    .AsQueryable()
                    .GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<CityDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<CityDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za gradove po državi.");
            }
        }

        public async Task<ActionResponse<List<CityDto>>> GetCitiesByMunicipalityId(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<City>().GetAll(c => c.MunicipalityId == id);
                return await ActionResponse<List<CityDto>>
                    .ReturnSuccess(mapper.Map<List<City>, List<CityDto>>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<List<CityDto>>.ReturnError("Greška prilikom dohvata gradova za općinu.");
            }
        }

        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByMunicipalityIdPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<CityDto> cities = new List<CityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<CityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out cities))
                {
                    cities = (await GetAllCities()).GetData();
                }

                var pagedResult = await cities
                    .Where(r => r.MunicipalityId == pagedRequest.AdditionalParams.Id)
                    .AsQueryable()
                    .GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<CityDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<CityDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za gradove po općini.");
            }
        }

        public async Task<ActionResponse<List<CityDto>>> GetAllCitiesWithoutRegion()
        {
            try
            {
                List<CityDto> cities = new List<CityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<CityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out cities))
                {
                    cities = (await GetAllCities()).GetData();
                }

                var citiesWithoutRegion = cities.Where(c => !c.RegionId.HasValue).ToList();
                return await ActionResponse<List<CityDto>>.ReturnSuccess(citiesWithoutRegion);
            }
            catch (Exception)
            {
                return await ActionResponse<List<CityDto>>.ReturnError("Greška prilikom dohvata gradova koji nemaju županiju.");
            }
        }

        public async Task<ActionResponse<List<CityDto>>> GetAllCitiesWithoutMunicipality()
        {
            try
            {
                List<CityDto> cities = new List<CityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<CityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out cities))
                {
                    cities = (await GetAllCities()).GetData();
                }

                var citiesWithoutMunicipality = cities.Where(c => !c.MunicipalityId.HasValue).ToList();
                return await ActionResponse<List<CityDto>>.ReturnSuccess(citiesWithoutMunicipality);
            }
            catch (Exception)
            {
                return await ActionResponse<List<CityDto>>.ReturnError("Greška prilikom dohvata gradova koji nemaju općinu.");
            }
        }

        #endregion City

        #region Municipality

        public async Task<ActionResponse<MunicipalityDto>> DeleteMunicipality(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Municipality>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<MunicipalityDto>.ReturnSuccess(null, "Brisanje općine uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<MunicipalityDto>.ReturnError("Greška prilikom brisanja općine.");
            }
            finally
            {
                await RefreshAllCache();
            }
        }

        public async Task<ActionResponse<List<MunicipalityDto>>> GetAllMunicipalities()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Municipality>()
                    .GetAll(includeProperties: municipalityInclude);
                return await ActionResponse<List<MunicipalityDto>>
                    .ReturnSuccess(mapper.Map<List<Municipality>, List<MunicipalityDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<MunicipalityDto>>.ReturnError("Greška prilikom dohvata svih općina.");
            }
        }

        public async Task<ActionResponse<PagedResult<MunicipalityDto>>> GetAllMunicipalitiesPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<MunicipalityDto> Municipalitys = new List<MunicipalityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<MunicipalityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out Municipalitys))
                {
                    Municipalitys = (await GetAllMunicipalities()).GetData();
                }

                var pagedResult = await Municipalitys.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<MunicipalityDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<MunicipalityDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za općine.");
            }
        }

        public async Task<ActionResponse<MunicipalityDto>> GetMunicipalityById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Municipality>()
                    .FindBy(c => c.Id == id, includeProperties: municipalityInclude);
                return await ActionResponse<MunicipalityDto>
                    .ReturnSuccess(mapper.Map<Municipality, MunicipalityDto>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<MunicipalityDto>.ReturnError("Greška prilikom dohvata općine.");
            }
        }

        public async Task<ActionResponse<List<MunicipalityDto>>> GetMunicipalitiesByCountryId(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Municipality>()
                    .GetAll(c => c.CountryId == id, includeProperties: municipalityInclude);
                return await ActionResponse<List<MunicipalityDto>>
                    .ReturnSuccess(mapper.Map<List<Municipality>, List<MunicipalityDto>>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<List<MunicipalityDto>>.ReturnError("Greška prilikom dohvata općina za državu.");
            }
        }

        public async Task<ActionResponse<PagedResult<MunicipalityDto>>> GetMunicipalitiesByCountryIdPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<MunicipalityDto> Municipalitys = new List<MunicipalityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<MunicipalityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out Municipalitys))
                {
                    Municipalitys = (await GetAllMunicipalities()).GetData();
                }

                var pagedResult = await Municipalitys
                    .Where(r => r.CountryId == pagedRequest.AdditionalParams.Id)
                    .AsQueryable()
                    .GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<MunicipalityDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<MunicipalityDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za općine po državi.");
            }
        }

        public async Task<ActionResponse<PagedResult<MunicipalityDto>>> GetMunicipalitiesBySearchQuery(BasePagedRequest pagedRequest)
        {
            try
            {
                List<MunicipalityDto> Municipalitys = new List<MunicipalityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<MunicipalityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out Municipalitys))
                {
                    Municipalitys = (await GetAllMunicipalities()).GetData();
                }

                var pagedResult = await Municipalitys.AsQueryable().GetBySearchQuery(pagedRequest);
                return await ActionResponse<PagedResult<MunicipalityDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<MunicipalityDto>>.ReturnError("Greška prilikom dohvata straničnih podataka država.");
            }
        }

        public async Task<ActionResponse<MunicipalityDto>> UpdateMunicipality(MunicipalityDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<MunicipalityDto, Municipality>(entityDto);
                unitOfWork.GetGenericRepository<Municipality>().Update(entityToUpdate);
                unitOfWork.Save();
                unitOfWork.GetContext().Entry(entityToUpdate).Collection(p => p.Cities).Load();
                mapper.Map(entityToUpdate, entityDto);

                return await ActionResponse<MunicipalityDto>.ReturnSuccess(entityDto);
            }
            catch (Exception)
            {
                return await ActionResponse<MunicipalityDto>.ReturnError($"Greška prilikom ažuriranja općine.");
            }
            finally
            {
                await RefreshAllCache();
            }
        }

        public async Task<ActionResponse<MunicipalityDto>> InsertMunicipality(MunicipalityDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<MunicipalityDto, Municipality>(entityDto);
                unitOfWork.GetGenericRepository<Municipality>().Add(entityToAdd);
                unitOfWork.Save();

                mapper.Map(entityToAdd, entityDto);
                return await ActionResponse<MunicipalityDto>.ReturnSuccess(entityDto);
            }
            catch (Exception)
            {
                return await ActionResponse<MunicipalityDto>.ReturnError($"Greška prilikom upisa općine.");
            }
            finally
            {
                await RefreshAllCache();
            }
        }

        public async Task<ActionResponse<List<MunicipalityDto>>> InsertMunicipalities(List<MunicipalityDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<MunicipalityDto>>.ReturnSuccess(entityDtos, "Općine uspješno dodane.");
                entityDtos.ForEach(async Municipality =>
                {
                    if ((await InsertMunicipality(Municipality))
                        .IsNotSuccess(out ActionResponse<MunicipalityDto> actionResponse, out Municipality))
                    {
                        response = await ActionResponse<List<MunicipalityDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<MunicipalityDto>>.ReturnError($"Greška prilikom upisa općina.");
            }
            finally
            {
                await RefreshAllCache();
            }
        }

        [CacheRefreshSource(typeof(MunicipalityDto))]
        public async Task<ActionResponse<List<MunicipalityDto>>> GetAllMunicipalitiesForCache()
        {
            try
            {
                var allEntities = unitOfWork.GetGenericRepository<Municipality>()
                    .GetAll(includeProperties: municipalityInclude);
                return await ActionResponse<List<MunicipalityDto>>.ReturnSuccess(
                    mapper.Map<List<Municipality>, List<MunicipalityDto>>(allEntities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<MunicipalityDto>>.ReturnError("Greška prilikom dohvata svih općina za brzu memoriju.");
            }
        }

        public async Task<ActionResponse<List<MunicipalityDto>>> GetMunicipalitiesByRegionId(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Municipality>()
                    .GetAll(c => c.RegionId == id, includeProperties: municipalityInclude);
                return await ActionResponse<List<MunicipalityDto>>
                    .ReturnSuccess(mapper.Map<List<Municipality>, List<MunicipalityDto>>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<List<MunicipalityDto>>.ReturnError("Greška prilikom dohvata općina za županiju.");
            }
        }

        public async Task<ActionResponse<PagedResult<MunicipalityDto>>> GetMunicipalitiesByRegionIdPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<MunicipalityDto> Municipalitys = new List<MunicipalityDto>();
                var cachedResponse = await cacheService.GetFromCache<List<MunicipalityDto>>();
                if (!cachedResponse.IsSuccessAndHasData(out Municipalitys))
                {
                    Municipalitys = (await GetAllMunicipalities()).GetData();
                }

                var pagedResult = await Municipalitys
                    .Where(r => r.RegionId == pagedRequest.AdditionalParams.Id)
                    .AsQueryable()
                    .GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<MunicipalityDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<MunicipalityDto>>.ReturnError("Greška prilikom dohvata straničnih podataka za općine po državi.");
            }
        }

        public async Task<ActionResponse<List<MunicipalityDto>>> GetAllMunicipalitiesWithoutRegion()
        {
            List<MunicipalityDto> Municipalities = new List<MunicipalityDto>();
            var cachedResponse = await cacheService.GetFromCache<List<MunicipalityDto>>();
            if (!cachedResponse.IsSuccessAndHasData(out Municipalities))
            {
                Municipalities = (await GetAllMunicipalities()).GetData();
            }
            var municipalitiesWithoutRegion = Municipalities.Where(m => !m.RegionId.HasValue).ToList();
            return await ActionResponse<List<MunicipalityDto>>.ReturnSuccess(municipalitiesWithoutRegion);
        }

        #endregion Municipality

        #region Helpers

        public async Task<ActionResponse<T>> AttachLocations<T>(List<T> locationsHolders) where T : LocationsHolder
        {
            try
            {
                var locationResponse = await GetAllFromCache();
                if (!locationResponse.IsSuccessAndHasData(out (List<CountryDto> Countries, List<RegionDto> Regions, List<MunicipalityDto> Municipalities, List<CityDto> Cities) locationResult))
                {
                    return await ActionResponse<T>.ReturnError(locationResponse.Message);
                }

                locationsHolders.ForEach(entity =>
                {
                    entity.City = mapper.Map<CityViewModel>(locationResult.Cities.FirstOrDefault(e => e.Id == entity.CityId));
                    entity.Country = mapper.Map<CountryViewModel>(locationResult.Countries.FirstOrDefault(e => e.Id == entity.CountryId));
                    entity.Municipality = mapper.Map<MunicipalityViewModel>(locationResult.Municipalities.FirstOrDefault(e => e.Id == entity.MunicipalityId));
                    entity.Region = mapper.Map<RegionViewModel>(locationResult.Regions.FirstOrDefault(e => e.Id == entity.RegionId));
                });

                return await ActionResponse<T>.ReturnSuccess(locationsHolders);
            }
            catch (Exception)
            {
                return await ActionResponse<T>.ReturnError("Greška prilikom dodjeljivanja lokacijskih podataka.");
            }
        }

        #endregion Helpers

        #region BattutaClient

        public async Task<ActionResponse<List<CountryDto>>> GetAllCountriesBattuta()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{battutaBaseUrl}country/all/?{battutaApiKey}");
                var client = httpClientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var countriesData = await response.Content.ReadAsAsync<List<CountryDto>>();
                    return await ActionResponse<List<CountryDto>>.ReturnSuccess(countriesData);
                }
                return await ActionResponse<List<CountryDto>>.ReturnError("Greška kod dohvata podataka svih država s online referenta." + response.ReasonPhrase);
            }
            catch (Exception)
            {
                return await ActionResponse<List<CountryDto>>.ReturnError("Greška prilikom dohvata svih država s battute.");
            }
        }

        public async Task<ActionResponse<List<RegionDto>>> GetRegionsForCountryCodeBattuta(string countryCode)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{battutaBaseUrl}region/{countryCode}/all/?{battutaApiKey}");
                var client = httpClientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var regionsData = await response.Content.ReadAsAsync<List<RegionDto>>();
                    return await ActionResponse<List<RegionDto>>.ReturnSuccess(regionsData);
                }
                return await ActionResponse<List<RegionDto>>.ReturnError("Greška kod dohvata podataka regija sa online referenta." + response.ReasonPhrase);
            }
            catch (Exception)
            {
                return await ActionResponse<List<RegionDto>>.ReturnError("Greška prilikom dohvata svih država s battute.");
            }
        }

        public async Task<ActionResponse<List<CityDto>>> GetCitiesForCountryCodeAndRegionBattuta(string countryCode, string regionHint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{battutaBaseUrl}city/{countryCode}/search/?region={regionHint}&{battutaApiKey}");
                var client = httpClientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var citiesData = await response.Content.ReadAsAsync<List<CityDto>>();
                    return await ActionResponse<List<CityDto>>.ReturnSuccess(citiesData);
                }
                return await ActionResponse<List<CityDto>>.ReturnError("Greška kod dohvata podataka gradova sa online referenta." + response.ReasonPhrase);
            }
            catch (Exception)
            {
                return await ActionResponse<List<CityDto>>.ReturnError("Greška prilikom dohvata gradova s battute.");
            }
        }

        public async Task<ActionResponse<List<CountryDto>>> SeedLocationData()
        {
            try
            {
                if ((await GetAllCountries()).IsNotSuccess(out ActionResponse<List<CountryDto>> getResponse, out List<CountryDto> countries))
                {
                    return getResponse;
                }

                if (countries.Count < 1)
                {
                    if ((await GetAllCountriesBattuta()).IsNotSuccess(out getResponse, out countries))
                    {
                        return getResponse;
                    }

                    if ((await InsertCountries(countries)).IsNotSuccess(out getResponse, out countries))
                    {
                        return getResponse;
                    }
                }

                return getResponse;
            }
            catch (Exception)
            {
                return await ActionResponse<List<CountryDto>>.ReturnError("Greška prilikom dohvata svih država s battute.");
            }
        }

        #endregion BattutaClient
    }
}
