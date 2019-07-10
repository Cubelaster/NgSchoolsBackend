using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
                    .GetAll(includeProperties: "Principal.UserDetails.Signature,Country,Region,City,InstitutionFiles.File,GoverningCouncil.GoverningCouncilMembers.User.UserDetails.Signature").FirstOrDefault();

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
                    List<FileDto> files = institution.Files != null
                        ? new List<FileDto>(institution.Files) : new List<FileDto>();

                    GoverningCouncilDto governingCouncil = institution.GoverningCouncil;

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

                    institution.GoverningCouncil = governingCouncil;
                    institution.GoverningCouncil.InstitutionId = institution.Id;
                    if ((await ModifyGoverningCouncil(institution)).IsNotSuccess(out fileResponse, out institution))
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
                    List<FileDto> files = institution.Files != null
                        ? new List<FileDto>(institution.Files) : new List<FileDto>();

                    GoverningCouncilDto governingCouncil = institution.GoverningCouncil;

                    var institutionToUpdate = mapper.Map<InstitutionDto, Institution>(institution);
                    unitOfWork.GetGenericRepository<Institution>().Update(institutionToUpdate);
                    unitOfWork.Save();

                    institution.Files = files;
                    if ((await ModifyFiles(institution))
                        .IsNotSuccess(out ActionResponse<InstitutionDto> fileResponse, out institution))
                    {
                        return fileResponse;
                    }

                    institution.GoverningCouncil = governingCouncil;
                    institution.GoverningCouncil.InstitutionId = institution.Id;
                    if ((await ModifyGoverningCouncil(institution)).IsNotSuccess(out fileResponse, out institution))
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

        #region Files

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

        #endregion Files

        #region Governing Council

        public async Task<ActionResponse<InstitutionDto>> ModifyGoverningCouncil(InstitutionDto entityDto)
        {
            try
            {
                var response = await ActionResponse<InstitutionDto>.ReturnSuccess(entityDto, "Podaci o upravnom vijeću institucije uspješno ažurirani.");

                if (entityDto.GoverningCouncil != null)
                {
                    if (entityDto.GoverningCouncil.Id.HasValue)
                    {
                        if ((await UpdateGoverningCouncil(entityDto.GoverningCouncil))
                            .IsNotSuccess(out ActionResponse<GoverningCouncilDto> councilResponse, out GoverningCouncilDto governingCouncil))
                        {
                            return await ActionResponse<InstitutionDto>.ReturnError(councilResponse.Message);
                        }
                        entityDto.GoverningCouncil = governingCouncil;
                    }
                    else
                    {
                        if ((await InsertGoverningCouncil(entityDto.GoverningCouncil))
                            .IsNotSuccess(out ActionResponse<GoverningCouncilDto> councilResponse, out GoverningCouncilDto governingCouncil))
                        {
                            return await ActionResponse<InstitutionDto>.ReturnError(councilResponse.Message);
                        }
                        entityDto.GoverningCouncil = governingCouncil;
                    }
                }
                else
                {
                    var institution = unitOfWork.GetGenericRepository<Institution>().FindBy(i => i.Id == entityDto.Id.Value, includeProperties: "GoverningCouncil");
                    if (institution.GoverningCouncil != null)
                    {
                        if ((await DeleteGoverningCouncil(institution.GoverningCouncil.Id))
                            .IsNotSuccess(out ActionResponse<GoverningCouncilDto> councilResponse))
                        {
                            return await ActionResponse<InstitutionDto>.ReturnError(councilResponse.Message);
                        }
                    }
                }

                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<InstitutionDto>.ReturnError("Greška prilikom ažuriranja podataka upravnog vijeća.");
            }
        }

        public async Task<ActionResponse<GoverningCouncilDto>> InsertGoverningCouncil(GoverningCouncilDto entityDto)
        {
            try
            {
                List<GoverningCouncilMemberDto> councilMembers = new List<GoverningCouncilMemberDto>(entityDto.GoverningCouncilMembers);

                var entityToAdd = mapper.Map<GoverningCouncilDto, GoverningCouncil>(entityDto);
                unitOfWork.GetGenericRepository<GoverningCouncil>().Add(entityToAdd);
                unitOfWork.Save();

                mapper.Map(entityToAdd, entityDto);

                entityDto.GoverningCouncilMembers = councilMembers;
                if ((await ModifyCouncilMembers(entityDto))
                    .IsNotSuccess(out ActionResponse<GoverningCouncilDto> response, out entityDto))
                {
                    return response;
                }

                return await ActionResponse<GoverningCouncilDto>.ReturnSuccess(entityDto, "Upravno vijeće za instituciju uspješno uneseno.");
            }
            catch (Exception)
            {
                return await ActionResponse<GoverningCouncilDto>.ReturnError("Greška prilikom unosa podataka za upravno vijeće.");
            }
        }

        public async Task<ActionResponse<GoverningCouncilDto>> UpdateGoverningCouncil(GoverningCouncilDto entityDto)
        {
            try
            {
                List<GoverningCouncilMemberDto> councilMembers = new List<GoverningCouncilMemberDto>(entityDto.GoverningCouncilMembers);

                var entityToUpdate = mapper.Map<GoverningCouncilDto, GoverningCouncil>(entityDto);
                unitOfWork.GetGenericRepository<GoverningCouncil>().Update(entityToUpdate);
                unitOfWork.Save();

                mapper.Map(entityToUpdate, entityDto);

                entityDto.GoverningCouncilMembers = councilMembers;
                if ((await ModifyCouncilMembers(entityDto))
                    .IsNotSuccess(out ActionResponse<GoverningCouncilDto> response, out entityDto))
                {
                    return response;
                }

                return await ActionResponse<GoverningCouncilDto>.ReturnSuccess(entityDto, "Upravno vijeće za instituciju uspješno ažurirano.");
            }
            catch (Exception)
            {
                return await ActionResponse<GoverningCouncilDto>.ReturnError("Greška prilikom unosa podataka za upravno vijeće.");
            }
        }

        public async Task<ActionResponse<GoverningCouncilDto>> DeleteGoverningCouncil(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<GoverningCouncil>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<GoverningCouncilDto>.ReturnSuccess(null, "Brisanje upravnog vijeća institucije uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<GoverningCouncilDto>.ReturnError("Greška prilikom brisanja upravnog vijeća institucije.");
            }
        }

        public async Task<ActionResponse<GoverningCouncilDto>> ModifyCouncilMembers(GoverningCouncilDto entityDto)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<GoverningCouncil>()
                    .FindBy(e => e.Id == entityDto.Id.Value, includeProperties: "GoverningCouncilMembers");

                var currentCouncilMembers = entity.GoverningCouncilMembers.ToList();
                var newCouncilMembers = entityDto.GoverningCouncilMembers;

                var membersToRemove = mapper.Map<List<GoverningCouncilMemberDto>>(currentCouncilMembers.Where(cem => !newCouncilMembers
                    .Select(ncm => ncm.Id).Contains(cem.Id)));

                var membersToAdd = newCouncilMembers
                    .Where(ncm => !ncm.Id.HasValue
                    || !currentCouncilMembers.Select(ccm => ccm.Id).Contains(ncm.Id.Value))
                    .Select(ncm =>
                    {
                        ncm.GoverningCouncilId = entity.Id;
                        return ncm;
                    })
                    .ToList();

                var membersToModify = newCouncilMembers
                    .Where(ncm => ncm.Id.HasValue && currentCouncilMembers.Select(ccm => ccm.Id).Contains(ncm.Id.Value))
                    .ToList();

                if ((await RemoveCouncilMembers(membersToRemove))
                    .IsNotSuccess(out ActionResponse<List<GoverningCouncilMemberDto>> actionResponse))
                {
                    return await ActionResponse<GoverningCouncilDto>.ReturnError("Neuspješna promjena članova upravnog vijeća.");
                }

                if ((await AddCouncilMembers(membersToAdd))
                    .IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<GoverningCouncilDto>.ReturnError("Neuspješna promjena članova upravnog vijeća.");
                }

                if ((await UpdateCouncilMembers(membersToModify))
                    .IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<GoverningCouncilDto>.ReturnError(actionResponse.Message);
                }

                return await ActionResponse<GoverningCouncilDto>.ReturnSuccess(entityDto, "Uspješno ažurirani članovi upravnog vijeća.");
            }
            catch (Exception)
            {
                return await ActionResponse<GoverningCouncilDto>.ReturnError("Greška prilikom ažuriranja članova upravnog vijeća.");
            }
        }

        public async Task<ActionResponse<List<GoverningCouncilMemberDto>>> RemoveCouncilMembers(List<GoverningCouncilMemberDto> examCommissionTeachers)
        {
            try
            {
                var response = await ActionResponse<List<GoverningCouncilMemberDto>>.ReturnSuccess(null, "Članovi uspješno maknuti iz upravnog vijeća.");
                examCommissionTeachers.ForEach(async ect =>
                {
                    if ((await RemoveCouncilMember(ect))
                    .IsNotSuccess(out ActionResponse<GoverningCouncilMemberDto> actionResponse))
                    {
                        response = await ActionResponse<List<GoverningCouncilMemberDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<GoverningCouncilMemberDto>>.ReturnError("Greška prilikom micanja članova upravnog vijeća.");
            }
        }

        public async Task<ActionResponse<GoverningCouncilMemberDto>> RemoveCouncilMember(GoverningCouncilMemberDto teacher)
        {
            try
            {
                unitOfWork.GetGenericRepository<GoverningCouncilMember>().Delete(teacher.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<GoverningCouncilMemberDto>.ReturnSuccess(null, "Član upravnog vijeća uspješno izbrisan.");
            }
            catch (Exception)
            {
                return await ActionResponse<GoverningCouncilMemberDto>.ReturnError("Greška prilikom micanja člana upravnog vijeća.");
            }
        }

        public async Task<ActionResponse<List<GoverningCouncilMemberDto>>> AddCouncilMembers(List<GoverningCouncilMemberDto> examCommissionTeachers)
        {
            try
            {
                var response = await ActionResponse<List<GoverningCouncilMemberDto>>.ReturnSuccess(null, "Novi članovi uspješno dodani u upravno vijeće.");
                examCommissionTeachers.ForEach(async ect =>
                {
                    if ((await AddCouncilMember(ect))
                    .IsNotSuccess(out ActionResponse<GoverningCouncilMemberDto> actionResponse))
                    {
                        response = await ActionResponse<List<GoverningCouncilMemberDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<GoverningCouncilMemberDto>>.ReturnError("Greška prilikom dodavanja članova u upravno vijeće.");
            }
        }

        public async Task<ActionResponse<GoverningCouncilMemberDto>> AddCouncilMember(GoverningCouncilMemberDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<GoverningCouncilMemberDto, GoverningCouncilMember>(entityDto);
                unitOfWork.GetGenericRepository<GoverningCouncilMember>().Add(entityToAdd);
                unitOfWork.Save();

                mapper.Map(entityToAdd, entityDto);

                unitOfWork.GetContext().Entry(entityToAdd).State = EntityState.Detached;
                entityToAdd = null;

                return await ActionResponse<GoverningCouncilMemberDto>
                    .ReturnSuccess(entityDto, "Novi član uspješno dodan u upravno vijeće.");
            }
            catch (Exception)
            {
                return await ActionResponse<GoverningCouncilMemberDto>.ReturnError("Greška prilikom dodavanja člana u upravno vijeće.");
            }
        }

        public async Task<ActionResponse<List<GoverningCouncilMemberDto>>> UpdateCouncilMembers(List<GoverningCouncilMemberDto> commissionMembers)
        {
            try
            {
                var response = await ActionResponse<List<GoverningCouncilMemberDto>>.ReturnSuccess(null, "Članovi upravnog vijeća uspješno ažurirani.");
                commissionMembers.ForEach(async cm =>
                {
                    if ((await UpdateCouncilMember(cm))
                    .IsNotSuccess(out ActionResponse<GoverningCouncilMemberDto> actionResponse))
                    {
                        response = await ActionResponse<List<GoverningCouncilMemberDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<GoverningCouncilMemberDto>>.ReturnError("Greška prilikom ažuriranja članova upravnog vijeća.");
            }
        }

        public async Task<ActionResponse<GoverningCouncilMemberDto>> UpdateCouncilMember(GoverningCouncilMemberDto member)
        {
            try
            {
                var entityToModify = mapper.Map<GoverningCouncilMemberDto, GoverningCouncilMember>(member);
                unitOfWork.GetGenericRepository<GoverningCouncilMember>().Update(entityToModify);
                unitOfWork.Save();

                mapper.Map(entityToModify, member);

                unitOfWork.GetContext().Entry(entityToModify).State = EntityState.Detached;
                entityToModify = null;

                return await ActionResponse<GoverningCouncilMemberDto>.ReturnSuccess(member, "Član upravnog vijeća uspješno ažuriran.");
            }
            catch (Exception)
            {
                return await ActionResponse<GoverningCouncilMemberDto>.ReturnError("Greška prilikom ažuriranja člana upravnog vijeća.");
            }
        }

        #endregion Governing Council
    }
}
