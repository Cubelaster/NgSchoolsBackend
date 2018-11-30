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
    public class ClassLocationsController : ControllerBase
    {
        private readonly IClassLocationsService classLocationsService;

        public ClassLocationsController(IClassLocationsService classLocationsService)
        {
            this.classLocationsService = classLocationsService;
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<List<ClassLocationsDto>>> GetAll()
        {
            return await classLocationsService.GetAll();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<ClassLocationsDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await classLocationsService.GetAllPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<ClassLocationsDto>> GetById(SimpleRequestBase request)
        {
            return await classLocationsService.GetById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<ClassLocationsDto>> Insert([FromBody]ClassLocationsDto request)
        {
            return await classLocationsService.Insert(request);
        }

        [HttpPost]
        public async Task<ActionResponse<ClassLocationsDto>> Update([FromBody]ClassLocationsDto request)
        {
            return await classLocationsService.Update(request);
        }

        [HttpPost]
        public async Task<ActionResponse<ClassLocationsDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await classLocationsService.Delete(request.Id);
        }
    }
}