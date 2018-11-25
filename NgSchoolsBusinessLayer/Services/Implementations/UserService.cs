using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.Contracts;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class UserService : IUserService
    {
        #region Ctors and Members

        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IJwtFactory jwtFactory;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public UserService(UserManager<User> userManager, IJwtFactory jwtFactory,
            RoleManager<Role> roleManager, IMapper mapper, IConfiguration configuration,
            ICacheService cacheService, ILoggerService loggerService, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.jwtFactory = jwtFactory;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<UserDto>> GetUserByEmail(string email)
        {
            try
            {
                var user = unitOfWork.GetGenericRepository<User>().FindBy(u => u.Email == email, includeProperties: "Roles.Role, UserDetails");
                return await ActionResponse<UserDto>.ReturnSuccess(mapper.Map<User, UserDto>(user));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
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
                var allUsers = unitOfWork.GetGenericRepository<User>().GetAll(includeProperties: "Roles.Role, UserDetails");
                return await ActionResponse<List<UserDto>>.ReturnSuccess(
                    mapper.Map<List<User>, List<UserDto>>(allUsers));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<UserDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<List<UserDto>>> GetAllUsers()
        {
            try
            {
                var allUsers = unitOfWork.GetGenericRepository<User>().GetAll(includeProperties: "Roles.Role");
                return await ActionResponse<List<UserDto>>
                    .ReturnSuccess(mapper.Map<List<User>, List<UserDto>>(allUsers));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<UserDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<PagedResult<UserDto>>> GetAllUsersPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<UserDto> users = new List<UserDto>();
                var cachedUsersResponse = await cacheService.GetFromCache<List<UserDto>>();
                if (!cachedUsersResponse.IsSuccessAndHasData(out users))
                {
                    users = (await GetAllUsers()).GetData();
                }

                var pagedResult = await users.AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<UserDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<UserDto>>
                    .ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<UserDto>> GetById(Guid userId)
        {
            try
            {
                var user = unitOfWork.GetGenericRepository<User>().FindBy(u => u.Id == userId, includeProperties: "Roles.Role, UserDetails");
                return await ActionResponse<UserDto>.ReturnSuccess(mapper.Map<User, UserDto>(user));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, userId);
                return await ActionResponse<UserDto>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<UserDto>> Create(UserDto request)
        {
            try
            {
                var user = mapper.Map<UserDto, User>(request);
                var result = await userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return await ActionResponse<UserDto>.ReturnError("Failed to create new user.");
                }

                request.Id = user.Id;

                var actionResponse = await AddToDefaultRole(request);
                if (!actionResponse.IsSuccessAndHasData(out request))
                {
                    return await ActionResponse<UserDto>.ReturnError("Failed to edit user's roles.");
                };

                actionResponse = await ModifyUserRoles(request);
                if (!actionResponse.IsSuccessAndHasData(out request))
                {
                    return await ActionResponse<UserDto>.ReturnError("Failed to edit user's roles.");
                };

                return await ActionResponse<UserDto>.ReturnSuccess(request);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, request);
                return await ActionResponse<UserDto>.ReturnError("Some sort of fuckup. Try again.");
            }
            finally
            {
                await cacheService.RefreshCache<List<UserDto>>();
            }
        }

        public async Task<ActionResponse<UserDto>> Update(UserDto request)
        {
            try
            {
                if (!request.Id.HasValue)
                {
                    return await ActionResponse<UserDto>.ReturnError("Incorect primary key so unable to update.");
                }

                var user = mapper.Map<UserDto, User>(request);
                var userDetails = user.UserDetails;

                userDetails = unitOfWork.GetCustomRepository<IUserRepository>()
                    .UpdateUserDetails(user.UserDetails);

                user = unitOfWork.GetCustomRepository<IUserRepository>().Update(user);
                unitOfWork.Save();

                var actionResponse = await ModifyUserRoles(request);
                if (!actionResponse.IsSuccessAndHasData(out request))
                {
                    return await ActionResponse<UserDto>.ReturnError("Failed to edit user's roles.");
                };

                return await ActionResponse<UserDto>.ReturnSuccess(request);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, request);
                return await ActionResponse<UserDto>.ReturnError("Some sort of fuckup. Try again.");
            }
            finally
            {
                await cacheService.RefreshCache<List<UserDto>>();
            }
        }

        public async Task<ActionResponse<object>> Delete(UserGetRequest request)
        {
            try
            {
                if (!request.Id.HasValue)
                {
                    return await ActionResponse<object>.ReturnError("Incorect primary key so unable to update.");
                }

                unitOfWork.GetGenericRepository<User>().Delete(request.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<object>.ReturnSuccess("Success!");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, request);
                return await ActionResponse<object>.ReturnError("Some sort of fuckup. Try again.");
            }
            finally
            {
                await cacheService.RefreshCache<List<UserDto>>();
            }
        }

        #region Roles 

        public async Task<ActionResponse<List<RoleDto>>> GetAllRoles()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Role>().GetAll();
                return await ActionResponse<List<RoleDto>>.ReturnSuccess(mapper.Map<List<Role>, List<RoleDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<RoleDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        private async Task<ActionResponse<UserDto>> AddRoles(UserDto user)
        {
            try
            {
                var entity = await userManager.FindByIdAsync(user.Id.Value.ToString());
                var result = await userManager.AddToRolesAsync(entity, user.Roles);
                if (!result.Succeeded)
                {
                    return await ActionResponse<UserDto>.ReturnError("Failed to add user to roles");
                }
                return await ActionResponse<UserDto>.ReturnSuccess(user);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, user);
                return await ActionResponse<UserDto>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        private async Task<ActionResponse<UserDto>> AddToDefaultRole(UserDto user)
        {
            try
            {
                var defaultRole = await roleManager
                    .FindByNameAsync(configuration.GetValue<string>("DefaultUserRole"));
                user.Roles.Add(defaultRole.Id.ToString());
                return await ActionResponse<UserDto>.ReturnSuccess(user);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, user);
                return await ActionResponse<UserDto>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        private async Task<ActionResponse<UserDto>> RemoveRoles(UserDto user)
        {
            try
            {
                var entity = await userManager.FindByIdAsync(user.Id.Value.ToString());
                var result = await userManager.RemoveFromRolesAsync(entity, user.Roles);
                if (!result.Succeeded)
                {
                    return await ActionResponse<UserDto>.ReturnError("Failed to remove from roles");
                }
                return await ActionResponse<UserDto>.ReturnSuccess(user);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, user);
                return await ActionResponse<UserDto>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        private async Task<ActionResponse<UserDto>> ModifyUserRoles(UserDto user)
        {
            try
            {
                var entity = mapper.Map<UserDto, User>(user);
                var currentUserRoles = await userManager.GetRolesAsync(entity);

                List<string> updateRoles = new List<string>();
                foreach (var roleId in user.Roles)
                {
                    var role = await roleManager.FindByIdAsync(roleId);
                    updateRoles.Add(role.Name);
                }

                var rolesToRemove = currentUserRoles.Where(cur => !updateRoles.Contains(cur)).ToList();
                var rolesToAdd = updateRoles.Where(ur => !currentUserRoles.Contains(ur)).ToList();

                user.Roles = rolesToRemove;
                var actionResponse = await RemoveRoles(user);
                if (!actionResponse.IsSuccess(out user))
                {
                    return actionResponse;
                }

                user.Roles = rolesToAdd;
                actionResponse = await AddRoles(user);
                if (!actionResponse.IsSuccess(out user))
                {
                    return actionResponse;
                }

                return actionResponse;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, user);
                return await ActionResponse<UserDto>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        #endregion Roles
    }
}
