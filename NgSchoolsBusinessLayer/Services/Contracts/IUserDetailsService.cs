using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IUserDetailsService
    {
        Task<ActionResponse<UserDetailsDto>> GetUserDetails(Guid userId);
        Task<ActionResponse<UserViewModel>> CreateUserDetails(UserViewModel userDetails);
        Task<ActionResponse<UserDetailsDto>> UpdateUserDetails(UserDetailsDto userDetails);
        Task<ActionResponse<UserViewModel>> UpdateUserDetails(UserViewModel userDetails);
    }
}