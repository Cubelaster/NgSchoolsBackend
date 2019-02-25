using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class InstitutionService : IInstitutionService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public InstitutionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<InstitutionDto>> GetInstitution()
        {
            try
            {
                var institution = unitOfWork.GetGenericRepository<Institution>()
                    .GetAll(includeProperties: "Principal,Country,Region,City,InstitutionFiles.File").FirstOrDefault();
                return await ActionResponse<InstitutionDto>
                    .ReturnSuccess(mapper.Map<Institution, InstitutionDto>(institution));
            }
            catch (Exception)
            {
                return await ActionResponse<InstitutionDto>.ReturnError("Greška prilikom dohvata podataka o instituciji");
            }
        }

        public async Task<ActionResponse<InstitutionDto>> Insert(InstitutionDto institution)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<FileDto> files = new List<FileDto>(institution.Files);
                    var institutionToAdd = mapper.Map<InstitutionDto, Institution>(institution);
                    unitOfWork.GetGenericRepository<Institution>().Add(institutionToAdd);
                    unitOfWork.Save();

                    institution.Id = institutionToAdd.Id;
                    institution.Files = files;
                    if ((await ModifyFiles(institution))
                        .IsNotSuccess(out ActionResponse<InstitutionDto> fileResponse, out institution))
                    {
                        return fileResponse;
                    }

                    scope.Complete();
                }
                return await GetInstitution();
            }
            catch (Exception)
            {
                return await ActionResponse<InstitutionDto>.ReturnError("Greška prilikom unosa podataka za instituciju.");
            }
        }

        public async Task<ActionResponse<InstitutionDto>> Update(InstitutionDto institution)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<FileDto> files = new List<FileDto>(institution.Files);
                    var institutionToUpdate = mapper.Map<InstitutionDto, Institution>(institution);
                    unitOfWork.GetGenericRepository<Institution>().Update(institutionToUpdate);
                    unitOfWork.Save();

                    institution.Files = files;
                    if ((await ModifyFiles(institution))
                        .IsNotSuccess(out ActionResponse<InstitutionDto> fileResponse, out institution))
                    {
                        return fileResponse;
                    }

                    scope.Complete();
                }
                return await GetInstitution();
            }
            catch (Exception)
            {
                return await ActionResponse<InstitutionDto>.ReturnError("Greška prilikom ažuriranja podataka institucije.");
            }
        }

        public async Task<ActionResponse<InstitutionDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Institution>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<InstitutionDto>.ReturnSuccess(null, "Brisanje institucije uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<InstitutionDto>.ReturnError("Greška prilikom brisanja institucije.");
            }
        }

        public async Task<ActionResponse<InstitutionDto>> ModifyFiles(InstitutionDto entityDto)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Institution>()
                    .FindBy(e => e.Id == entityDto.Id, includeProperties: "InstitutionFiles.File");

                var currentFiles = mapper.Map<List<InstitutionFile>, List<InstitutionFileDto>>(entity.InstitutionFiles.ToList());

                var newFiles = entityDto.Files;

                var filesToRemove = currentFiles
                    .Where(cet => !newFiles.Select(f => f.Id).Contains(cet.FileId)).ToList();

                var filesToAdd = newFiles
                    .Where(nt => !currentFiles.Select(uec => uec.FileId).Contains(nt.Id))
                    .Select(sf => new InstitutionFileDto
                    {
                        FileId = sf.Id,
                        InstitutionId = entityDto.Id
                    })
                    .ToList();

                if ((await RemoveFiles(filesToRemove))
                    .IsNotSuccess(out ActionResponse<List<InstitutionFileDto>> removeResponse))
                {
                    return await ActionResponse<InstitutionDto>.ReturnError("Neuspješno micanje dokumenata instituciji.");
                }

                if ((await AddFiles(filesToAdd)).IsNotSuccess(out ActionResponse<List<InstitutionFileDto>> addResponse, out filesToAdd))
                {
                    return await ActionResponse<InstitutionDto>.ReturnError("Neuspješno dodavanje dokumenata instituciji.");
                }
                return await ActionResponse<InstitutionDto>.ReturnSuccess(entityDto, "Uspješno izmijenjeni dokumenti institucije.");
            }
            catch (Exception)
            {
                return await ActionResponse<InstitutionDto>.ReturnError("Greška prilikom izmjene dokumenata institucije.");
            }
        }

        public async Task<ActionResponse<List<InstitutionFileDto>>> RemoveFiles(List<InstitutionFileDto> entities)
        {
            try
            {
                var response = await ActionResponse<List<InstitutionFileDto>>.ReturnSuccess(null, "Datoteke uspješno maknute sa institucije.");
                entities.ForEach(async file =>
                {
                    if ((await RemoveFile(file))
                        .IsNotSuccess(out ActionResponse<InstitutionFileDto> actionResponse))
                    {
                        response = await ActionResponse<List<InstitutionFileDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<InstitutionFileDto>>.ReturnError("Greška prilikom micanja dokumenata sa institucije.");
            }
        }

        public async Task<ActionResponse<InstitutionFileDto>> RemoveFile(InstitutionFileDto entity)
        {
            try
            {
                unitOfWork.GetGenericRepository<InstitutionFile>().Delete(entity.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<InstitutionFileDto>.ReturnSuccess(null, "Dokument uspješno maknut s institucije.");
            }
            catch (Exception)
            {
                return await ActionResponse<InstitutionFileDto>.ReturnError("Greška prilikom micanja dokumenta s institucije.");
            }
        }

        public async Task<ActionResponse<List<InstitutionFileDto>>> AddFiles(List<InstitutionFileDto> entities)
        {
            try
            {
                var response = await ActionResponse<List<InstitutionFileDto>>.ReturnSuccess(null, "Dokumenti uspješno dodani instituciji.");
                entities.ForEach(async s =>
                {
                    if ((await AddFile(s))
                        .IsNotSuccess(out ActionResponse<InstitutionFileDto> actionResponse, out s))
                    {
                        response = await ActionResponse<List<InstitutionFileDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<InstitutionFileDto>>.ReturnError("Greška prilikom dodavanja dokumenata instituciji.");
            }
        }

        public async Task<ActionResponse<InstitutionFileDto>> AddFile(InstitutionFileDto file)
        {
            try
            {
                var entityToAdd = mapper.Map<InstitutionFileDto, InstitutionFile>(file);
                unitOfWork.GetGenericRepository<InstitutionFile>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<InstitutionFileDto>
                    .ReturnSuccess(mapper.Map<InstitutionFile, InstitutionFileDto>(entityToAdd), "Dokument uspješno dodan instituciji.");
            }
            catch (Exception)
            {
                return await ActionResponse<InstitutionFileDto>.ReturnError("Greška prilikom dodavanja dokumenta instituciji.");
            }
        }
    }
}
