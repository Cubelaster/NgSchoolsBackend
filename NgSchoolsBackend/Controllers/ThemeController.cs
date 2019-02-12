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
    public class ThemeController : ControllerBase
    {
        private readonly IThemeService themeService;
        public ThemeController(IThemeService themeService)
        {
            this.themeService = themeService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<ThemeDto>>> GetAll()
        {
            return await themeService.GetAll();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<ThemeDto>>> GetAllBySubjectId(SimpleRequestBase request)
        {
            return await themeService.GetAllBySubjectId(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<ThemeDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await themeService.GetAllPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<ThemeDto>> GetById(SimpleRequestBase request)
        {
            return await themeService.GetById(request.Id);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<ThemeDto>> Insert([FromBody]ThemeDto request)
        {
            return await themeService.Insert(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<ThemeDto>> Update([FromBody]ThemeDto request)
        {
            return await themeService.Update(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<ThemeDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await themeService.Delete(request.Id);
        }
    }
}
