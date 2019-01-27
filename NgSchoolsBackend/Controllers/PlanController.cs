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
    public class PlanController : ControllerBase
    {
        private readonly IPlanService planService;
        public PlanController(IPlanService planService)
        {
            this.planService = planService;
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<List<PlanDto>>> GetAll()
        {
            return await planService.GetAll();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<PlanDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await planService.GetAllPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<PlanDto>> GetById(SimpleRequestBase request)
        {
            return await planService.GetById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<PlanDto>> GetByEducationProgramId(SimpleRequestBase request)
        {
            return await planService.GetByEducationProgramId(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<PlanDto>> Insert([FromBody]PlanDto request)
        {
            return await planService.Insert(request);
        }

        [HttpPost]
        public async Task<ActionResponse<PlanDto>> Update([FromBody]PlanDto request)
        {
            return await planService.Update(request);
        }

        [HttpPost]
        public async Task<ActionResponse<PlanDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await planService.Delete(request.Id);
        }
    }
}
