using Microsoft.AspNetCore.Identity;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Extensions;
using System.Collections.Generic;
using System.Security.Claims;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using System.Linq;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class AuthService : IAuthService
    {
        #region Ctors and Members

        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IJwtFactory jwtFactory;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager,
            IUserService userService, IJwtFactory jwtFactory, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.mapper = mapper;
            this.jwtFactory = jwtFactory;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<object>> Login(LoginRequest loginRequest)
        {
            var result = await signInManager.PasswordSignInAsync(loginRequest.UserName, loginRequest.Password, true, true);
            if (result.Succeeded)
            {
                var userToVerify = await userManager.FindByNameAsync(loginRequest.UserName);
                //var user = mapper.Map<UserDto>(userToVerify);
                var user = new UserDto
                {
                    Id = userToVerify.Id,
                    UserName = userToVerify.UserName
                };
                user.Claims = (await userService.GetUserClaims(user)).GetData();
                var jwtToken = await jwtFactory.GenerateSecurityToken(user, loginRequest.RememberMe);
                return await ActionResponse<object>.ReturnSuccess(new
                {
                    id = user.Claims.Single(c => c.Type == "Id").Value,
                    auth_token = jwtToken
                });
            }
            return await ActionResponse<object>.ReturnError("Wrong username or password");
        }
    }
}
