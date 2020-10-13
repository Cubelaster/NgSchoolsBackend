using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class UserDetailsService : IUserDetailsService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public UserDetailsService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
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
                    TitlePrefix = userDetails.TitlePrefix,
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
            catch (Exception)
            {
                return await ActionResponse<UserViewModel>.ReturnError("Greška prilikom upisa korisničkih detalja.");
            }
        }

        public async Task<ActionResponse<UserDetailsDto>> GetUserDetails(Guid userId)
        {
            try
            {
                var userDetails = unitOfWork.GetGenericRepository<UserDetails>()
                    .FindBy(ud => ud.UserId == userId,
                    includeProperties: "Avatar,Signature,City,Country,Region,TeacherFiles.File");
                return await ActionResponse<UserDetailsDto>
                    .ReturnSuccess(mapper.Map<UserDetailsDto>(userDetails));
            }
            catch (Exception)
            {
                return await ActionResponse<UserDetailsDto>.ReturnError("Dogodila se greška prilikom dohvata detalja za korisnika.");
            }
        }

        public async Task<ActionResponse<UserDetailsDto>> UpdateUserDetails(UserDetailsDto userDetails)
        {
            try
            {
                List<FileDto> teacherFiles = new List<FileDto>(userDetails.TeacherFiles);

                var entityToUpdate = unitOfWork.GetGenericRepository<UserDetails>()
                    .FindBy(ud => ud.UserId == userDetails.UserId);
                mapper.Map(userDetails, entityToUpdate);

                unitOfWork.GetGenericRepository<UserDetails>().Update(entityToUpdate);
                unitOfWork.Save();

                userDetails = mapper.Map<UserDetails, UserDetailsDto>(entityToUpdate);
                userDetails.TeacherFiles = teacherFiles;

                if ((await ModifyFiles(userDetails))
                    .IsNotSuccess(out ActionResponse<UserDetailsDto> fileResponse, out userDetails))
                {
                    return fileResponse;
                }

                return await GetUserDetails(userDetails.UserId.Value);
            }
            catch (Exception)
            {
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
                userDetailsEntity.TitlePrefix = userDetails.TitlePrefix;

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
                unitOfWork.Save();
                return await ActionResponse<UserViewModel>.ReturnSuccess(userDetails);
            }
            catch (Exception)
            {
                return await ActionResponse<UserViewModel>
                    .ReturnError("Dogodila se greška prilikom ažuriranja podataka o korisniku. Molimo pokušajte ponovno.");
            }
        }

        #region Files

        public async Task<ActionResponse<UserDetailsDto>> ModifyFiles(UserDetailsDto entityDto)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<UserDetails>()
                    .FindBy(e => e.Id == entityDto.Id, includeProperties: "TeacherFiles.File");

                var currentFiles = mapper.Map<List<TeacherFile>, List<TeacherFileDto>>(entity.TeacherFiles.ToList());

                var newFiles = entityDto.TeacherFiles;

                var filesToRemove = currentFiles
                    .Where(cet => !newFiles.Select(f => f.Id).Contains(cet.FileId)).ToList();

                var filesToAdd = newFiles
                    .Where(nt => !currentFiles.Select(uec => uec.FileId).Contains(nt.Id))
                    .Select(sf => new TeacherFileDto
                    {
                        FileId = sf.Id,
                        UserDetailsId = entityDto.Id
                    })
                    .ToList();

                if ((await RemoveFiles(filesToRemove))
                    .IsNotSuccess(out ActionResponse<List<TeacherFileDto>> removeResponse))
                {
                    return await ActionResponse<UserDetailsDto>.ReturnError("Neuspješno micanje dokumenata s nastavnika.");
                }

                if ((await AddFiles(filesToAdd)).IsNotSuccess(out ActionResponse<List<TeacherFileDto>> addResponse, out filesToAdd))
                {
                    return await ActionResponse<UserDetailsDto>.ReturnError("Neuspješno dodavanje dokumenata nastavniku.");
                }
                return await ActionResponse<UserDetailsDto>.ReturnSuccess(entityDto, "Uspješno izmijenjeni dokumenti nastavnika.");
            }
            catch (Exception)
            {
                return await ActionResponse<UserDetailsDto>.ReturnError("Greška prilikom izmjene dokumenata nastavnika.");
            }
        }

        public async Task<ActionResponse<List<TeacherFileDto>>> RemoveFiles(List<TeacherFileDto> entities)
        {
            try
            {
                var response = await ActionResponse<List<TeacherFileDto>>.ReturnSuccess(null, "Datoteke uspješno maknute sa nastavnika.");
                entities.ForEach(async file =>
                {
                    if ((await RemoveFile(file))
                        .IsNotSuccess(out ActionResponse<TeacherFileDto> actionResponse))
                    {
                        response = await ActionResponse<List<TeacherFileDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<TeacherFileDto>>.ReturnError("Greška prilikom micanja dokumenata sa nastavnika.");
            }
        }

        public async Task<ActionResponse<TeacherFileDto>> RemoveFile(TeacherFileDto entity)
        {
            try
            {
                unitOfWork.GetGenericRepository<TeacherFile>().Delete(entity.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<TeacherFileDto>.ReturnSuccess(null, "Dokument uspješno maknut s nastavnika.");
            }
            catch (Exception)
            {
                return await ActionResponse<TeacherFileDto>.ReturnError("Greška prilikom micanja dokumenta s nastavnika.");
            }
        }

        public async Task<ActionResponse<List<TeacherFileDto>>> AddFiles(List<TeacherFileDto> entities)
        {
            try
            {
                var response = await ActionResponse<List<TeacherFileDto>>.ReturnSuccess(null, "Dokumenti uspješno dodani nastavniku.");
                entities.ForEach(async s =>
                {
                    if ((await AddFile(s))
                        .IsNotSuccess(out ActionResponse<TeacherFileDto> actionResponse, out s))
                    {
                        response = await ActionResponse<List<TeacherFileDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<TeacherFileDto>>.ReturnError("Greška prilikom dodavanja dokumenata nastavniku.");
            }
        }

        public async Task<ActionResponse<TeacherFileDto>> AddFile(TeacherFileDto file)
        {
            try
            {
                var entityToAdd = mapper.Map<TeacherFileDto, TeacherFile>(file);
                unitOfWork.GetGenericRepository<TeacherFile>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<TeacherFileDto>
                    .ReturnSuccess(mapper.Map<TeacherFile, TeacherFileDto>(entityToAdd), "Dokument uspješno dodan nastavniku.");
            }
            catch (Exception)
            {
                return await ActionResponse<TeacherFileDto>.ReturnError("Greška prilikom dodavanja dokumenta nastavniku.");
            }
        }

        #endregion Files
    }
}
