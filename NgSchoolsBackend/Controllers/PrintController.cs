using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PrintController : ControllerBase
    {
        #region Ctors and Members

        private readonly IStudentGroupService studentGroupService;
        private readonly IStudentService studentService;
        private readonly IStudentRegisterService studentRegisterService;

        public PrintController(IStudentGroupService studentGroupService, 
            IStudentService studentService, IStudentRegisterService studentRegisterService)
        {
            this.studentGroupService = studentGroupService;
            this.studentService = studentService;
            this.studentRegisterService = studentRegisterService;
        }

        #endregion Ctors and Members

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<TeacherSubjectByDatesPrintModelData>>> GetTeacherClassesByStudentGroupId(SimpleRequestBase request)
        {
            return await studentGroupService.GetTeacherClasses(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<ThemesByWeekPrintModel>>> GetThemesByWeeks(SimpleRequestBase request)
        {
            return await studentGroupService.GetThemesByWeeks(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<ThemesClassesPrintModel>>> GetThemesClasses(SimpleRequestBase request)
        {
            return await studentGroupService.GetThemesClasses(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<StudentEducationProgramsPrintModel>> GetStudentsEducationPrograms(SimpleRequestBase request)
        {
            return await studentService.GetStudentsEducationPrograms(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<StudentRegisterPrintDataAggregatedDto>>> GetPrintDataByBookAndRegisterRange(StudentRegisterPrintForRangeRequest request)
        {
            return await studentRegisterService.GetPrintDataForBookAndEntriesRange(request);
        }
    }
}
