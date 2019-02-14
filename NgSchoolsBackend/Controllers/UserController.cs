using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<UserViewModel>>> GetAll()
        {
            return await userService.GetAllUsersFE();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<UserViewModel>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await userService.GetAllUsersPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<PagedResult<TeacherViewModel>>> GetAllTeachersPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await userService.GetAllTeachersPaged(pagedRequest);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<UserDto>> GetById([FromBody] UserGetRequest request)
        {
            return await userService.GetById(request.Id.Value);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<UserViewModel>> GetUserById([FromBody] UserGetRequest request)
        {
            return await userService.GetUserViewModelById(request.Id.Value);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<TeacherViewModel>> GetTeacherById([FromBody] UserGetRequest request)
        {
            return await userService.GetTeacherViewModelById(request.Id.Value);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<UserViewModel>> Create([FromBody] UserViewModel request)
        {
            return await userService.Create(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<TeacherViewModel>> CreateTeacher([FromBody] TeacherViewModel request)
        {
            return await userService.CreateTeacher(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<UserViewModel>> Update([FromBody] UserViewModel request)
        {
            return await userService.Update(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<TeacherViewModel>> UpdateTeacher([FromBody] TeacherViewModel request)
        {
            return await userService.UpdateTeacher(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<object>> Delete([FromBody] UserGetRequest request)
        {
            return await userService.Delete(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<object>> DeleteTeacher([FromBody] UserGetRequest request)
        {
            return await userService.DeleteTeacher(request);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<List<RoleDto>>> GetAllRoles()
        {
            return await userService.GetAllRoles();
        }
    }
}