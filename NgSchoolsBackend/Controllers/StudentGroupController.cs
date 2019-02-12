﻿using Microsoft.AspNetCore.Authorization;
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
    public class StudentGroupController : ControllerBase
    {
        private readonly IStudentGroupService studentGroupService;
        public StudentGroupController(IStudentGroupService studentGroupService)
        {
            this.studentGroupService = studentGroupService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<StudentGroupDto>>> GetAll()
        {
            return await studentGroupService.GetAll();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<StudentGroupDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await studentGroupService.GetAllPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDto>> GetById(SimpleRequestBase request)
        {
            return await studentGroupService.GetById(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDto>> Insert([FromBody]StudentGroupDto request)
        {
            return await studentGroupService.Insert(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDto>> Update([FromBody]StudentGroupDto request)
        {
            return await studentGroupService.Update(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await studentGroupService.Delete(request.Id);
        }
    }
}
