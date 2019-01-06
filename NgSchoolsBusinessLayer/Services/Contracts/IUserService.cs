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
        Task<ActionResponse<List<UserDto>>> GetAllUsers();
        Task<ActionResponse<List<UserViewModel>>> GetAllUsersFE();
        Task<ActionResponse<List<UserDto>>> GetAllUsersForCache();
        Task<ActionResponse<UserDto>> GetUserByEmail(string name);
        Task<ActionResponse<UserDto>> GetById(Guid userId);
        Task<ActionResponse<UserViewModel>> GetUserViewModelById(Guid value);
        Task<ActionResponse<PagedResult<UserViewModel>>> GetAllUsersPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<PagedResult<TeacherViewModel>>> GetAllTeachersPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<UserViewModel>> Create(UserViewModel request);
        Task<ActionResponse<TeacherViewModel>> CreateTeacher(TeacherViewModel request);
        Task<ActionResponse<object>> Delete(UserGetRequest request);
        Task<ActionResponse<TeacherViewModel>> UpdateTeacher(TeacherViewModel request);
        Task<ActionResponse<UserViewModel>> Update(UserViewModel request);

        Task<ActionResponse<List<Claim>>> GetUserClaims(UserDto user);
        Task<ActionResponse<List<RoleDto>>> GetAllRoles();
        Task<ActionResponse<UserViewModel>> AddRoles(UserViewModel user);
        Task<ActionResponse<UserViewModel>> AddToDefaultRole(UserViewModel user);
        Task<ActionResponse<UserViewModel>> RemoveRoles(UserViewModel user);
        Task<ActionResponse<UserViewModel>> ModifyUserRoles(UserViewModel user);
    }
}