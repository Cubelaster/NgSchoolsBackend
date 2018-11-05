using System;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IUserService
    {
        ActionResponse<UserDto> GetUserById(Guid Id);
        ActionResponse<UserDto> GetUserByName(string name);
    }
}