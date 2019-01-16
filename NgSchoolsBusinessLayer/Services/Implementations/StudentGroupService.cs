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
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<int> studentIds = new List<int>(entityDto.StudentIds);
                    var entityToAdd = mapper.Map<StudentGroupDto, StudentGroup>(entityDto);
                    unitOfWork.GetGenericRepository<StudentGroup>().Add(entityToAdd);
                    unitOfWork.Save();
                    mapper.Map(entityToAdd, entityDto);
                    entityDto.StudentIds = new List<int>(studentIds);

                    if ((await ModifyStudentsInGroup(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupDto> response, out entityDto))
                    {
                        return response;
                    }

                    entityToAdd = unitOfWork.GetGenericRepository<StudentGroup>().FindBy(sg => sg.Id == entityDto.Id, includeProperties: "StudentsInGroups.Student");

                    scope.Complete();
                    return await ActionResponse<StudentGroupDto>
                        .ReturnSuccess(mapper.Map<StudentGroup, StudentGroupDto>(entityToAdd), "Grupa studenata uspješno upisana.");
                }
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom upisa grupe studenata.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> ModifyStudentsInGroup(StudentGroupDto studentGroup)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentGroup>()
                    .FindBy(e => e.Id == studentGroup.Id.Value, includeProperties: "StudentsInGroups.Student");
                var currentStudents = mapper.Map<List<StudentsInGroups>, List<StudentInGroupDto>>(entity.StudentsInGroups.ToList());

                var newStudents = studentGroup.StudentIds;

                var studentsToRemove = currentStudents.Where(cet => !newStudents.Contains(cet.StudentId.Value)).ToList();

                var studentsToAdd = newStudents
                    .Where(nt => !currentStudents.Select(uec => uec.StudentId).Contains(nt))
                    .Select(nt => new StudentInGroupDto { StudentId = nt, GroupId = studentGroup.Id })
                    .ToList();

                if ((await RemoveStudentsFromGroup(studentsToRemove))
                    .IsNotSuccess(out ActionResponse<List<StudentInGroupDto>> actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješna ažuriranje studenata u grupi.");
                }

                if ((await AddStudentsInGroup(studentsToAdd))
                    .IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješna promjena studenata u grupi.");
                }
                return await ActionResponse<StudentGroupDto>.ReturnSuccess(studentGroup, "Uspješno izmijenjeni studenti grupe.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, studentGroup);
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom ažuriranja grupe studenata.");
            }
        }

        public async Task<ActionResponse<List<StudentInGroupDto>>> RemoveStudentsFromGroup(List<StudentInGroupDto> studentsInGroup)
        {
            try
            {
                var response = await ActionResponse<List<StudentInGroupDto>>.ReturnSuccess(null, "Studenti uspješno maknuti iz grupe.");
                studentsInGroup.ForEach(async sig =>
                {
                    if ((await RemoveStudentFromGroup(sig))
                    .IsNotSuccess(out ActionResponse<StudentInGroupDto> actionResponse))
                    {
                        response = await ActionResponse<List<StudentInGroupDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, studentsInGroup);
                return await ActionResponse<List<StudentInGroupDto>>.ReturnError("Some sort of fuckup. Try again.");
            }
        }

        public async Task<ActionResponse<StudentInGroupDto>> RemoveStudentFromGroup(StudentInGroupDto studentInGroup)
        {
            try
            {
                unitOfWork.GetGenericRepository<StudentsInGroups>().Delete(studentInGroup.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<StudentInGroupDto>.ReturnSuccess(null, "Student upsješno izbrisan iz grupe.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, studentInGroup);
                return await ActionResponse<StudentInGroupDto>.ReturnError("Greška prilikom micanja studenta iz grupe.");
            }
        }

        public async Task<ActionResponse<List<StudentInGroupDto>>> AddStudentsInGroup(List<StudentInGroupDto> students)
        {
            try
            {
                var response = await ActionResponse<List<StudentInGroupDto>>.ReturnSuccess(null, "Studenti uspješno dodani u grupu.");
                students.ForEach(async s =>
                {
                    if ((await AddStudentInGroup(s))
                    .IsNotSuccess(out ActionResponse<StudentInGroupDto> actionResponse, out s))
                    {
                        response = await ActionResponse<List<StudentInGroupDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, students);
                return await ActionResponse<List<StudentInGroupDto>>.ReturnError("Greška prilikom dodavanja studenata u grupu.");
            }
        }

        public async Task<ActionResponse<StudentInGroupDto>> AddStudentInGroup(StudentInGroupDto student)
        {
            try
            {
                var entityToAdd = mapper.Map<StudentInGroupDto, StudentsInGroups>(student);
                unitOfWork.GetGenericRepository<StudentsInGroups>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<StudentInGroupDto>
                    .ReturnSuccess(mapper.Map<StudentsInGroups, StudentInGroupDto>(entityToAdd),
                    "Student uspješno dodan u grupu.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, student);
                return await ActionResponse<StudentInGroupDto>.ReturnError("Greška prilikom dodavanja studenta u grupu.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> Update(StudentGroupDto entityDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var entityToUpdate = mapper.Map<StudentGroupDto, StudentGroup>(entityDto);
                    unitOfWork.GetGenericRepository<StudentGroup>().Update(entityToUpdate);
                    unitOfWork.Save();

                    if ((await ModifyStudentsInGroup(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupDto> response, out entityDto))
                    {
                        return response;
                    }

                    scope.Complete();
                    return await ActionResponse<StudentGroupDto>
                        .ReturnSuccess(entityDto, "Grupa studenata uspješno izmijenjena.");
                }
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
