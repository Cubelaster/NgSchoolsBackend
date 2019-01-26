﻿using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DiaryController : ControllerBase
    {
        private readonly IDiaryService diaryService;
        public DiaryController(IDiaryService diaryService)
        {
            this.diaryService = diaryService;
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<List<DiaryDto>>> GetAll()
        {
            return await diaryService.GetAll();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<DiaryDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await diaryService.GetAllPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<DiaryDto>> GetById(SimpleRequestBase request)
        {
            return await diaryService.GetById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<DiaryDto>> Insert([FromBody]DiaryDto request)
        {
            return await diaryService.Insert(request);
        }

        [HttpPost]
        public async Task<ActionResponse<DiaryDto>> Update([FromBody]DiaryDto request)
        {
            return await diaryService.Update(request);
        }

        [HttpPost]
        public async Task<ActionResponse<DiaryDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await diaryService.Delete(request.Id);
        }
    }
}