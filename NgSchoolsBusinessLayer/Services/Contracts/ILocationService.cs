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
        Task<ActionResponse<List<CountryDto>>> GetAllCountriesForCache();
        Task<ActionResponse<List<CityDto>>> GetAllCitiessForCache();
        Task<ActionResponse<List<RegionDto>>> GetAllRegionsForCache();
        Task<ActionResponse<CountryDto>> InsertCountry(CountryDto entityDto);
        Task<ActionResponse<CountryDto>> GetCountryById(int id);
        Task<ActionResponse<CountryDto>> UpdateCountry(CountryDto entityDto);
        Task<ActionResponse<List<CountryDto>>> GetAllCountries();
        Task<ActionResponse<PagedResult<CountryDto>>> GetAllCountriesPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<CountryDto>>> GetCountriesBySearchQuery(BasePagedRequest pagedRequest);
        Task<ActionResponse<CountryDto>> DeleteCountry(int id);
        Task<ActionResponse<RegionDto>> GetRegionById(int id);
        Task<ActionResponse<RegionDto>> InsertRegion(RegionDto entityDto);
        Task<ActionResponse<RegionDto>> UpdateRegion(RegionDto entityDto);
        Task<ActionResponse<List<RegionDto>>> GetAllRegions();
        Task<ActionResponse<PagedResult<RegionDto>>> GetAllRegionsPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsBySearchQuery(BasePagedRequest pagedRequest);
        Task<ActionResponse<RegionDto>> DeleteRegion(int id);
        Task<ActionResponse<CityDto>> GetCityById(int id);
        Task<ActionResponse<CityDto>> InsertCity(CityDto entityDto);
        Task<ActionResponse<CityDto>> UpdateCity(CityDto entityDto);
        Task<ActionResponse<List<CityDto>>> GetAllCities();
        Task<ActionResponse<PagedResult<CityDto>>> GetAllCitiesPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<CityDto>>> GetCitiesBySearchQuery(BasePagedRequest pagedRequest);
        Task<ActionResponse<CityDto>> DeleteCity(int id);
    }
}