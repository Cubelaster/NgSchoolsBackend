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
    public class EducationLevelController : ControllerBase
    {
        private readonly IEducationLevelService educationLevelService;

        public EducationLevelController(IEducationLevelService educationLevelService)
        {
            this.educationLevelService = educationLevelService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<EducationLevelDto>>> GetAll()
        {
            return await educationLevelService.GetAll();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<EducationLevelDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await educationLevelService.GetAllPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<EducationLevelDto>> GetById(SimpleRequestBase request)
        {
            return await educationLevelService.GetById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<EducationLevelDto>> Insert([FromBody]EducationLevelDto request)
        {
            return await educationLevelService.Insert(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<EducationLevelDto>> Update([FromBody]EducationLevelDto request)
        {
            return await educationLevelService.Update(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<EducationLevelDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await educationLevelService.Delete(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            return await educationLevelService.GetTotalNumber();
        }
    }
}
