using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsBusinessLayer.Utilities.Attributes;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class UserService : IUserService
    {
        #region Ctors and Members

        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IUserDetailsService userDetailsService;

        private readonly IJwtFactory jwtFactory;
        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        private readonly string includeProperties = "Roles.Role,UserDetails.Avatar,UserDetails.Signature,UserDetails.City,UserDetails.Region,UserDetails.Country,UserDetails.TeacherFiles.File";

        public UserService(UserManager<User> userManager, IUserDetailsService userDetailsService,
            IJwtFactory jwtFactory, RoleManager<Role> roleManager, IMapper mapper, IConfiguration configuration,
            ICacheService cacheService, ILoggerService loggerService, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userDetailsService = userDetailsService;
            this.jwtFactory = jwtFactory;
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
        }

        #endregion Ctors and Members

        #region Readers

        public async Task<ActionResponse<List<UserDto>>> GetAllUsers()
        {
            try
            {
                var allUsers = unitOfWork.GetGenericRepository<User>()
                    .GetAll(includeProperties: includeProperties);
                return await ActionResponse<List<UserDto>>
                    .ReturnSuccess(mapper.Map<List<User>, List<UserDto>>(allUsers));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<UserDto>>.ReturnError("Greška prilikom dohvata podataka za korisnike.");
            }
        }

        public async Task<ActionResponse<List<UserViewModel>>> GetAllUsersFE()
        {
            try
            {
                List<UserDto> users = new List<UserDto>();
                var cachedUsersResponse = await cacheService.GetFromCache<List<UserDto>>();
                if (!cachedUsersResponse.IsSuccessAndHasData(out users))
                {
                    users = (await GetAllUsers()).GetData();
                }
                var feUsers = mapper.Map<List<UserDto>, List<UserViewModel>>(users);
                return await ActionResponse<List<UserViewModel>>.ReturnSuccess(feUsers);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<UserViewModel>>.ReturnError("Greška prilikom dohvata podataka za korisnike.");
            }
        }

        [CacheRefreshSource(typeof(UserDto))]
        public async Task<ActionResponse<List<UserDto>>> GetAllUsersForCache()
        {
            try
            {
                var allUsers = unitOfWork.GetGenericRepository<User>()
                    .GetAll(includeProperties: includeProperties);
                return await ActionResponse<List<UserDto>>.ReturnSuccess(
                    mapper.Map<List<User>, List<UserDto>>(allUsers));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<UserDto>>.ReturnError("Greška prilikom dohvata podataka za korisnika.");
            }
        }

        public async Task<ActionResponse<UserDto>> GetUserByEmail(string email)
        {
            try
            {
                var user = unitOfWork.GetGenericRepository<User>()
                    .FindBy(u => u.Email == email, includeProperties);
                return await ActionResponse<UserDto>.ReturnSuccess(mapper.Map<User, UserDto>(user));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<UserDto>.ReturnError("Greška prilikom dohvata korisnika po email adresi.");
            }
        }

        public async Task<ActionResponse<UserDto>> GetById(Guid userId)
        {
            try
            {
                var user = unitOfWork.GetGenericRepository<User>()
                    .FindBy(u => u.Id == userId, includeProperties: includeProperties);
                return await ActionResponse<UserDto>.ReturnSuccess(mapper.Map<User, UserDto>(user));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, userId);
                return await ActionResponse<UserDto>.ReturnError("Greška prilikom dohvata podataka korisnika.");
            }
        }

        public async Task<ActionResponse<UserViewModel>> GetUserViewModelById(Guid userId)
        {
            try
            {
                if ((await GetById(userId)).IsNotSuccess(out ActionResponse<UserDto> response, out UserDto user))
                {
                    return await ActionResponse<UserViewModel>.ReturnError(response.Message);
                }
                return await ActionResponse<UserViewModel>.ReturnSuccess(mapper.Map<UserDto, UserViewModel>(user));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, userId);
                return await ActionResponse<UserViewModel>.ReturnError("Greška prilikom dohvata podataka o korisniku.");
            }
        }

        public async Task<ActionResponse<TeacherViewModel>> GetTeacherViewModelById(Guid userId)
        {
            try
            {
                if ((await GetById(userId)).IsNotSuccess(out ActionResponse<UserDto> response, out UserDto user))
                {
                    return await ActionResponse<TeacherViewModel>.ReturnError(response.Message);
                }
                return await ActionResponse<TeacherViewModel>.ReturnSuccess(mapper.Map<UserDto, TeacherViewModel>(user));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, userId);
                return await ActionResponse<TeacherViewModel>.ReturnError("Greška prilikom dohvata podataka o nastavniku.");
            }
        }

        public async Task<ActionResponse<PagedResult<UserViewModel>>> GetAllUsersPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<UserDto> users = new List<UserDto>();
                var cachedUsersResponse = await cacheService.GetFromCache<List<UserDto>>();
                if (!cachedUsersResponse.IsSuccessAndHasData(out users))
                {
                    users = (await GetAllUsers()).GetData();
                }

                var pagedResult = await mapper.Map<List<UserDto>, List<UserViewModel>>(users)
                    .AsQueryable()
                    .GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<UserViewModel>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<UserViewModel>>.ReturnError("Greška prilikom dohvata podataka korisnika.");
            }
        }

        public async Task<ActionResponse<PagedResult<TeacherViewModel>>> GetAllTeachersPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                List<UserDto> users = new List<UserDto>();
                var cachedUsersResponse = await cacheService.GetFromCache<List<UserDto>>();
                if (!cachedUsersResponse.IsSuccessAndHasData(out users))
                {
                    users = (await GetAllUsers()).GetData();
                }

                var teacherUsers = mapper.Map<List<UserDto>, List<TeacherViewModel>>(
                    users
                    .Where(u => u.UserRoles.Any(ur => ur.Name == "Nastavnik"))
                    .ToList());

                var pagedResult = await teacherUsers
                    .AsQueryable().GetPaged(pagedRequest);
                return await ActionResponse<PagedResult<TeacherViewModel>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<TeacherViewModel>>.ReturnError("Greška prilikom dohvata straničnih podataka nastavnika.");
            }
        }

        #endregion Readers

        #region Writers

        public async Task<ActionResponse<UserViewModel>> Create(UserViewModel request)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    request.DateCreated = DateTime.UtcNow;
                    var user = mapper.Map<UserViewModel, User>(request);
                    var result = await userManager.CreateAsync(user, request.Password);
                    if (!result.Succeeded)
                    {
                        return await ActionResponse<UserViewModel>.ReturnError("Failed to create new user.");
                    }

                    request.Id = user.Id;

                    if ((await AddToDefaultRole(request)).IsNotSuccess(out ActionResponse<UserViewModel> actionResponse, out request))
                    {
                        return await ActionResponse<UserViewModel>.ReturnError("Failed to edit user's roles.", request);
                    }

                    if ((await ModifyUserRoles(request)).IsNotSuccess(out actionResponse, out request))
                    {
                        return await ActionResponse<UserViewModel>.ReturnError("Failed to edit user's roles.");
                    }

                    if ((await userDetailsService.CreateUserDetails(request)).IsNotSuccess(out actionResponse, out request))
                    {
                        return actionResponse;
                    }
                    scope.Complete();
                }

                return await ActionResponse<UserViewModel>.ReturnSuccess(request);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, request);
                return await ActionResponse<UserViewModel>.ReturnError("Dogodila se greška, molimo kontaktirajte svog administratora.");
            }
            finally
            {
                await cacheService.RefreshCache<List<UserDto>>();
            }
        }

        public async Task<ActionResponse<TeacherViewModel>> CreateTeacher(TeacherViewModel request)
        {
            try
            {
                if (!request.UserId.HasValue)
                {
                    return await ActionResponse<TeacherViewModel>.ReturnError("Molimo, povežite nastavnika na postojećeg korisnika.");
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if ((await UpdateTeacher(request))
                        .IsNotSuccess(out ActionResponse<TeacherViewModel> actionResponse, out request))
                    {
                        return actionResponse;
                    }

                    var rolesRequest = new UserViewModel
                    {
                        Id = request.UserId,
                        RolesNamed = new List<string> { "Nastavnik" }
                    };

                    if ((await AddRoles(mapper.Map<UserViewModel>(rolesRequest)))
                        .IsNotSuccess(out ActionResponse<UserViewModel> rolesResponse, out UserViewModel viewModel))
                    {
                        return await ActionResponse<TeacherViewModel>
                            .ReturnError("Dogodila se greška prilikom dodavanja role nastavnika za korisnika.");
                    }
                    scope.Complete();
                }
                return await ActionResponse<TeacherViewModel>.ReturnSuccess(request, "Korisnik uspješno dodan u rolu nastavnika i njegovi detelji ažurirani.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, request);
                return await ActionResponse<TeacherViewModel>.ReturnError("Greška prilikom kreacije novog nastavnika.");
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
                return await ActionResponse<object>.ReturnSuccess(null, "Success!");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, request);
                return await ActionResponse<object>.ReturnError("Greška prilikom brisanja.");
            }
            finally
            {
                await cacheService.RefreshCache<List<UserDto>>();
            }
        }

        public async Task<ActionResponse<object>> DeleteTeacher(UserGetRequest request)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var rolesRequest = new UserViewModel
                    {
                        Id = request.Id,
                        RolesNamed = new List<string> { "Nastavnik" }
                    };

                    if ((await RemoveRoles(mapper.Map<UserViewModel>(rolesRequest)))
                        .IsNotSuccess(out ActionResponse<UserViewModel> rolesResponse, out UserViewModel viewModel))
                    {
                        return await ActionResponse<object>
                            .ReturnError("Dogodila se greška prilikom brisanja role nastavnika za korisnika. Molimo pokušajte ponovno.");
                    }
                    scope.Complete();
                }
                return await ActionResponse<object>.ReturnSuccess(null, "Nastavnik uspješno izbrisan!");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, request);
                return await ActionResponse<object>.ReturnError("Greška prilikom brisanja nastavnika.");
            }
            finally
            {
                await cacheService.RefreshCache<List<UserDto>>();
            }
        }

        public async Task<ActionResponse<UserViewModel>> Update(UserViewModel request)
        {
            try
            {
                if (!request.Id.HasValue || !request.UserDetailsId.HasValue)
                {
                    return await ActionResponse<UserViewModel>.ReturnError("Incorect primary key so unable to update.");
                }

                if ((await userDetailsService.UpdateUserDetails(request))
                    .IsNotSuccess(out ActionResponse<UserViewModel> response, out request))
                {
                    return await ActionResponse<UserViewModel>.ReturnError(response.Message, request);
                }

                unitOfWork.Save();

                if ((await ModifyUserRoles(request)).IsNotSuccess(out response))
                {
                    return response;
                }

                return await ActionResponse<UserViewModel>.ReturnSuccess(request, "User updated successfully.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, request);
                return await ActionResponse<UserViewModel>.ReturnError("Greška prilikom ažuriranja podataka korisnika.");
            }
            finally
            {
                await cacheService.RefreshCache<List<UserDto>>();
            }
        }

        public async Task<ActionResponse<TeacherViewModel>> UpdateTeacher(TeacherViewModel request)
        {
            try
            {
                var response = ActionResponse<TeacherViewModel>.ReturnSuccess(request);

                if (!request.UserId.HasValue)
                {
                    return (await response).AppendErrorMessage("Nepotpuni podatci. Molimo popunite sva obavezna polja.");
                }

                if ((await userDetailsService.GetUserDetails(request.UserId.Value))
                    .IsNotSuccess(out ActionResponse<UserDetailsDto> actionResponse, out UserDetailsDto userDetails))
                {
                    return (await response).AppendErrorMessage(actionResponse.Message);
                }

                mapper.Map(request, userDetails);

                if ((await userDetailsService.UpdateUserDetails(userDetails)).IsNotSuccess(out actionResponse, out userDetails))
                {
                    return (await response).AppendErrorMessage(actionResponse.Message);
                }

                return await ActionResponse<TeacherViewModel>.ReturnSuccess(mapper.Map(userDetails, request), "Nastavnik uspješno ažuriran.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, request);
                return await ActionResponse<TeacherViewModel>
                    .ReturnError("Dogodila se greška prilikom ažuriranja podataka o nastavniku. Molimo pokušajte ponovno.");
            }
            finally
            {
                await cacheService.RefreshCache<List<UserDto>>();
            }
        }

        #endregion Writers

        #region Roles 

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
                return await ActionResponse<List<RoleDto>>.ReturnError("Greška prilikom dohvata prava korisnika.");
            }
        }

        public async Task<ActionResponse<UserViewModel>> AddRoles(UserViewModel user)
        {
            try
            {
                var entity = await userManager.FindByIdAsync(user.Id.Value.ToString());
                var result = await userManager.AddToRolesAsync(entity, user.RolesNamed);
                if (!result.Succeeded && !result.Errors.Any(e => e.Code == "UserAlreadyInRole"))
                {
                    return await ActionResponse<UserViewModel>.ReturnError("Greška prilikom dodavanja rola korisniku.");
                }
                return await ActionResponse<UserViewModel>.ReturnSuccess(user);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, user);
                return await ActionResponse<UserViewModel>.ReturnError("Greška prilikom dodavanja rola korisniku.");
            }
        }

        public async Task<ActionResponse<UserViewModel>> AddToDefaultRole(UserViewModel user)
        {
            try
            {
                var defaultRole = await roleManager
                    .FindByNameAsync(configuration.GetValue<string>("DefaultUserRole"));
                user.Roles.Add(defaultRole.Id);
                return await ActionResponse<UserViewModel>.ReturnSuccess(user);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, user);
                return await ActionResponse<UserViewModel>.ReturnError("Greška prilikom dodavanja korisnika u početnu rolu.");
            }
        }

        public async Task<ActionResponse<UserViewModel>> RemoveRoles(UserViewModel user)
        {
            try
            {
                var entity = await userManager.FindByIdAsync(user.Id.Value.ToString());
                var result = await userManager.RemoveFromRolesAsync(entity, user.RolesNamed);
                if (!result.Succeeded)
                {
                    return await ActionResponse<UserViewModel>.ReturnError("Failed to remove from roles");
                }
                return await ActionResponse<UserViewModel>.ReturnSuccess(user);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, user);
                return await ActionResponse<UserViewModel>.ReturnError("Greška prilikom micanja rola s korisnika.");
            }
        }

        public async Task<ActionResponse<UserViewModel>> ModifyUserRoles(UserViewModel user)
        {
            try
            {
                var entity = mapper.Map<UserViewModel, User>(user);
                var currentUserRoles = await userManager.GetRolesAsync(entity);

                List<string> updateRoles = new List<string>();
                foreach (var roleId in user.Roles)
                {
                    var role = await roleManager.FindByIdAsync(roleId.ToString());
                    updateRoles.Add(role.Name);
                }

                var rolesToRemove = currentUserRoles.Where(cur => !updateRoles.Contains(cur)).ToList();
                var rolesToAdd = updateRoles.Where(ur => !currentUserRoles.Contains(ur)).ToList();

                user.RolesNamed = rolesToRemove;
                var actionResponse = await RemoveRoles(user);
                if (!actionResponse.IsSuccess(out user))
                {
                    return actionResponse;
                }

                user.RolesNamed = rolesToAdd;
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
                return await ActionResponse<UserViewModel>.ReturnError("Greška prilikom ažuriranja rola korisnika.");
            }
        }

        #endregion Roles
    }
}
