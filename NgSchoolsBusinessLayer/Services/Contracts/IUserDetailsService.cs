using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.ViewModels;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IUserDetailsService
    {
        Task<ActionResponse<UserViewModel>> CreateUserDetails(UserViewModel userDetails);
        Task<ActionResponse<UserDetailsDto>> UpdateUserDetails(UserDetailsDto userDetails);
        Task<ActionResponse<UserViewModel>> UpdateUserDetails(UserViewModel userDetails);
    }
}