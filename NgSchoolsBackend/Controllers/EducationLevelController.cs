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

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<List<EducationLevelDto>>> GetAll()
        {
            return await educationLevelService.GetAll();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<EducationLevelDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await educationLevelService.GetAllPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationLevelDto>> GetById(int id)
        {
            return await educationLevelService.GetById(id);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationLevelDto>> Insert([FromBody]EducationLevelDto request)
        {
            return await educationLevelService.Insert(request);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationLevelDto>> Update([FromBody]EducationLevelDto request)
        {
            return await educationLevelService.Update(request);
        }
    }
}
