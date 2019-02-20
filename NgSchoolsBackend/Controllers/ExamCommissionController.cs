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
    public class ExamCommissionController : ControllerBase
    {
        private readonly IExamCommissionService examCommissionService;

        public ExamCommissionController(IExamCommissionService examCommissionService)
        {
            this.examCommissionService = examCommissionService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<ExamCommissionDto>>> GetAll()
        {
            return await examCommissionService.GetAll();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<ExamCommissionDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await examCommissionService.GetAllPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<ExamCommissionDto>> GetById(SimpleRequestBase request)
        {
            return await examCommissionService.GetById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<ExamCommissionDto>> Insert([FromBody]ExamCommissionDto request)
        {
            return await examCommissionService.Insert(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<ExamCommissionDto>> Update([FromBody] ExamCommissionDto request)
        {
            return await examCommissionService.Update(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<ExamCommissionDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await examCommissionService.Delete(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            return await examCommissionService.GetTotalNumber();
        }
    }
}