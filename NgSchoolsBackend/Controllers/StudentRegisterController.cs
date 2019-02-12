﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentRegisterController
    {
        private readonly IStudentRegisterService studentRegisterService;

        public StudentRegisterController(IStudentRegisterService studentRegisterService)
        {
            this.studentRegisterService = studentRegisterService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<StudentRegisterDto>> GetById(SimpleRequestBase request)
        {
            return await studentRegisterService.GetById(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<StudentRegisterDto>>> GetAll()
        {
            return await studentRegisterService.GetAll();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<StudentRegisterDto>>> GetAllNotFull()
        {
            return await studentRegisterService.GetAllNotFull();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<StudentRegisterDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            return await studentRegisterService.GetAllPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<StudentRegisterEntryDto>> GetEntryById(SimpleRequestBase request)
        {
            return await studentRegisterService.GetEntryById(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<StudentRegisterEntryDto>>> GetAllEntries()
        {
            return await studentRegisterService.GetAllEntries();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<StudentRegisterEntryDto>>> GetAllEntriesPaged(BasePagedRequest pagedRequest)
        {
            return await studentRegisterService.GetAllEntriesPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<StudentRegisterEntryDto>>> GetAllEntriesByBookIdPaged(BasePagedRequest pagedRequest)
        {
            return await studentRegisterService.GetAllEntriesByBookIdPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<StudentRegisterEntryDto>> InsertEntry(StudentRegisterEntryInsertRequest request)
        {
            return await studentRegisterService.InsertEntry(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<StudentRegisterEntryDto>> UpdateEntry(StudentRegisterEntryInsertRequest request)
        {
            return await studentRegisterService.UpdateEntry(request);
        }
    }
}
