using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IUserService
    {
        Task<ActionResponse<UserDto>> GetUserById(Guid Id);
        Task<ActionResponse<UserDto>> GetUserByEmail(string name);
        Task<ActionResponse<List<Claim>>> GetUserClaims(UserDto user);
        Task<ActionResponse<List<UserDto>>> GetAllUsers();
        Task<ActionResponse<PagedResult<UserDto>>> GetAllUsersPaged(BasePagedRequest pagedRequest);
    }
}