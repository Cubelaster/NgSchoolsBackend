using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IssuedPrintController : ControllerBase
    {
        #region Ctors and Members

        private readonly IIssuedPrintService issuedPrintService;

        public IssuedPrintController(IIssuedPrintService issuedPrintService)
        {
            this.issuedPrintService = issuedPrintService;
        }

        #endregion Ctors and Members

        [HttpPost]
        public async Task<ActionResponse<IssuedPrintDto>> GetById(SimpleRequestBase request) => await issuedPrintService.GetById(request.Id);

        [HttpPost]
        public async Task<ActionResponse<List<IssuedPrintDto>>> GetAll() => await issuedPrintService.GetAll();

        [HttpPost]
        public async Task<ActionResponse<List<IssuedPrintDto>>> GetForStudentAndProgram(IssuedPrintDto request) => await issuedPrintService.GetForStudentAndProgram(request);

        [HttpPost]
        public async Task<ActionResponse<int>> GetForStudentAndProgramTotalDuplicates(IssuedPrintDto request) => await issuedPrintService.GetForStudentAndProgramTotalDuplicates(request);

        [HttpPost]
        public async Task<ActionResponse<Dictionary<DateTime, int>>> GetForCurrentYear(SimpleRequestBase request) => await issuedPrintService.GetForCurrentYear(request);

        [HttpPost]
        public async Task<ActionResponse<IssuedPrintDto>> Insert([FromBody] IssuedPrintDto entityDto) => await issuedPrintService.Insert(entityDto);

        [HttpPost]
        public async Task<ActionResponse<IssuedPrintDto>> Update([FromBody] IssuedPrintDto entityDto) => await issuedPrintService.Update(entityDto);

        [HttpPost]
        public async Task<ActionResponse<IssuedPrintDto>> Increment([FromBody] IssuedPrintDto entityDto) => await issuedPrintService.Increment(entityDto);

        [HttpPost]
        public async Task<ActionResponse<IssuedPrintDto>> Delete([FromBody] SimpleRequestBase request) => await issuedPrintService.Delete(request.Id);
    }
}
