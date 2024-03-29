﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<SubjectDto>>> GetAll()
        {
            return await subjectService.GetAll();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<SubjectDto>>> GetAllByEducationProgramId(SimpleRequestBase request)
        {
            return await subjectService.GetAllByEducationProgramId(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<SubjectDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await subjectService.GetAllPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<SubjectDto>> GetById(SimpleRequestBase request)
        {
            return await subjectService.GetById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<SubjectDto>> Insert([FromBody]SubjectDto request)
        {
            return await subjectService.Insert(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<SubjectDto>> Update([FromBody]SubjectDto request)
        {
            return await subjectService.Update(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<SubjectDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await subjectService.Delete(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            return await subjectService.GetTotalNumber();
        }
    }
}
