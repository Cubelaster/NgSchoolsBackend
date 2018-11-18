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
    public class ClassTypeController
    {
        private readonly IClassTypeService classTypeService;

        public ClassTypeController(IClassTypeService classTypeService)
        {
            this.classTypeService = classTypeService;
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<ClassTypeDto>> GetById(SimpleRequestBase request)
        {
            return await classTypeService.GetById(request.Id);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<ClassTypeDto>> Insert([FromBody] ClassTypeDto classType)
        {
            return await classTypeService.Insert(classType);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<ClassTypeDto>> Update([FromBody] ClassTypeDto classType)
        {
            return await classTypeService.Update(classType);
        }

        [HttpPost]
        public async Task<ActionResponse<List<ClassTypeDto>>> GetAll()
        {
            return await classTypeService.GetAll();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<ClassTypeDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await classTypeService.GetAllClassTypesPaged(pagedRequest);
        }
    }
}
