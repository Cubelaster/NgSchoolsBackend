using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface ILocationService
    {
        #region Country

        Task<ActionResponse<List<CountryDto>>> GetAllCountriesForCache();
        Task<ActionResponse<CountryDto>> InsertCountry(CountryDto entityDto);
        Task<ActionResponse<CountryDto>> GetCountryById(int id);
        Task<ActionResponse<CountryDto>> GetCountryByCode(string alpha2Code);
        Task<ActionResponse<CountryDto>> UpdateCountry(CountryDto entityDto);
        Task<ActionResponse<List<CountryDto>>> GetAllCountries();
        Task<ActionResponse<PagedResult<CountryDto>>> GetAllCountriesPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<CountryDto>>> GetCountriesBySearchQuery(BasePagedRequest pagedRequest);
        Task<ActionResponse<CountryDto>> DeleteCountry(int id);

        #endregion Country

        #region Region

        Task<ActionResponse<List<RegionDto>>> GetAllRegionsForCache();
        Task<ActionResponse<RegionDto>> GetRegionById(int id);
        Task<ActionResponse<List<RegionDto>>> GetRegionsByCountryId(int id);
        Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsByCountryIdPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<RegionDto>> InsertRegion(RegionDto entityDto);
        Task<ActionResponse<RegionDto>> UpdateRegion(RegionDto entityDto);
        Task<ActionResponse<List<RegionDto>>> GetAllRegions();
        Task<ActionResponse<PagedResult<RegionDto>>> GetAllRegionsPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsBySearchQuery(BasePagedRequest pagedRequest);
        Task<ActionResponse<RegionDto>> DeleteRegion(int id);

        #endregion Region

        #region City

        Task<ActionResponse<List<CityDto>>> GetAllCitiesForCache();
        Task<ActionResponse<CityDto>> GetCityById(int id);
        Task<ActionResponse<List<CityDto>>> GetCitiesByRegionId(int id);
        Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByRegionIdPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<List<CityDto>>> GetCitiesByCountryId(int id);
        Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByCountryIdPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<List<CityDto>>> GetCitiesByMunicipalityId(int id);
        Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByMunicipalityIdPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<CityDto>> InsertCity(CityDto entityDto);
        Task<ActionResponse<CityDto>> UpdateCity(CityDto entityDto);
        Task<ActionResponse<List<CityDto>>> GetAllCities();
        Task<ActionResponse<List<CityDto>>> GetAllCitiesWithoutRegion();
        Task<ActionResponse<List<CityDto>>> GetAllCitiesWithoutMunicipality();
        Task<ActionResponse<PagedResult<CityDto>>> GetAllCitiesPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<CityDto>>> GetCitiesBySearchQuery(BasePagedRequest pagedRequest);
        Task<ActionResponse<CityDto>> DeleteCity(int id);

        #endregion City

        #region Municipality

        Task<ActionResponse<MunicipalityDto>> DeleteMunicipality(int id);
        Task<ActionResponse<List<MunicipalityDto>>> GetAllMunicipalities();
        Task<ActionResponse<List<MunicipalityDto>>> GetAllMunicipalitiesWithoutRegion();
        Task<ActionResponse<PagedResult<MunicipalityDto>>> GetAllMunicipalitiesPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<MunicipalityDto>> GetMunicipalityById(int id);
        Task<ActionResponse<List<MunicipalityDto>>> GetMunicipalitiesByCountryId(int id);
        Task<ActionResponse<List<MunicipalityDto>>> GetMunicipalitiesByRegionId(int id);
        Task<ActionResponse<PagedResult<MunicipalityDto>>> GetMunicipalitiesByCountryIdPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<MunicipalityDto>>> GetMunicipalitiesByRegionIdPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<MunicipalityDto>>> GetMunicipalitiesBySearchQuery(BasePagedRequest pagedRequest);
        Task<ActionResponse<MunicipalityDto>> UpdateMunicipality(MunicipalityDto entityDto);
        Task<ActionResponse<MunicipalityDto>> InsertMunicipality(MunicipalityDto entityDto);
        Task<ActionResponse<List<MunicipalityDto>>> InsertMunicipalities(List<MunicipalityDto> entityDtos);
        Task<ActionResponse<List<MunicipalityDto>>> GetAllMunicipalitiesForCache();

        #endregion Municipality

        Task<ActionResponse<(List<CountryDto> Countries, List<RegionDto> Regions, List<MunicipalityDto> Municipalities, List<CityDto> Cities)>> GetAllFromCache();

        Task<ActionResponse<T>> AttachLocations<T>(List<T> locationsHolders) where T : LocationsHolder;

        Task<ActionResponse<List<CountryDto>>> SeedLocationData();
    }
}