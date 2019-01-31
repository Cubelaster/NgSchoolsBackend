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
    public class EducationProgramController : ControllerBase
    {
        private readonly IEducationProgramService educationProgramService;
        public EducationProgramController(IEducationProgramService educationProgramService)
        {
            this.educationProgramService = educationProgramService;
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<List<EducationProgramDto>>> GetAll()
        {
            return await educationProgramService.GetAll();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<EducationProgramDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await educationProgramService.GetAllPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<PagedResult<EducationProgramDto>>> GetBySearchQuery([FromBody] BasePagedRequest pagedRequest)
        {
            return await educationProgramService.GetBySearchQuery(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationProgramDto>> GetById(SimpleRequestBase request)
        {
            return await educationProgramService.GetById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationProgramDto>> Insert([FromBody]EducationProgramDto request)
        {
            return await educationProgramService.Insert(request);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationProgramDto>> Update([FromBody]EducationProgramDto request)
        {
            return await educationProgramService.Update(request);
        }

        [HttpPost]
        public async Task<ActionResponse<EducationProgramDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await educationProgramService.Delete(request.Id);
        }
    }
}
