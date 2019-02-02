using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService locationService;

        public LocationController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        #region Country

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> GetCountryById(SimpleRequestBase request)
        {
            return await locationService.GetCountryById(request.Id);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> InsertCountry([FromBody] CountryDto entityDto)
        {
            return await locationService.InsertCountry(entityDto);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> UpdateCountry([FromBody] CountryDto entityDto)
        {
            return await locationService.UpdateCountry(entityDto);
        }

        [HttpPost]
        public async Task<ActionResponse<List<CountryDto>>> GetAllCountries()
        {
            return await locationService.GetAllCountries();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CountryDto>>> GetAllCountriesPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllCountriesPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<PagedResult<CountryDto>>> GetCountriesBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCountriesBySearchQuery(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<CountryDto>> DeleteCountry([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteCountry(request.Id);
        }

        #endregion Country

        #region Region

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> GetRegionById(SimpleRequestBase request)
        {
            return await locationService.GetRegionById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<RegionDto>> GetRegionsByCountryId(SimpleRequestBase request)
        {
            return await locationService.GetRegionsByCountryId(request.Id);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> InsertRegion([FromBody] RegionDto entityDto)
        {
            return await locationService.InsertRegion(entityDto);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> UpdateRegion([FromBody] RegionDto entityDto)
        {
            return await locationService.UpdateRegion(entityDto);
        }

        [HttpPost]
        public async Task<ActionResponse<List<RegionDto>>> GetAllRegions()
        {
            return await locationService.GetAllRegions();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<RegionDto>>> GetAllRegionsPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllRegionsPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetRegionsBySearchQuery(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<RegionDto>> DeleteRegion([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteRegion(request.Id);
        }

        #endregion Region

        #region City

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<CityDto>> GetCityById(SimpleRequestBase request)
        {
            return await locationService.GetCityById(request.Id);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<CityDto>> GetCitiesByRegionId(SimpleRequestBase request)
        {
            return await locationService.GetCitiesByRegionId(request.Id);
        }


        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<CityDto>> InsertCity([FromBody] CityDto entityDto)
        {
            return await locationService.InsertCity(entityDto);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<CityDto>> UpdateCity([FromBody] CityDto entityDto)
        {
            return await locationService.UpdateCity(entityDto);
        }

        [HttpPost]
        public async Task<ActionResponse<List<CityDto>>> GetAllCities()
        {
            return await locationService.GetAllCities();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetAllCitiesPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllCitiesPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCitiesBySearchQuery(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<CityDto>> DeleteCity([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteCity(request.Id);
        }

        #endregion City
    }
}
