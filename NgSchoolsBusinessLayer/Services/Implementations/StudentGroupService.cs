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
        private readonly IExamCommissionService examCommissionService;
        private readonly IStudentService studentService;

        public StudentGroupService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork, IExamCommissionService examCommissionService,
            IStudentService studentService
            )
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
            this.examCommissionService = examCommissionService;
            this.studentService = studentService;
        }

        #endregion Ctors and Members

        #region Readers

        public async Task<ActionResponse<StudentGroupDto>> GetById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentGroup>()
                    .FindBy(c => c.Id == id,
                    includeProperties: "ClassLocation,StudentsInGroups.Student,SubjectTeachers,EducationLeader,ExamCommission,StudentGroupClassAttendances.StudentClassAttendances");
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
                    .GetAll(includeProperties: "ClassLocation,StudentsInGroups.Student,SubjectTeachers,EducationLeader,ExamCommission,StudentGroupClassAttendances.StudentClassAttendances");
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
                    .GetAllAsQueryable(includeProperties: "ClassLocation,StudentsInGroups.Student,SubjectTeachers,EducationLeader,ExamCommission,StudentGroupClassAttendances.StudentClassAttendances")
                    .GetPaged(pagedRequest);

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

        #endregion Readers

        #region Writers

        public async Task<ActionResponse<StudentGroupDto>> Insert(StudentGroupDto entityDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<int> studentIds = new List<int>(entityDto.StudentIds);
                    entityDto.StudentNames = null;

                    List<StudentGroupSubjectTeachersDto> subjectTeachers = entityDto.SubjectTeachers != null ?
                        new List<StudentGroupSubjectTeachersDto>(entityDto.SubjectTeachers) : new List<StudentGroupSubjectTeachersDto>();
                    entityDto.SubjectTeachers = null;

                    List<StudentGroupClassAttendanceDto> classAttendances = entityDto.StudentGroupClassAttendances != null ?
                        new List<StudentGroupClassAttendanceDto>(entityDto.StudentGroupClassAttendances) : new List<StudentGroupClassAttendanceDto>();

                    if ((await ModifyExamCommission(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupDto> commissionResponse, out entityDto))
                    {
                        return commissionResponse;
                    }

                    var entityToAdd = mapper.Map<StudentGroupDto, StudentGroup>(entityDto);
                    unitOfWork.GetGenericRepository<StudentGroup>().Add(entityToAdd);
                    unitOfWork.Save();
                    mapper.Map(entityToAdd, entityDto);
                    entityDto.StudentIds = new List<int>(studentIds);

                    if ((await ModifyStudentsInGroup(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupDto> response, out entityDto))
                    {
                        return response;
                    }

                    mapper.Map(entityDto, entityToAdd);
                    unitOfWork.Save();

                    entityDto.SubjectTeachers = subjectTeachers;
                    if ((await ModifySubjectTeachers(entityDto)).IsNotSuccess(out response, out entityDto))
                    {
                        return response;
                    }

                    if ((await GetById(entityToAdd.Id)).IsNotSuccess(out response, out entityDto))
                    {
                        return response;
                    }

                    if (!string.IsNullOrEmpty(entityDto.EnrolmentDate))
                    {
                        if ((await UpdateEnrolmentDate(entityDto)).IsNotSuccess(out response, out entityDto))
                        {
                            return response;
                        }
                    }

                    entityDto.StudentGroupClassAttendances = classAttendances;
                    if ((await ModifyClassAttendance(entityDto)).IsNotSuccess(out response, out entityDto))
                    {
                        return response;
                    }

                    scope.Complete();
                    return await ActionResponse<StudentGroupDto>.ReturnSuccess(entityDto, "Grupa studenata uspješno upisana.");
                }
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
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    List<int> studentIds = new List<int>(entityDto.StudentIds);
                    entityDto.StudentNames = null;

                    ExamCommissionDto examCommission = entityDto.ExamCommission;

                    List<StudentGroupClassAttendanceDto> classAttendances = entityDto.StudentGroupClassAttendances != null ?
                        new List<StudentGroupClassAttendanceDto>(entityDto.StudentGroupClassAttendances) : new List<StudentGroupClassAttendanceDto>();

                    List<StudentGroupSubjectTeachersDto> subjectTeachers = entityDto.SubjectTeachers != null ?
                        new List<StudentGroupSubjectTeachersDto>(entityDto.SubjectTeachers) : new List<StudentGroupSubjectTeachersDto>();
                    entityDto.SubjectTeachers = null;

                    var entityToUpdate = mapper.Map<StudentGroupDto, StudentGroup>(entityDto);
                    unitOfWork.GetGenericRepository<StudentGroup>().Update(entityToUpdate);
                    unitOfWork.Save();

                    mapper.Map(entityToUpdate, entityDto);
                    entityDto.ExamCommission = examCommission;
                    if ((await ModifyExamCommission(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupDto> commissionResponse, out entityDto))
                    {
                        return commissionResponse;
                    }

                    mapper.Map(entityDto, entityToUpdate);
                    unitOfWork.Save();

                    mapper.Map(entityToUpdate, entityDto);
                    entityDto.StudentIds = new List<int>(studentIds);

                    if ((await ModifyStudentsInGroup(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupDto> response, out entityDto))
                    {
                        return response;
                    }

                    if ((await ModifySubjectTeachers(entityDto)).IsNotSuccess(out response, out entityDto))
                    {
                        return response;
                    }

                    if ((await GetById(entityToUpdate.Id)).IsNotSuccess(out response, out entityDto))
                    {
                        return response;
                    }

                    if (!string.IsNullOrEmpty(entityDto.EnrolmentDate))
                    {
                        if ((await UpdateEnrolmentDate(entityDto)).IsNotSuccess(out response, out entityDto))
                        {
                            return response;
                        }
                    }

                    entityDto.StudentGroupClassAttendances = classAttendances;
                    if ((await ModifyClassAttendance(entityDto)).IsNotSuccess(out response, out entityDto))
                    {
                        return response;
                    }

                    scope.Complete();
                    return await ActionResponse<StudentGroupDto>.ReturnSuccess(entityDto, "Grupa studenata uspješno izmijenjena.");
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

        #endregion Writers

        #region Students in Group

        public async Task<ActionResponse<StudentGroupDto>> UpdateEnrolmentDate(StudentGroupDto entityDto)
        {
            try
            {
                var response = await ActionResponse<StudentGroupDto>.ReturnSuccess(entityDto, "Datumi upisa za studente uspješno izmijenjeni.");
                entityDto.Students.ForEach(async studentDto =>
                {
                    studentDto.EnrolmentDate = entityDto.EnrolmentDate;
                    if ((await studentService.UpdateEnrollmentDate(studentDto)).IsNotSuccess(out ActionResponse<StudentDto> sUpResponse, out studentDto))
                    {
                        response = await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom izmjene datuima upisa za studente u grupi.");
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentGroupDto>.ReturnError($"Greška prilikom ažuriranja datuma upisa za studente u grupi.");
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

        #endregion Students in Group

        #region Exam Commission

        public async Task<ActionResponse<StudentGroupDto>> ModifyExamCommission(StudentGroupDto entityDto)
        {
            try
            {
                if (entityDto.ExamCommissionId.HasValue)
                {
                    if ((await examCommissionService.Update(entityDto.ExamCommission))
                        .IsNotSuccess(out ActionResponse<ExamCommissionDto> examCommissionResponse, out ExamCommissionDto examCommission))
                    {
                        return await ActionResponse<StudentGroupDto>.ReturnError(examCommissionResponse.Message);
                    }
                    entityDto.ExamCommission = examCommission;
                }
                else
                {
                    if (entityDto.Id.HasValue)
                    {
                        var entityToCheck = unitOfWork.GetGenericRepository<StudentGroup>().FindBy(sg => sg.Id == entityDto.Id);
                        if (entityToCheck.ExamCommissionId.HasValue)
                        {
                            if ((await examCommissionService.Delete(entityToCheck.ExamCommissionId.Value))
                                .IsNotSuccess(out ActionResponse<ExamCommissionDto> deleteResponse))
                            {
                                return await ActionResponse<StudentGroupDto>.ReturnError(deleteResponse.Message);
                            }
                        }
                        else
                        {
                            if ((await examCommissionService.Insert(entityDto.ExamCommission))
                                .IsNotSuccess(out ActionResponse<ExamCommissionDto> insertResponse, out ExamCommissionDto examCommission))
                            {
                                return await ActionResponse<StudentGroupDto>.ReturnError(insertResponse.Message);
                            }
                            entityDto.ExamCommission = examCommission;
                            entityDto.ExamCommissionId = examCommission.Id;
                        }
                    }
                    else
                    {
                        if (entityDto.ExamCommission != null)
                        {
                            if ((await examCommissionService.Insert(entityDto.ExamCommission))
                                .IsNotSuccess(out ActionResponse<ExamCommissionDto> insertResponse, out ExamCommissionDto examCommission))
                            {
                                return await ActionResponse<StudentGroupDto>.ReturnError(insertResponse.Message);
                            }
                            entityDto.ExamCommission = examCommission;
                            entityDto.ExamCommissionId = examCommission.Id;
                        }
                    }
                }
                return await ActionResponse<StudentGroupDto>.ReturnSuccess(entityDto, "Ispitna komisija za grupu studenata uspješno izmijenjena.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentGroupDto>.ReturnError($"Greška prilikom ažuriranja ispitne komisije za grupu studenata.");
            }
        }

        #endregion Exam Commission

        #region SubjectTeachers

        public async Task<ActionResponse<StudentGroupDto>> ModifySubjectTeachers(StudentGroupDto studentGroup)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentGroup>()
                    .FindBy(e => e.Id == studentGroup.Id.Value, includeProperties: "SubjectTeachers");
                var currentSubjectTeachers = mapper.Map<List<StudentGroupSubjectTeachers>, List<StudentGroupSubjectTeachersDto>>(entity.SubjectTeachers.ToList());

                var newSubjectTeachers = studentGroup.SubjectTeachers != null ? studentGroup.SubjectTeachers : new List<StudentGroupSubjectTeachersDto>();

                var teachersToRemove = currentSubjectTeachers
                    .Where(cet => newSubjectTeachers
                        .All(nst => nst.SubjectId != cet.SubjectId || nst.TeacherId != cet.TeacherId)).ToList();

                var teachersToAdd = newSubjectTeachers
                    .Where(nt => currentSubjectTeachers
                    .All(nst => nst.SubjectId != nt.SubjectId || nst.TeacherId != nt.TeacherId))
                    .Select(nst => new StudentGroupSubjectTeachersDto
                    {
                        StudentGroupId = entity.Id,
                        SubjectId = nst.SubjectId,
                        TeacherId = nst.TeacherId
                    })
                    .ToList();

                if ((await RemoveSubjectTeachers(teachersToRemove))
                    .IsNotSuccess(out ActionResponse<List<StudentGroupSubjectTeachersDto>> actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješno ažuriranje učitelja predmeta u grupi.");
                }

                if ((await AddSubjectTeachers(teachersToAdd)).IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješno ažuriranje učitelja predmeta u grupi.");
                }
                return await ActionResponse<StudentGroupDto>.ReturnSuccess(studentGroup, "Ažuriranje učitelja predmeta u grupi uspješno.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, studentGroup);
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom ažuriranja učitelja predmeta.");
            }
        }

        public async Task<ActionResponse<List<StudentGroupSubjectTeachersDto>>> RemoveSubjectTeachers(List<StudentGroupSubjectTeachersDto> sTeachers)
        {
            try
            {
                var response = await ActionResponse<List<StudentGroupSubjectTeachersDto>>.ReturnSuccess(null, "Studenti uspješno maknuti iz grupe.");
                sTeachers.ForEach(async sig =>
                {
                    if ((await RemoveSubjectTeacher(sig))
                        .IsNotSuccess(out ActionResponse<StudentGroupSubjectTeachersDto> actionResponse))
                    {
                        response = await ActionResponse<List<StudentGroupSubjectTeachersDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, sTeachers);
                return await ActionResponse<List<StudentGroupSubjectTeachersDto>>.ReturnError("Greška prilikom micanja učitelja predmeta.");
            }
        }

        public async Task<ActionResponse<StudentGroupSubjectTeachersDto>> RemoveSubjectTeacher(StudentGroupSubjectTeachersDto subjectTeacher)
        {
            try
            {
                unitOfWork.GetGenericRepository<StudentGroupSubjectTeachers>().Delete(subjectTeacher.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<StudentGroupSubjectTeachersDto>.ReturnSuccess(null, "Učitelj predmeta uspješno izbrisan iz grupe.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, subjectTeacher);
                return await ActionResponse<StudentGroupSubjectTeachersDto>.ReturnError("Greška prilikom micanja učitelja predmeta iz grupe.");
            }
        }

        public async Task<ActionResponse<List<StudentGroupSubjectTeachersDto>>> AddSubjectTeachers(List<StudentGroupSubjectTeachersDto> students)
        {
            try
            {
                var response = await ActionResponse<List<StudentGroupSubjectTeachersDto>>.ReturnSuccess(null, "Učitelji predmeta uspješno dodani u grupu.");
                students.ForEach(async s =>
                {
                    if ((await AddSubjectTeacher(s))
                    .IsNotSuccess(out ActionResponse<StudentGroupSubjectTeachersDto> actionResponse, out s))
                    {
                        response = await ActionResponse<List<StudentGroupSubjectTeachersDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, students);
                return await ActionResponse<List<StudentGroupSubjectTeachersDto>>.ReturnError("Greška prilikom dodavanja učitelja predmeta u grupu.");
            }
        }

        public async Task<ActionResponse<StudentGroupSubjectTeachersDto>> AddSubjectTeacher(StudentGroupSubjectTeachersDto student)
        {
            try
            {
                var entityToAdd = mapper.Map<StudentGroupSubjectTeachersDto, StudentGroupSubjectTeachers>(student);
                unitOfWork.GetGenericRepository<StudentGroupSubjectTeachers>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<StudentGroupSubjectTeachersDto>
                    .ReturnSuccess(mapper.Map<StudentGroupSubjectTeachers, StudentGroupSubjectTeachersDto>(entityToAdd),
                    "Učitelj predmeta uspješno dodan u grupu.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, student);
                return await ActionResponse<StudentGroupSubjectTeachersDto>.ReturnError("Greška prilikom dodavanja učitelja predmeta u grupu.");
            }
        }

        #endregion SubjectTeachers

        #region Class Attendance

        public async Task<ActionResponse<StudentGroupDto>> ModifyClassAttendance(StudentGroupDto entityDto)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentGroup>()
                    .FindBy(p => p.Id == entityDto.Id,
                    includeProperties: "StudentGroupClassAttendances.StudentClassAttendances");

                var currentDays = mapper.Map<List<StudentGroupClassAttendance>, List<StudentGroupClassAttendanceDto>>(entity.StudentGroupClassAttendances.ToList());

                var newDays = entityDto.StudentGroupClassAttendances;

                var daysToRemove = currentDays
                    .Where(cd => !newDays.Select(nd => nd.Id).Contains(cd.Id)).ToList();

                var daysToAdd = newDays
                    .Where(nt => !currentDays.Select(cd => cd.Id).Contains(nt.Id))
                    .Select(nt => {
                        nt.StudentGroupId = entityDto.Id;
                        return nt;
                    })
                    .ToList();

                var daysToModify = newDays.Where(cd => currentDays.Select(nd => nd.Id).Contains(cd.Id)).ToList();

                if ((await RemoveDaysFromAttendance(daysToRemove))
                    .IsNotSuccess(out ActionResponse<List<StudentGroupClassAttendanceDto>> actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješno ažuriranje dana u praćenju nastave.");
                }

                if ((await AddDaysToAttendance(daysToAdd)).IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješno ažuriranje dana u praćenju nastave.");
                }

                if ((await ModifyDaysInAttendance(daysToModify)).IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješno ažuriranje dana u praćenju nastave.");
                }

                return await ActionResponse<StudentGroupDto>.ReturnSuccess(entityDto, "Uspješno izmijenjeni dani u praćenju nastave.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentGroupDto>.ReturnError($"Greška prilikom upisa pohađanja nastave.");
            }
        }

        #region Remove Days

        public async Task<ActionResponse<List<StudentGroupClassAttendanceDto>>> RemoveDaysFromAttendance(List<StudentGroupClassAttendanceDto> days)
        {
            try
            {
                var response = await ActionResponse<List<StudentGroupClassAttendanceDto>>.ReturnSuccess(null, "Dani uspješno maknuti iz plana.");
                days.ForEach(async pd =>
                {
                    if ((await RemoveDayFromAttendance(pd))
                    .IsNotSuccess(out ActionResponse<StudentGroupClassAttendanceDto> actionResponse))
                    {
                        response = await ActionResponse<List<StudentGroupClassAttendanceDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, days);
                return await ActionResponse<List<StudentGroupClassAttendanceDto>>.ReturnError("Greška prilikom micanja dana iz plana.");
            }
        }

        public async Task<ActionResponse<StudentGroupClassAttendanceDto>> RemoveDayFromAttendance(StudentGroupClassAttendanceDto entityDto)
        {
            try
            {
                unitOfWork.GetGenericRepository<StudentGroupClassAttendance>().Delete(entityDto.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<StudentGroupClassAttendanceDto>.ReturnSuccess(null, "Dan uspješno izbrisan iz praćenja nastave.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentGroupClassAttendanceDto>.ReturnError("Greška prilikom micanja dana iz praćenja nastave.");
            }
        }

        #endregion Remove Days

        #region Add Days

        public async Task<ActionResponse<List<StudentGroupClassAttendanceDto>>> AddDaysToAttendance(List<StudentGroupClassAttendanceDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<StudentGroupClassAttendanceDto>>.ReturnSuccess(null, "Dani uspješno dodani u plan.");
                entityDtos.ForEach(async pd =>
                {
                    if ((await AddDayToAttendance(pd))
                        .IsNotSuccess(out ActionResponse<StudentGroupClassAttendanceDto> actionResponse, out pd))
                    {
                        response = await ActionResponse<List<StudentGroupClassAttendanceDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<StudentGroupClassAttendanceDto>>.ReturnError("Greška prilikom dodavanja dana u praćenje nastave.");
            }
        }

        public async Task<ActionResponse<StudentGroupClassAttendanceDto>> AddDayToAttendance(StudentGroupClassAttendanceDto entityDto)
        {
            try
            {
                List<StudentClassAttendanceDto> studentAttendance = new List<StudentClassAttendanceDto>(entityDto.StudentClassAttendances);

                var entityToAdd = mapper.Map<StudentGroupClassAttendanceDto, StudentGroupClassAttendance>(entityDto);
                unitOfWork.GetGenericRepository<StudentGroupClassAttendance>().Add(entityToAdd);
                unitOfWork.Save();
                mapper.Map(entityToAdd, entityDto);

                entityDto.StudentClassAttendances = studentAttendance
                    .Select(pd =>
                    {
                        pd.StudentGroupClassAttendanceId = entityToAdd.Id;
                        return pd;
                    }).ToList();

                if ((await ModifyStudentsAttendance(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupClassAttendanceDto> response, out entityDto))
                {
                    return response;
                }

                return await ActionResponse<StudentGroupClassAttendanceDto>.ReturnSuccess(entityDto, "Dan praćenja nastave uspješno unesen.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentGroupClassAttendanceDto>.ReturnError("Greška prilikom dodavanja dana praćenja u nastavu.");
            }
        }

        #endregion Add Days

        #region Update Days

        public async Task<ActionResponse<List<StudentGroupClassAttendanceDto>>> ModifyDaysInAttendance(List<StudentGroupClassAttendanceDto> planDays)
        {
            try
            {
                var response = await ActionResponse<List<StudentGroupClassAttendanceDto>>.ReturnSuccess(null, "Dani praćenja nastave uspješno izmijenjeni.");
                planDays.ForEach(async pd =>
                {
                    if ((await ModifyDayInAttendance(pd))
                    .IsNotSuccess(out ActionResponse<StudentGroupClassAttendanceDto> actionResponse, out pd))
                    {
                        response = await ActionResponse<List<StudentGroupClassAttendanceDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, planDays);
                return await ActionResponse<List<StudentGroupClassAttendanceDto>>.ReturnError("Greška prilikom dodavanja dana u plan.");
            }
        }

        public async Task<ActionResponse<StudentGroupClassAttendanceDto>> ModifyDayInAttendance(StudentGroupClassAttendanceDto entityDto)
        {
            try
            {
                List<StudentClassAttendanceDto> studentAttendance = new List<StudentClassAttendanceDto>(entityDto.StudentClassAttendances);

                var entityToUpdate = unitOfWork.GetGenericRepository<StudentGroupClassAttendance>().FindSingle(entityDto.Id.Value);
                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<StudentGroupClassAttendance>().Update(entityToUpdate);
                unitOfWork.Save();

                entityDto.StudentClassAttendances = studentAttendance
                    .Select(pd =>
                    {
                        pd.StudentGroupClassAttendanceId = entityToUpdate.Id;
                        return pd;
                    }).ToList();

                if ((await ModifyStudentsAttendance(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupClassAttendanceDto> response, out entityDto))
                {
                    return response;
                }

                return await ActionResponse<StudentGroupClassAttendanceDto>
                    .ReturnSuccess(mapper.Map(entityToUpdate, entityDto));
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex);
                return await ActionResponse<StudentGroupClassAttendanceDto>.ReturnError("Greška prilikom ažuriranja dana praćenja nastave.");
            }
        }

        #endregion Update Days

        #region StudentAttendance

        public async Task<ActionResponse<StudentGroupClassAttendanceDto>> ModifyStudentsAttendance(StudentGroupClassAttendanceDto entityDto)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentGroupClassAttendance>()
                    .FindBy(p => p.Id == entityDto.Id,
                    includeProperties: "StudentClassAttendances");

                var currentStudents = mapper.Map<List<StudentClassAttendance>, List<StudentClassAttendanceDto>>(entity.StudentClassAttendances.ToList());

                var newStudents = entityDto.StudentClassAttendances;

                var studentsToAdd = newStudents
                    .Where(nt => !currentStudents.Select(cd => cd.Id).Contains(nt.Id))
                    .Select(ca =>
                    {
                        ca.StudentGroupClassAttendanceId = entityDto.Id;
                        return ca;
                    }).ToList();

                var studentsToModify = newStudents
                    .Where(cd => currentStudents.Select(nd => nd.Id).Contains(cd.Id))
                    .Select(ca =>
                    {
                        ca.StudentGroupClassAttendanceId = entityDto.Id;
                        return ca;
                    }).ToList();

                if ((await AddAttendances(studentsToAdd)).IsNotSuccess(out ActionResponse<List<StudentClassAttendanceDto>> actionResponse))
                {
                    return await ActionResponse<StudentGroupClassAttendanceDto>.ReturnError("Neuspješno ažuriranje praćenja nastave za studente.");
                }

                if ((await ModifyAttendances(studentsToModify)).IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<StudentGroupClassAttendanceDto>.ReturnError("Neuspješno ažuriranje praćenja nastave za studente.");
                }

                return await ActionResponse<StudentGroupClassAttendanceDto>.ReturnSuccess(entityDto, "Uspješno izmijenjeno praćenje nastave za studenta.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentGroupClassAttendanceDto>.ReturnError($"Greška prilikom upisa pohađanja nastave.");
            }
        }


        public async Task<ActionResponse<List<StudentClassAttendanceDto>>> AddAttendances(List<StudentClassAttendanceDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<StudentClassAttendanceDto>>.ReturnSuccess(entityDtos, "Pohađanje nastave uspješno dodano u praćenje nastave.");
                entityDtos.ForEach(async pdt =>
                {
                    if ((await AddAttendance(pdt))
                        .IsNotSuccess(out ActionResponse<StudentClassAttendanceDto> insertResponse, out pdt))
                    {
                        response = await ActionResponse<List<StudentClassAttendanceDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<StudentClassAttendanceDto>>.ReturnError("Greška prilikom dodavanja podataka pohađanja u praćenje nastave.");
            }
        }

        public async Task<ActionResponse<StudentClassAttendanceDto>> AddAttendance(StudentClassAttendanceDto entityDto)
        {
            try
            {
                var entityToAdd = mapper.Map<StudentClassAttendanceDto, StudentClassAttendance>(entityDto);
                unitOfWork.GetGenericRepository<StudentClassAttendance>().Add(entityToAdd);
                unitOfWork.Save();
                return await ActionResponse<StudentClassAttendanceDto>
                    .ReturnSuccess(mapper.Map(entityToAdd, entityDto), "Zapis prisustvovanja uspješno dodan.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentClassAttendanceDto>.ReturnError("Greška prilikom dodavanja zapisa prisustvovanja.");
            }
        }

        public async Task<ActionResponse<List<StudentClassAttendanceDto>>> ModifyAttendances(List<StudentClassAttendanceDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<StudentClassAttendanceDto>>.ReturnSuccess(null, "Zapisi prisustvovanja uspješno izmijenjeni.");
                entityDtos.ForEach(async pds =>
                {
                    if ((await ModifyAttendance(pds))
                    .IsNotSuccess(out ActionResponse<StudentClassAttendanceDto> insertResponse, out pds))
                    {
                        response = await ActionResponse<List<StudentClassAttendanceDto>>.ReturnError(insertResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDtos);
                return await ActionResponse<List<StudentClassAttendanceDto>>.ReturnError("Greška prilikom ažuriranja tema u predmetu planskog dana.");
            }
        }

        public async Task<ActionResponse<StudentClassAttendanceDto>> ModifyAttendance(StudentClassAttendanceDto entityDto)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<StudentClassAttendance>().FindSingle(entityDto.Id.Value);
                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<StudentClassAttendance>().Update(entityToUpdate);
                unitOfWork.Save();
                return await ActionResponse<StudentClassAttendanceDto>
                    .ReturnSuccess(mapper.Map(entityToUpdate, entityDto), "Zapis prisustva uspješno izmijenjen.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<StudentClassAttendanceDto>.ReturnError("Greška prilikom ažuriranja teme u danu plana.");
            }
        }

        #endregion StudentAttendance

        #endregion Class Attendance
    }
}
