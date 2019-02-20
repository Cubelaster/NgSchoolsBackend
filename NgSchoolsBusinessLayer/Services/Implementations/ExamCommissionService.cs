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

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class ExamCommissionService : IExamCommissionService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public ExamCommissionService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<ExamCommissionDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<ExamCommission>()
                    .FindBy(c => c.Id == id, includeProperties: "UserExamCommissions.User.UserDetails");
                return await ActionResponse<ExamCommissionDto>
                    .ReturnSuccess(mapper.Map<ExamCommission, ExamCommissionDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
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
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<ExamCommissionDto>>.ReturnError("Greška prilikom dohvata svih ispitnih komisija.");
            }
        }

        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            try
            {
                return await ActionResponse<int>.ReturnSuccess(unitOfWork.GetGenericRepository<Subject>().GetAllAsQueryable().Count());
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
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
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<ExamCommissionDto>>.ReturnError("Greška prilikom dohvata straničnih podataka ispitnih komisija.");
            }
        }

        public async Task<ActionResponse<ExamCommissionDto>> Insert(ExamCommissionDto entityDto)
        {
            try
            {
                List<Guid> teachers = new List<Guid>(entityDto.TeacherIds);
                var entityToAdd = mapper.Map<ExamCommissionDto, ExamCommission>(entityDto);
                unitOfWork.GetGenericRepository<ExamCommission>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);
                entityDto.TeacherIds = new List<Guid>(teachers);
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
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
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

                return await ActionResponse<ExamCommissionDto>.ReturnSuccess(entityDto);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
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
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<ExamCommissionDto>.ReturnError("Greška prilikom brisanja ispitne komisije.");
            }
        }

        public async Task<ActionResponse<ExamCommissionDto>> ModifyExamTeachers(ExamCommissionDto examCommission)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<ExamCommission>()
                    .FindBy(e => e.Id == examCommission.Id.Value, includeProperties: "UserExamCommissions");
                var currentExamTeachers = entity.UserExamCommissions.ToList();
                var newTeachers = new List<Guid>(examCommission.TeacherIds);

                var teachersToRemove = mapper.Map<List<UserExamCommission>, List<UserExamCommissionDto>>
                    (currentExamTeachers.Where(cet => !newTeachers.Contains(cet.UserId)).ToList());

                var teachersToAdd = newTeachers
                    .Where(nt => !currentExamTeachers.Select(uec => uec.UserId).Contains(nt))
                    .Select(nt => new UserExamCommissionDto { UserId = nt, ExamCommissionId = examCommission.Id })
                    .ToList();

                if ((await RemoveExamTeachers(teachersToRemove))
                    .IsNotSuccess(out ActionResponse<List<UserExamCommissionDto>> actionResponse))
                {
                    return await ActionResponse<ExamCommissionDto>.ReturnError("Neuspješna promjena nastavnika u ispitnoj komisiji.");
                }

                if ((await AddExamTeachers(teachersToAdd))
                    .IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<ExamCommissionDto>.ReturnError("Neuspješna promjena nastavnika u ispitnoj komisiji.");
                }
                return await ActionResponse<ExamCommissionDto>.ReturnSuccess(examCommission, "Uspješno izmijenjeni članovi ispitne komisije.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, examCommission);
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
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, examCommissionTeachers);
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
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, teacher);
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
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, examCommissionTeachers);
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
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, teacher);
                return await ActionResponse<UserExamCommissionDto>.ReturnError("Greška prilikom dodavanja nastavnika u ispitnu komisiju.");
            }
        }
    }
}
