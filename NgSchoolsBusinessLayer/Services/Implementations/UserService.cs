using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class UserService : IUserService
    {
        #region Ctors and Members

        private readonly IUserRepository userRepository;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly IJwtFactory jwtFactory;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILoggerService loggerService;

        public UserService(IUserRepository userRepository, UserManager<User> userManager,
            IJwtFactory jwtFactory, RoleManager<IdentityRole<Guid>> roleManager, IMapper mapper,
            ICacheService cacheService, ILoggerService loggerService)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.jwtFactory = jwtFactory;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.loggerService = loggerService;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<UserDto>> GetUserById(Guid Id)
        {
            try
            {
                var bla = await cacheService.GetFromCache<List<UserDto>>();
                return await ActionResponse<UserDto>.ReturnSuccess(userRepository.GetUserById(Id));
            }
            catch (Exception ex)
            {
                return await ActionResponse<UserDto>.ReturnError(ex.Message);
            }
        }

        public async Task<ActionResponse<UserDto>> GetUserByName(string name)
        {
            try
            {
                return await ActionResponse<UserDto>
                    .ReturnSuccess(mapper.Map<User, UserDto>(userRepository.GetUserByName(name)));
            }
            catch (Exception ex)
            {
                return await ActionResponse<UserDto>.ReturnError(ex.Message);
            }
        }

        public async Task<ActionResponse<List<Claim>>> GetUserClaims(UserDto user)
        {
            try
            {
                var userToVerify = await userManager.FindByNameAsync(user.UserName);
                var allClaims = await jwtFactory.GetJwtClaims(user);

                IdentityOptions identityOptions = new IdentityOptions();
                allClaims.AddRange(new List<Claim>() {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                });

                // Get additional claims linked to user, without role and get roles as well
                var userClaims = await userManager.GetClaimsAsync(userToVerify);
                allClaims.AddRange(userClaims);

                var userRoles = await userManager.GetRolesAsync(userToVerify);
                foreach (var userRole in userRoles)
                {
                    allClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    var role = await roleManager.FindByNameAsync(userRole);
                    if (role != null)
                    {
                        var roleClaims = await roleManager.GetClaimsAsync(role);
                        foreach (Claim roleClaim in roleClaims)
                        {
                            allClaims.Add(roleClaim);
                        }
                    }
                }

                return await ActionResponse<List<Claim>>.ReturnSuccess(allClaims);
            }
            catch (Exception ex)
            {
                return await ActionResponse<List<Claim>>.ReturnError(ex.Message);
            }
        }

        [CacheRefreshSource(typeof(UserDto))]
        public async Task<ActionResponse<List<UserDto>>> GetAllUsersForCache()
        {
            try
            {
                return await ActionResponse<List<UserDto>>.ReturnSuccess(
                    mapper.Map<List<User>, List<UserDto>>(userRepository.GetAllUsers()));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<UserDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }
    }
}
