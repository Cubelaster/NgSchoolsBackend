using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using System;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResponse<ApplicationDto>> Get()
        {
            var result = ActionResponse<ApplicationDto>.ReturnSuccess(new ApplicationDto {
                Version = Environment.Version,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
            return await result;
        }
    }
}
