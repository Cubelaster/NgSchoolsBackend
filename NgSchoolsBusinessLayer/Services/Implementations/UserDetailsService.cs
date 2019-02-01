using AutoMapper;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class UserDetailsService : IUserDetailsService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ICacheService cacheService;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public UserDetailsService(IMapper mapper, ICacheService cacheService,
            ILoggerService loggerService, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<UserViewModel>> CreateUserDetails(UserViewModel userDetails)
        {
            try
            {
                var userDetailsEntity = new UserDetails
                {
                    FirstName = userDetails.FirstName,
                    LastName = userDetails.LastName,
                    Mobile = userDetails.Mobile,
                    Mobile2 = userDetails.Mobile2,
                    Phone = userDetails.Phone,
                    Title = userDetails.Title,
                    UserId = userDetails.Id,
                    DateCreated = DateTime.UtcNow
                };

                if (userDetails.Avatar != null)
                {
                    userDetailsEntity.AvatarId = userDetails.Avatar.Id.Value;
                }
                else
                {
                    userDetailsEntity.Avatar = null;
                }

                if (userDetails.Signature != null)
                {
                    userDetailsEntity.SignatureId = userDetails.Signature.Id.Value;
                }
                else
                {
                    userDetailsEntity.Signature = null;
                }

                unitOfWork.GetGenericRepository<UserDetails>().Add(userDetailsEntity);
                unitOfWork.Save();
                userDetails.UserDetailsId = userDetailsEntity.Id;

                return await ActionResponse<UserViewModel>.ReturnSuccess(userDetails);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, userDetails);
                return await ActionResponse<UserViewModel>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<UserDetailsDto>> GetUserDetails(Guid userId)
        {
            try
            {
                return await ActionResponse<UserDetailsDto>
                    .ReturnSuccess(mapper.Map<UserDetailsDto>(unitOfWork.GetGenericRepository<UserDetails>()
                    .FindBy(ud => ud.UserId == userId)));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, userId);
                return await ActionResponse<UserDetailsDto>.ReturnError("Dogodila se greška prilikom dohvata detalja za korisnika.");
            }
        }

        public async Task<ActionResponse<UserDetailsDto>> UpdateUserDetails(UserDetailsDto userDetails)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<UserDetails>()
                    .FindBy(ud => ud.UserId == userDetails.UserId);
                mapper.Map(userDetails, entityToUpdate);
                unitOfWork.GetGenericRepository<UserDetails>().Update(entityToUpdate);
                userDetails = mapper.Map<UserDetails, UserDetailsDto>(entityToUpdate);
                return await ActionResponse<UserDetailsDto>.ReturnSuccess(userDetails);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, userDetails);
                return await ActionResponse<UserDetailsDto>
                    .ReturnError("Dogodila se greška prilikom ažuriranja podataka o korisniku. Molimo pokušajte ponovno.");
            }
        }

        public async Task<ActionResponse<UserViewModel>> UpdateUserDetails(UserViewModel userDetails)
        {
            try
            {
                var userDetailsEntity = unitOfWork.GetGenericRepository<UserDetails>().FindBy(ud => ud.Id == userDetails.UserDetailsId);
                userDetailsEntity.FirstName = userDetails.FirstName;
                userDetailsEntity.LastName = userDetails.LastName;
                userDetailsEntity.Mobile = userDetails.Mobile;
                userDetailsEntity.Mobile2 = userDetails.Mobile2;
                userDetailsEntity.Phone = userDetails.Phone;
                userDetailsEntity.Title = userDetails.Title;

                if (userDetails.Avatar != null)
                {
                    userDetailsEntity.AvatarId = userDetails.Avatar.Id.Value;
                }
                else
                {
                    userDetailsEntity.Avatar = null;
                    userDetailsEntity.AvatarId = null;
                }

                if (userDetails.Signature != null)
                {
                    userDetailsEntity.SignatureId = userDetails.Signature.Id.Value;
                }
                else
                {
                    userDetailsEntity.Signature = null;
                    userDetailsEntity.SignatureId = null;
                }

                unitOfWork.GetGenericRepository<UserDetails>().Update(userDetailsEntity);

                return await ActionResponse<UserViewModel>.ReturnSuccess(userDetails);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, userDetails);
                return await ActionResponse<UserViewModel>
                    .ReturnError("Dogodila se greška prilikom ažuriranja podataka o korisniku. Molimo pokušajte ponovno.");
            }
        }
    }
}
