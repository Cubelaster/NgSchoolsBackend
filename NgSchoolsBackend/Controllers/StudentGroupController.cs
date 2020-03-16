using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Dto.StudentGroup;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.Requests.StudentGroup;
using NgSchoolsBusinessLayer.Models.ViewModels.StudentGroup;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<StudentGroupDto>>> GetAll()
        {
            return await studentGroupService.GetAll();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<StudentGroupGridViewModel>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await studentGroupService.GetAllPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDto>> GetById(SimpleRequestBase request)
        {
            return await studentGroupService.GetById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDetailsViewModel>> Details(SimpleRequestBase request)
        {
            return await studentGroupService.Details(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDto>> Insert([FromBody]StudentGroupDto request)
        {
            return await studentGroupService.Insert(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDto>> Update([FromBody]StudentGroupDto request)
        {
            return await studentGroupService.Update(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDetailsViewModel>> UpdateDetails([FromBody]StudentGroupUpdateRequest request)
        {
            return await studentGroupService.UpdateDetails(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDto>> ModifyStudentsInGroup([FromBody]StudentGroupDto request)
        {
            return await studentGroupService.ModifyStudentsInGroup(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<StudentGroupDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await studentGroupService.Delete(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<StudentGroupClassAttendanceDto>>> GetClassAttendancesByGroupId([FromBody] SimpleRequestBase request)
        {
            return await studentGroupService.GetClassAttendancesByGroupId(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            return await studentGroupService.GetTotalNumber();
        }

        #region CombinedGroups

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<List<CombinedGroupDto>>> GetAllCombined()
        {
            return await studentGroupService.GetAllCombined();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<CombinedGroupDto>>> GetAllCombinedPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await studentGroupService.GetAllCombinedPaged(pagedRequest);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CombinedGroupDto>> GetCombinedById(SimpleRequestBase request)
        {
            return await studentGroupService.GetCombinedById(request.Id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CombinedGroupDto>> InsertCombined([FromBody] CombinedGroupDto request)
        {
            return await studentGroupService.InsertCombined(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CombinedGroupDto>> UpdateCombined([FromBody] CombinedGroupDto request)
        {
            return await studentGroupService.UpdateCombined(request);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResponse<CombinedGroupDto>> DeleteCombined([FromBody] SimpleRequestBase request)
        {
            return await studentGroupService.DeleteCombined(request.Id);
        }

        #endregion CombinedGroups
    }
}
