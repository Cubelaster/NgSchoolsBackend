using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.ViewModels;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IUserService
    {
        Task<ActionResponse<UserDto>> GetUserByEmail(string name);
        Task<ActionResponse<List<Claim>>> GetUserClaims(UserDto user);
        Task<ActionResponse<List<UserDto>>> GetAllUsers();
        Task<ActionResponse<PagedResult<UserViewModel>>> GetAllUsersPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<TeacherViewModel>>> GetAllTeachersPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<UserDto>> GetById(Guid userId);
        Task<ActionResponse<UserDto>> Create(UserDto request);
        Task<ActionResponse<UserDto>> Update(UserDto request);
        Task<ActionResponse<object>> Delete(UserGetRequest request);
        Task<ActionResponse<List<RoleDto>>> GetAllRoles();
        Task<ActionResponse<TeacherViewModel>> UpdateTeacher(TeacherViewModel request);
        Task<ActionResponse<UserViewModel>> GetViewModelById(Guid value);
    }
}