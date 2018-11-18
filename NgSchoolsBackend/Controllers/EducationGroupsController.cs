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
    public class EducationGroupsController : ControllerBase
    {
        private readonly IEducationGroupService educationGroupService;

        public EducationGroupsController(IEducationGroupService educationGroupService)
        {
            this.educationGroupService = educationGroupService;
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<List<EducationGroupDto>>> GetAll()
        {
            return await educationGroupService.GetAll();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<EducationGroupDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await educationGroupService.GetAllPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationGroupDto>> GetById(SimpleRequestBase request)
        {
            return await educationGroupService.GetById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationGroupDto>> Insert([FromBody]EducationGroupDto request)
        {
            return await educationGroupService.Insert(request);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationGroupDto>> Update([FromBody]EducationGroupDto request)
        {
            return await educationGroupService.Update(request);
        }
    }
}
