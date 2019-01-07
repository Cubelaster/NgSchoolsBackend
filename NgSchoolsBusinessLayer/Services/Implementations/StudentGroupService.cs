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
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class StudentGroupService : IStudentGroupService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public StudentGroupService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<StudentGroupDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentGroup>()
                    .FindBy(c => c.Id == id, includeProperties: "StudentsInGroup.Student");
                return await ActionResponse<StudentGroupDto>
                    .ReturnSuccess(mapper.Map<StudentGroup, StudentGroupDto>(entity));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom dohvata grupe studenata.");
            }
        }

        public async Task<ActionResponse<List<StudentGroupDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<StudentGroup>()
                    .GetAll(includeProperties: "StudentsInGroup.Student");
                return await ActionResponse<List<StudentGroupDto>>
                    .ReturnSuccess(mapper.Map<List<StudentGroup>, List<StudentGroupDto>>(entities));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<List<StudentGroupDto>>.ReturnError("Greška prilikom dohvata svih grupa studenata.");
            }
        }

        public async Task<ActionResponse<PagedResult<StudentGroupDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<StudentGroup>()
                    .GetAllAsQueryable(includeProperties: "StudentsInGroup.Student").GetPaged(pagedRequest);

                var pagedResult = new PagedResult<StudentGroupDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<StudentGroup>, List<StudentGroupDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<StudentGroupDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, pagedRequest);
                return await ActionResponse<PagedResult<StudentGroupDto>>.ReturnError("Greška prilikom dohvata straničnih podataka grupe studenata.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> Insert(StudentGroupDto entityDto)
        {
            try
            {
                //List<Guid> teachers = new List<Guid>(entityDto.TeacherIds);
                var entityToAdd = mapper.Map<StudentGroupDto, StudentGroup>(entityDto);
                unitOfWork.GetGenericRepository<StudentGroup>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);
                //entityDto.TeacherIds = new List<Guid>(teachers);
                //if ((await ModifyExamTeachers(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupDto> actionResponse, out entityDto))
                //{
                //    return actionResponse;
                //}

                return await ActionResponse<StudentGroupDto>
                    .ReturnSuccess(mapper.Map<StudentGroup, StudentGroupDto>(entityToAdd));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom upisa grupe studenata.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> Update(StudentGroupDto entityDto)
        {
            try
            {
                var entityToUpdate = mapper.Map<StudentGroupDto, StudentGroup>(entityDto);
                unitOfWork.GetGenericRepository<StudentGroup>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<StudentGroupDto>
                    .ReturnSuccess(mapper.Map<StudentGroup, StudentGroupDto>(entityToUpdate));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom ažuriranja grupe studenata.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> Delete(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<StudentGroup>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<StudentGroupDto>.ReturnSuccess(null, "Brisanje grupe studenata uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom brisanja grupe studenata.");
            }
        }
    }
}
