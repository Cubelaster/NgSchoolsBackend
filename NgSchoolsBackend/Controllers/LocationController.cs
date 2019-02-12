using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> GetCountryById(SimpleRequestBase request)
        {
            return await locationService.GetCountryById(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> InsertCountry([FromBody] CountryDto entityDto)
        {
            return await locationService.InsertCountry(entityDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> UpdateCountry([FromBody] CountryDto entityDto)
        {
            return await locationService.UpdateCountry(entityDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<CountryDto>>> GetAllCountries()
        {
            return await locationService.GetAllCountries();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CountryDto>>> GetAllCountriesPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllCountriesPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CountryDto>>> GetCountriesBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCountriesBySearchQuery(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> DeleteCountry([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteCountry(request.Id);
        }

        #endregion Country

        #region Region

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> GetRegionById(SimpleRequestBase request)
        {
            return await locationService.GetRegionById(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<RegionDto>>> GetRegionsByCountryId(SimpleRequestBase request)
        {
            return await locationService.GetRegionsByCountryId(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> InsertRegion([FromBody] RegionDto entityDto)
        {
            return await locationService.InsertRegion(entityDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> UpdateRegion([FromBody] RegionDto entityDto)
        {
            return await locationService.UpdateRegion(entityDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<RegionDto>>> GetAllRegions()
        {
            return await locationService.GetAllRegions();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<RegionDto>>> GetAllRegionsPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllRegionsPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsByCountryIdPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetRegionsByCountryIdPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetRegionsBySearchQuery(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> DeleteRegion([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteRegion(request.Id);
        }

        #endregion Region

        #region City

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<CityDto>> GetCityById(SimpleRequestBase request)
        {
            return await locationService.GetCityById(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<CityDto>>> GetCitiesByRegionId(SimpleRequestBase request)
        {
            return await locationService.GetCitiesByRegionId(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByRegionIdPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCitiesByRegionIdPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<CityDto>> InsertCity([FromBody] CityDto entityDto)
        {
            return await locationService.InsertCity(entityDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<CityDto>> UpdateCity([FromBody] CityDto entityDto)
        {
            return await locationService.UpdateCity(entityDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<CityDto>>> GetAllCities()
        {
            return await locationService.GetAllCities();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetAllCitiesPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllCitiesPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCitiesBySearchQuery(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<CityDto>> DeleteCity([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteCity(request.Id);
        }

        #endregion City
    }
}
