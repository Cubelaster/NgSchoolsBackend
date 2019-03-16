using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> GetCountryById(SimpleRequestBase request)
        {
            return await locationService.GetCountryById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> InsertCountry([FromBody] CountryDto entityDto)
        {
            return await locationService.InsertCountry(entityDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> UpdateCountry([FromBody] CountryDto entityDto)
        {
            return await locationService.UpdateCountry(entityDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<CountryDto>>> GetAllCountries()
        {
            return await locationService.GetAllCountries();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CountryDto>>> GetAllCountriesPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllCountriesPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CountryDto>>> GetCountriesBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCountriesBySearchQuery(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CountryDto>> DeleteCountry([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteCountry(request.Id);
        }

        #endregion Country

        #region Region

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> GetRegionById(SimpleRequestBase request)
        {
            return await locationService.GetRegionById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<RegionDto>>> GetRegionsByCountryId(SimpleRequestBase request)
        {
            return await locationService.GetRegionsByCountryId(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> InsertRegion([FromBody] RegionDto entityDto)
        {
            return await locationService.InsertRegion(entityDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> UpdateRegion([FromBody] RegionDto entityDto)
        {
            return await locationService.UpdateRegion(entityDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<RegionDto>>> GetAllRegions()
        {
            return await locationService.GetAllRegions();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<RegionDto>>> GetAllRegionsPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllRegionsPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsByCountryIdPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetRegionsByCountryIdPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<RegionDto>>> GetRegionsBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetRegionsBySearchQuery(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<RegionDto>> DeleteRegion([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteRegion(request.Id);
        }

        #endregion Region

        #region City

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CityDto>> GetCityById(SimpleRequestBase request)
        {
            return await locationService.GetCityById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<CityDto>>> GetCitiesByRegionId(SimpleRequestBase request)
        {
            return await locationService.GetCitiesByRegionId(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByRegionIdPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCitiesByRegionIdPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<CityDto>>> GetCitiesByMunicipalityId(SimpleRequestBase request)
        {
            return await locationService.GetCitiesByMunicipalityId(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByMunicipalityIdPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCitiesByMunicipalityIdPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<CityDto>>> GetCitiesByCountryId(SimpleRequestBase request)
        {
            return await locationService.GetCitiesByCountryId(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesByCountryIdPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCitiesByCountryIdPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CityDto>> InsertCity([FromBody] CityDto entityDto)
        {
            return await locationService.InsertCity(entityDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CityDto>> UpdateCity([FromBody] CityDto entityDto)
        {
            return await locationService.UpdateCity(entityDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<CityDto>>> GetAllCities()
        {
            return await locationService.GetAllCities();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<CityDto>>> GetAllCitiesWithoutRegion()
        {
            return await locationService.GetAllCitiesWithoutRegion();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<CityDto>>> GetAllCitiesWithoutMunicipality()
        {
            return await locationService.GetAllCitiesWithoutMunicipality();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetAllCitiesPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllCitiesPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CityDto>>> GetCitiesBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetCitiesBySearchQuery(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CityDto>> DeleteCity([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteCity(request.Id);
        }

        #endregion City

        #region Municipality

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<MunicipalityDto>> DeleteMunicipality([FromBody] SimpleRequestBase request)
        {
            return await locationService.DeleteMunicipality(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<MunicipalityDto>> GetMunicipalityById(SimpleRequestBase request)
        {
            return await locationService.GetMunicipalityById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<MunicipalityDto>>> GetMunicipalitysByCountryId(SimpleRequestBase request)
        {
            return await locationService.GetMunicipalitiesByCountryId(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<MunicipalityDto>>> GetMunicipalitysByRegionId(SimpleRequestBase request)
        {
            return await locationService.GetMunicipalitiesByRegionId(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<MunicipalityDto>> InsertMunicipality([FromBody] MunicipalityDto entityDto)
        {
            return await locationService.InsertMunicipality(entityDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<MunicipalityDto>> UpdateMunicipality([FromBody] MunicipalityDto entityDto)
        {
            return await locationService.UpdateMunicipality(entityDto);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<MunicipalityDto>>> GetAllMunicipalitys()
        {
            return await locationService.GetAllMunicipalities();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<MunicipalityDto>>> GetAllMunicipalitiesWithoutRegion()
        {
            return await locationService.GetAllMunicipalitiesWithoutRegion();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<MunicipalityDto>>> GetAllMunicipalitysPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetAllMunicipalitiesPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<MunicipalityDto>>> GetMunicipalitysByCountryIdPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetMunicipalitiesByCountryIdPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<MunicipalityDto>>> GetMunicipalitysByRegionIdPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetMunicipalitiesByRegionIdPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<MunicipalityDto>>> GetMunicipalitysBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await locationService.GetMunicipalitiesBySearchQuery(pagedRequest);
        }

        #endregion Municipality
    }
}
