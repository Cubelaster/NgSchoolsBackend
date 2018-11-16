﻿using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InstitutionController
    {
        private readonly IInstitutionService institutionService;

        public InstitutionController(IInstitutionService institutionService)
        {
            this.institutionService = institutionService;
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<InstitutionDto>> Get()
        {
            return await institutionService.GetInstitution();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<InstitutionDto>> Insert([FromBody] InstitutionDto institution)
        {
            return await institutionService.Insert(institution);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<InstitutionDto>> Update([FromBody] InstitutionDto institution)
        {
            return await institutionService.Update(institution);
        }
    }
}
