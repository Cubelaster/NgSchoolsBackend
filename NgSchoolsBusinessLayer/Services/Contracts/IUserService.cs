using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IUserService
    {
        Task<ActionResponse<UserDto>> GetUserById(Guid Id);
        Task<ActionResponse<UserDto>> GetUserByName(string name);
        Task<ActionResponse<List<Claim>>> GetUserClaims(UserDto user);
    }
}