using Microsoft.AspNetCore.Identity;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class AuthService : IAuthService
    {
        #region Ctors and Members

        private readonly UserManager<User> userManager;

        public AuthService(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<LoginRequest>> Login(LoginRequest loginRequest)
        {
            // get the user to verifty
            var userToVerify = await userManager.FindByNameAsync(loginRequest.UserName);

            // check the credentials
            var loggedIn = await userManager.CheckPasswordAsync(userToVerify, loginRequest.Password);

            return await Task.FromResult(ActionResponse<LoginRequest>.ReturnSuccess(loginRequest));
            //await _userManager.CreateAsync(appUser, user.GetValue<string>("Password"));
        }
    }
}
