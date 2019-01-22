using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
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
    public class DiaryService : IDiaryService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public DiaryService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<DiaryDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<Diary>()
                    .FindBy(c => c.Id == id, includeProperties: "StudentGroups.StudentGroup");
                return await ActionResponse<DiaryDto>
                    .ReturnSuccess(mapper.Map<Diary, DiaryDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<DiaryDto>.ReturnError("Greška prilikom dohvata dnevnika rada.");
            }
        }

        public async Task<ActionResponse<List<DiaryDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<Diary>()
                    .GetAll(includeProperties: "StudentGroups.StudentGroup");
                return await ActionResponse<List<DiaryDto>>
                    .ReturnSuccess(mapper.Map<List<Diary>, List<DiaryDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<DiaryDto>>.ReturnError("Greška prilikom dohvata svih dnevnika rada.");
            }
        }

        public async Task<ActionResponse<PagedResult<DiaryDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<Diary>()
                    .GetAllAsQueryable(includeProperties: "StudentGroups.StudentGroup").GetPaged(pagedRequest);

                var pagedResult = new PagedResult<DiaryDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<Diary>, List<DiaryDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<DiaryDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<DiaryDto>>.ReturnError("Greška prilikom dohvata straničnih podataka grupe studenata.");
            }
        }

        public async Task<ActionResponse<DiaryDto>> Insert(DiaryDto entityDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var entityToAdd = mapper.Map<DiaryDto, Diary>(entityDto);
                    unitOfWork.GetGenericRepository<Diary>().Add(entityToAdd);
                    unitOfWork.Save();
                    mapper.Map(entityToAdd, entityDto);

                    if ((await ModifyStudentGroupsForDiary(entityDto)).IsNotSuccess(out ActionResponse<DiaryDto> response, out entityDto))
                    {
                        return response;
                    }

                    scope.Complete();
                    return await ActionResponse<DiaryDto>
                        .ReturnSuccess(mapper.Map(entityToAdd, entityDto));
                }
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<DiaryDto>.ReturnError("Greška prilikom upisa dnevnika rada.");
            }
        }

        private async Task<ActionResponse<DiaryDto>> ModifyStudentGroupsForDiary(DiaryDto entityDto)
        {
            try
            {
                var existingGroups = mapper.Map<List<DiaryStudentGroup>, List<DiaryStudentGroupDto>>(
                    unitOfWork.GetGenericRepository<DiaryStudentGroup>().GetAll(s => s.DiaryId == entityDto.Id));

                entityDto.StudentGroupIds = entityDto.StudentGroupIds ?? new List<int>();

                var groupsToAdd = entityDto.StudentGroupIds
                    .Where(nsg => !existingGroups.Select(eg => eg.StudentGroupId).Contains(nsg))
                    .Select(dsg => new DiaryStudentGroupDto
                    {
                        DiaryId = entityDto.Id,
                        StudentGroupId = dsg
                    }).ToList();

                var groupsToRemove = existingGroups
                    .Where(es => !entityDto.StudentGroupIds.Contains(es.StudentGroupId.Value))
                    .ToList();

                if ((await AddStudentGroups(groupsToAdd)).IsNotSuccess<List<DiaryStudentGroupDto>>(out ActionResponse<List<DiaryStudentGroupDto>> groupResponse, out groupsToAdd))
                {
                    return await ActionResponse<DiaryDto>.ReturnError(groupResponse.Message);
                }

                if ((await RemoveStudentGroups(groupsToRemove)).IsNotSuccess(out groupResponse))
                {
                    return await ActionResponse<DiaryDto>.ReturnError(groupResponse.Message);
                }

                return await ActionResponse<DiaryDto>.ReturnSuccess(entityDto, "Dnevnik rada uspješno unesen.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<DiaryDto>.ReturnError("Greška prilikom ažuriranja grupa studenata u dnevniku rada.");
            }
        }

        public async Task<ActionResponse<List<DiaryStudentGroupDto>>> AddStudentGroups(List<DiaryStudentGroupDto> studentGroupDtos)
        {
            try
            {
                var response = await ActionResponse<List<DiaryStudentGroupDto>>.ReturnSuccess(studentGroupDtos, "Pridruživanje grupa studenata uspješan.");
                studentGroupDtos.ForEach(async s =>
                {
                    if ((await InsertDiaryStudentGroup(s)).IsNotSuccess(out ActionResponse<DiaryStudentGroupDto> insertResponse, out s))
                    {
                        response = await ActionResponse<List<DiaryStudentGroupDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<DiaryStudentGroupDto>>.ReturnError("Greška prilikom pridruživanja grupa studenata.");
            }
        }

        public async Task<ActionResponse<DiaryStudentGroupDto>> InsertDiaryStudentGroup(DiaryStudentGroupDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<DiaryStudentGroupDto, DiaryStudentGroup>(entityDto);
                unitOfWork.GetGenericRepository<DiaryStudentGroup>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);

                return await ActionResponse<DiaryStudentGroupDto>
                    .ReturnSuccess(mapper.Map(entityToAdd, entityDto));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<DiaryStudentGroupDto>.ReturnError("Greška prilikom unošenja grupe studenata za dnevnik rada.");
            }
        }

        public async Task<ActionResponse<List<DiaryStudentGroupDto>>> RemoveStudentGroups(List<DiaryStudentGroupDto> studentGroupDtos)
        {
            try
            {
                var response = await ActionResponse<List<DiaryStudentGroupDto>>.ReturnSuccess(studentGroupDtos, "Unos predmeta uspješan.");
                studentGroupDtos.ForEach(async s =>
                {
                    if ((await DeleteDiaryStudentGroup(s)).IsNotSuccess(out ActionResponse<DiaryStudentGroupDto> insertResponse, out s))
                    {
                        response = await ActionResponse<List<DiaryStudentGroupDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });

                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<DiaryStudentGroupDto>>.ReturnError("Greška prilikom upisa studenta.");
            }
        }

        public async Task<ActionResponse<DiaryStudentGroupDto>> DeleteDiaryStudentGroup(DiaryStudentGroupDto entityDto)
        {
            try
            {
                unitOfWork.GetGenericRepository<DiaryStudentGroup>().Delete(entityDto.Id.Value);
                unitOfWork.Save();

                return await ActionResponse<DiaryStudentGroupDto>.ReturnSuccess(null, "Brisanje grupe studenata iz dnevnika rada uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<DiaryStudentGroupDto>.ReturnError("Greška prilikom unošenja grupe studenata za dnevnik rada.");
            }
        }

        public async Task<ActionResponse<DiaryDto>> Update(DiaryDto entityDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var entityToUpdate = mapper.Map<DiaryDto, Diary>(entityDto);
                    unitOfWork.GetGenericRepository<Diary>().Update(entityToUpdate);
                    unitOfWork.Save();

                    if ((await ModifyStudentGroupsForDiary(entityDto)).IsNotSuccess(out ActionResponse<DiaryDto> response, out entityDto))
                    {
                        return response;
                    }

                    scope.Complete();
                    return await ActionResponse<DiaryDto>
                        .ReturnSuccess(mapper.Map(entityToUpdate, entityDto), "Dnevnik rada uspješno ažuriran.");
                }
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<DiaryDto>.ReturnError("Greška prilikom ažuriranja dnevnika rada.");
            }
        }

        public async Task<ActionResponse<DiaryDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<Diary>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<DiaryDto>.ReturnSuccess(null, "Brisanje dnevnika rada uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<DiaryDto>.ReturnError("Greška prilikom brisanja dnevnika rada.");
            }
        }
    }
}