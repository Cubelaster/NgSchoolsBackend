using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class ExamCommissionService : IExamCommissionService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public ExamCommissionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<ExamCommissionDto>> GetById(int id)
        {
            try
            {
                var query = unitOfWork.GetGenericRepository<ExamCommission>()
                    .ReadAll()
                    .Where(e => e.Id == id);

                var entity = mapper.ProjectTo<ExamCommissionDto>(query).Single();

                return await ActionResponse<ExamCommissionDto>.ReturnSuccess(entity);
            }
            catch (Exception)
            {
                return await ActionResponse<ExamCommissionDto>.ReturnError("Greška prilikom dohvata ispitne komisije.");
            }
        }

        public async Task<ActionResponse<List<ExamCommissionDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<ExamCommission>()
                    .GetAll(includeProperties: "UserExamCommissions.User.UserDetails");
                return await ActionResponse<List<ExamCommissionDto>>
                    .ReturnSuccess(mapper.Map<List<ExamCommission>, List<ExamCommissionDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<ExamCommissionDto>>.ReturnError("Greška prilikom dohvata svih ispitnih komisija.");
            }
        }

        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            try
            {
                return await ActionResponse<int>.ReturnSuccess(unitOfWork.GetGenericRepository<Subject>().GetAllAsQueryable().Count());
            }
            catch (Exception)
            {
                return await ActionResponse<int>.ReturnError("Greška prilikom dohvata broja ispitnih komisija.");
            }
        }

        public async Task<ActionResponse<PagedResult<ExamCommissionDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<ExamCommission>()
                    .GetAllAsQueryable(includeProperties: "UserExamCommissions.User.UserDetails").GetPaged(pagedRequest);

                var pagedResult = new PagedResult<ExamCommissionDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<ExamCommission>, List<ExamCommissionDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<ExamCommissionDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<ExamCommissionDto>>.ReturnError("Greška prilikom dohvata straničnih podataka ispitnih komisija.");
            }
        }

        public async Task<ActionResponse<ExamCommissionDto>> Insert(ExamCommissionDto entityDto)
        {
            try
            {
                List<UserExamCommissionDto> commissionMembers = new List<UserExamCommissionDto>(entityDto.CommissionMembers);
                var entityToAdd = mapper.Map<ExamCommissionDto, ExamCommission>(entityDto);
                unitOfWork.GetGenericRepository<ExamCommission>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);
                entityDto.CommissionMembers = new List<UserExamCommissionDto>(commissionMembers);
                if ((await ModifyExamTeachers(entityDto)).IsNotSuccess(out ActionResponse<ExamCommissionDto> actionResponse, out entityDto))
                {
                    return actionResponse;
                }

                if ((await GetById(entityToAdd.Id)).IsNotSuccess(out actionResponse, out entityDto))
                {
                    return actionResponse;
                }

                return await ActionResponse<ExamCommissionDto>.ReturnSuccess(entityDto);
            }
            catch (Exception)
            {
                return await ActionResponse<ExamCommissionDto>.ReturnError("Greška prilikom unosa ispitne komisije.");
            }
        }

        public async Task<ActionResponse<ExamCommissionDto>> Update(ExamCommissionDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<ExamCommissionDto, ExamCommission>(entityDto);
                unitOfWork.GetGenericRepository<ExamCommission>().Update(entityToUpdate);
                if ((await ModifyExamTeachers(entityDto)).IsNotSuccess(out ActionResponse<ExamCommissionDto> response, out entityDto))
                {
                    return response;
                }
                unitOfWork.Save();

                if ((await GetById(entityToUpdate.Id)).IsNotSuccess(out response, out entityDto))
                {
                    return response;
                }

                unitOfWork.GetContext().Entry(entityToUpdate).State = EntityState.Detached;
                entityToUpdate = null;

                return await ActionResponse<ExamCommissionDto>.ReturnSuccess(entityDto);
            }
            catch (Exception)
            {
                return await ActionResponse<ExamCommissionDto>.ReturnError("Greška prilikom ažuriranja ispitne komisije.");
            }
        }

        public async Task<ActionResponse<ExamCommissionDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<ExamCommission>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<ExamCommissionDto>.ReturnSuccess(null, "Brisanje ispitne komisije uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<ExamCommissionDto>.ReturnError("Greška prilikom brisanja ispitne komisije.");
            }
        }

        public async Task<ActionResponse<ExamCommissionDto>> ModifyExamTeachers(ExamCommissionDto examCommission)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<ExamCommission>()
                    .FindBy(e => e.Id == examCommission.Id.Value, includeProperties: "UserExamCommissions");

                var currentCommissionMembers = entity.UserExamCommissions.ToList();
                var newCommissionMembers = examCommission.CommissionMembers;

                var membersToRemove = mapper.Map<List<UserExamCommissionDto>>(currentCommissionMembers.Where(cem => !newCommissionMembers
                    .Select(ncm => ncm.Id).Contains(cem.Id)));

                var membersToAdd = newCommissionMembers
                    .Where(ncm => !ncm.Id.HasValue
                    || !currentCommissionMembers.Select(ccm => ccm.Id).Contains(ncm.Id.Value))
                    .Select(ncm =>
                    {
                        ncm.ExamCommissionId = examCommission.Id;
                        return ncm;
                    })
                    .ToList();

                var membersToModify = newCommissionMembers
                    .Where(ncm => ncm.Id.HasValue && currentCommissionMembers.Select(ccm => ccm.Id).Contains(ncm.Id.Value))
                    .ToList();

                if ((await RemoveExamTeachers(membersToRemove))
                    .IsNotSuccess(out ActionResponse<List<UserExamCommissionDto>> actionResponse))
                {
                    return await ActionResponse<ExamCommissionDto>.ReturnError("Neuspješna promjena nastavnika u ispitnoj komisiji.");
                }

                if ((await AddExamTeachers(membersToAdd))
                    .IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<ExamCommissionDto>.ReturnError("Neuspješna promjena nastavnika u ispitnoj komisiji.");
                }

                if((await ModifyCommissionMembers(membersToModify))
                    .IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<ExamCommissionDto>.ReturnError(actionResponse.Message);
                }

                return await ActionResponse<ExamCommissionDto>.ReturnSuccess(examCommission, "Uspješno izmijenjeni članovi ispitne komisije.");
            }
            catch (Exception)
            {
                return await ActionResponse<ExamCommissionDto>.ReturnError("Greška prilikom ažuriranja članova ispitne komisije.");
            }
        }

        public async Task<ActionResponse<List<UserExamCommissionDto>>> RemoveExamTeachers(List<UserExamCommissionDto> examCommissionTeachers)
        {
            try
            {
                var response = await ActionResponse<List<UserExamCommissionDto>>.ReturnSuccess(null, "Nastavnici uspješno maknuti iz ispitne komisije.");
                examCommissionTeachers.ForEach(async ect =>
                {
                    if ((await RemoveExamTeacher(ect))
                    .IsNotSuccess(out ActionResponse<UserExamCommissionDto> actionResponse))
                    {
                        response = await ActionResponse<List<UserExamCommissionDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<UserExamCommissionDto>>.ReturnError("Greška prilikom micanja članova ispitne komisije.");
            }
        }

        public async Task<ActionResponse<UserExamCommissionDto>> RemoveExamTeacher(UserExamCommissionDto teacher)
        {
            try
            {
                unitOfWork.GetGenericRepository<UserExamCommission>().Delete(teacher.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<UserExamCommissionDto>.ReturnSuccess(null, "Nastavnik upsješno izbrisan iz komisije.");
            }
            catch (Exception)
            {
                return await ActionResponse<UserExamCommissionDto>.ReturnError("Greška prilikom micanja člana ispitne komisije.");
            }
        }

        public async Task<ActionResponse<List<UserExamCommissionDto>>> AddExamTeachers(List<UserExamCommissionDto> examCommissionTeachers)
        {
            try
            {
                var response = await ActionResponse<List<UserExamCommissionDto>>.ReturnSuccess(null, "Nastavnici uspješno dodani u ispitnu komisiju.");
                examCommissionTeachers.ForEach(async ect =>
                {
                    if ((await AddExamTeacher(ect))
                    .IsNotSuccess(out ActionResponse<UserExamCommissionDto> actionResponse))
                    {
                        response = await ActionResponse<List<UserExamCommissionDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<UserExamCommissionDto>>.ReturnError("Greška prilikom dodavanja nastavnika u ispitnu komisiju.");
            }
        }

        public async Task<ActionResponse<UserExamCommissionDto>> AddExamTeacher(UserExamCommissionDto teacher)
        {
            try
            {
                var entityToAdd = mapper.Map<UserExamCommissionDto, UserExamCommission>(teacher);
                unitOfWork.GetGenericRepository<UserExamCommission>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<UserExamCommissionDto>
                    .ReturnSuccess(mapper.Map<UserExamCommission, UserExamCommissionDto>(entityToAdd),
                    "Nastavnik uspješno dodan u komisiju.");
            }
            catch (Exception)
            {
                return await ActionResponse<UserExamCommissionDto>.ReturnError("Greška prilikom dodavanja nastavnika u ispitnu komisiju.");
            }
        }

        public async Task<ActionResponse<List<UserExamCommissionDto>>> ModifyCommissionMembers(List<UserExamCommissionDto> commissionMembers)
        {
            try
            {
                var response = await ActionResponse<List<UserExamCommissionDto>>.ReturnSuccess(null, "Članovi ispitne komisije uspješno ažurirani.");
                commissionMembers.ForEach(async cm =>
                {
                    if ((await ModifyCommissionMember(cm))
                    .IsNotSuccess(out ActionResponse<UserExamCommissionDto> actionResponse))
                    {
                        response = await ActionResponse<List<UserExamCommissionDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<UserExamCommissionDto>>.ReturnError("Greška prilikom ažuriranja člana ispitne komisije.");
            }
        }

        public async Task<ActionResponse<UserExamCommissionDto>> ModifyCommissionMember(UserExamCommissionDto member)
        {
            try
            {
                var entityToModify = mapper.Map<UserExamCommissionDto, UserExamCommission>(member);
                unitOfWork.GetGenericRepository<UserExamCommission>().Update(entityToModify);
                unitOfWork.Save();

                mapper.Map(entityToModify, member);

                unitOfWork.GetContext().Entry(entityToModify).State = EntityState.Detached;
                entityToModify = null;

                return await ActionResponse<UserExamCommissionDto>.ReturnSuccess(member,"Član komisije uspješno ažuriran.");
            }
            catch (Exception)
            {
                return await ActionResponse<UserExamCommissionDto>.ReturnError("Greška prilikom ažuriranja člana ispitne komisije.");
            }
        }
    }
}
