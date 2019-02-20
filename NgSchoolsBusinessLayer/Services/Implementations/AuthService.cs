﻿using Microsoft.AspNetCore.Identity;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using System.Threading.Tasks;
using AutoMapper;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Models.Responses;
using System;

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
        private readonly ICacheService cacheService;
        private readonly ILoggerService loggerService;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager,
            IUserService userService, IJwtFactory jwtFactory, IMapper mapper,
            ICacheService cacheService, ILoggerService loggerService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.mapper = mapper;
            this.jwtFactory = jwtFactory;
            this.cacheService = cacheService;
            this.loggerService = loggerService;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {
            try
            {
                var userToLogin = await userManager.FindByEmailAsync(loginRequest.Email);
                if (userToLogin == null)
                {
                    return await ActionResponse<LoginResponse>.ReturnError("Ne postoji korisnik registriran s tom email adresom.");
                }

                var result = await signInManager.PasswordSignInAsync(userToLogin, loginRequest.Password, true, true);
                if (result.Succeeded)
                {
                    var user = mapper.Map<UserDto>(userToLogin);
                    user.Claims = (await userService.GetUserClaims(user)).GetData();
                    var jwtToken = await jwtFactory.GenerateSecurityToken(user, loginRequest.RememberMe);

                    return await ActionResponse<LoginResponse>.ReturnSuccess(new LoginResponse
                    {
                        UserId = user.Id.Value,
                        JwtToken = jwtToken.Token,
                        ValidUntil = jwtToken.ValidUntil
                    }, "Successfully logged in!");
                }
                return await ActionResponse<LoginResponse>.ReturnError("Pogrešno korisničko ime ili lozinka.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, loginRequest);
                return await ActionResponse<LoginResponse>.ReturnError("Dogodila se kritična greška. Molimo kontaktirajte administratore.");
            }
        }
    }
}
