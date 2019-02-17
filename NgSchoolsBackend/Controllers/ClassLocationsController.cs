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
    public class ClassLocationsController : ControllerBase
    {
        private readonly IClassLocationsService classLocationsService;

        public ClassLocationsController(IClassLocationsService classLocationsService)
        {
            this.classLocationsService = classLocationsService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<ClassLocationsDto>>> GetAll()
        {
            return await classLocationsService.GetAll();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<ClassLocationsDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await classLocationsService.GetAllPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<ClassLocationsDto>> GetById(SimpleRequestBase request)
        {
            return await classLocationsService.GetById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<ClassLocationsDto>> Insert([FromBody]ClassLocationsDto request)
        {
            return await classLocationsService.Insert(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<ClassLocationsDto>> Update([FromBody]ClassLocationsDto request)
        {
            return await classLocationsService.Update(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<ClassLocationsDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await classLocationsService.Delete(request.Id);
        }
    }
}
