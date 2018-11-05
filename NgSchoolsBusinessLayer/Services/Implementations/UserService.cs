using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Repository.Contracts;
using System;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class UserService : IUserService
    {
        #region Ctors and Members

        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        #endregion Ctors and Members

        public ActionResponse<UserDto> GetUserById(Guid Id)
        {
            try
            {
                return ActionResponse<UserDto>.ReturnSuccess(userRepository.GetUserById(Id));
            }
            catch (Exception ex)
            {
                return ActionResponse<UserDto>.ReturnError(ex.Message);
            }
        }

        public ActionResponse<UserDto> GetUserByName(string name)
        {
            try
            {
                return ActionResponse<UserDto>.ReturnSuccess(userRepository.GetUserByName(name));
            }
            catch (Exception ex)
            {
                return ActionResponse<UserDto>.ReturnError(ex.Message);
            }
        }
    }
}
