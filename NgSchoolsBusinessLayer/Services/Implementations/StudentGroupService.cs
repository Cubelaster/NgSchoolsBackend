using AutoMapper;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Models.ViewModels;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Enums;
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IExamCommissionService examCommissionService;
        private readonly IStudentService studentService;
        private readonly string includeProperties = "ClassLocation,StudentsInGroups.Student,StudentsInGroups.Student.StudentsInGroups.StudentRegisterEntry," +
            "StudentsInGroups.Employer,SubjectTeachers,EducationLeader.UserDetails.Signature,Director.UserDetails.Signature,ExamCommission.UserExamCommissions.User.UserDetails," +
            "PracticalExamCommission.UserExamCommissions.User.UserDetails,StudentGroupClassAttendances.StudentClassAttendances," +
            "StudentsInGroups.Student.AddressCity,StudentsInGroups.Student.AddressCountry,StudentsInGroups.Student.AddressRegion," +
            "StudentsInGroups.Student.CountryOfBirth,StudentsInGroups.Student.RegionOfBirth,StudentsInGroups.Student.CityOfBirth," +
            "StudentsInGroups.Student.MunicipalityOfBirth,StudentsInGroups.Student.AddressMunicipality," +
            "StudentsInGroups.StudentRegisterEntry";
        private readonly string includeCombinedProperties = "StudentGroups.ClassLocation,StudentGroups.StudentsInGroups.Student," +
            "StudentGroups.StudentsInGroups.Employer,StudentGroups.SubjectTeachers,StudentGroups.EducationLeader.UserDetails.Signature," +
            "StudentGroups.Director.UserDetails.Signature,StudentGroups.ExamCommission.UserExamCommissions.User.UserDetails," +
            "StudentGroups.PracticalExamCommission.UserExamCommissions.User.UserDetails,StudentGroups.StudentGroupClassAttendances.StudentClassAttendances," +
            "StudentGroups.Program.EducationGroup,StudentGroups.StudentsInGroups.Student.AddressCity,StudentGroups.StudentsInGroups.Student.AddressCountry," +
            "StudentGroups.StudentsInGroups.Student.AddressRegion,StudentGroups.StudentsInGroups.Student.CountryOfBirth," +
            "StudentGroups.StudentsInGroups.Student.RegionOfBirth,StudentGroups.StudentsInGroups.Student.CityOfBirth," +
            "StudentGroups.StudentsInGroups.Student.MunicipalityOfBirth,StudentGroups.StudentsInGroups.Student.AddressMunicipality," +
            "StudentGroups.StudentsInGroups.StudentRegisterEntry";

        public StudentGroupService(IMapper mapper,
            IUnitOfWork unitOfWork, IExamCommissionService examCommissionService,
            IStudentService studentService)
        {
            this.mapper = mapper;
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
                    .FindBy(c => c.Id == id, includeProperties: includeProperties);

                return await ActionResponse<StudentGroupDto>
                    .ReturnSuccess(mapper.Map<StudentGroupDto>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom dohvata grupe studenata.");
            }
        }

        public async Task<ActionResponse<List<StudentGroupDto>>> GetAll()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<StudentGroup>()
                    .GetAll(includeProperties: includeProperties);
                return await ActionResponse<List<StudentGroupDto>>
                    .ReturnSuccess(mapper.Map<List<StudentGroup>, List<StudentGroupDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentGroupDto>>.ReturnError("Greška prilikom dohvata svih grupa studenata.");
            }
        }

        public async Task<ActionResponse<int>> GetTotalNumber()
        {
            try
            {
                return await ActionResponse<int>.ReturnSuccess(unitOfWork.GetGenericRepository<StudentGroup>().GetAllAsQueryable().Count());
            }
            catch (Exception)
            {
                return await ActionResponse<int>.ReturnError("Greška prilikom dohvata broja grupa studenata.");
            }
        }

        public async Task<ActionResponse<PagedResult<StudentGroupDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<StudentGroup>()
                    .GetAllAsQueryable(includeProperties: includeProperties)
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
            catch (Exception)
            {
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
                    entityDto.StudentNames = null;

                    List<StudentInGroupDto> studentsInGroup = entityDto.StudentsInGroup != null ?
                        new List<StudentInGroupDto>(entityDto.StudentsInGroup) : new List<StudentInGroupDto>();
                    entityDto.StudentsInGroup = null;

                    List<StudentGroupSubjectTeachersDto> subjectTeachers = entityDto.SubjectTeachers != null ?
                        new List<StudentGroupSubjectTeachersDto>(entityDto.SubjectTeachers) : new List<StudentGroupSubjectTeachersDto>();
                    entityDto.SubjectTeachers = null;

                    List<StudentGroupClassAttendanceDto> classAttendances = entityDto.StudentGroupClassAttendances != null ?
                        new List<StudentGroupClassAttendanceDto>(entityDto.StudentGroupClassAttendances) : new List<StudentGroupClassAttendanceDto>();

                    ExamCommissionDto practicalExamCommission = entityDto.PracticalExamCommission;

                    if ((await ModifyExamCommission(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupDto> commissionResponse, out entityDto))
                    {
                        return commissionResponse;
                    }

                    entityDto.PracticalExamCommission = practicalExamCommission;
                    if ((await ModifyPracticalExamCommission(entityDto)).IsNotSuccess(out commissionResponse, out entityDto))
                    {
                        return commissionResponse;
                    }

                    var entityToAdd = mapper.Map<StudentGroupDto, StudentGroup>(entityDto);
                    unitOfWork.GetGenericRepository<StudentGroup>().Add(entityToAdd);
                    unitOfWork.Save();
                    mapper.Map(entityToAdd, entityDto);

                    entityDto.StudentsInGroup = new List<StudentInGroupDto>(studentsInGroup);

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

                    if ((await GetById(entityDto.Id.Value)).IsNotSuccess(out response, out entityDto))
                    {
                        return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom čitanja novounesenih podataka.");
                    }

                    scope.Complete();
                    return await ActionResponse<StudentGroupDto>.ReturnSuccess(entityDto, "Grupa studenata uspješno upisana.");
                }
            }
            catch (Exception)
            {
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom upisa grupe studenata.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> Update(StudentGroupDto entityDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    entityDto.StudentNames = null;

                    List<StudentInGroupDto> studentsInGroup = entityDto.StudentsInGroup != null ?
                        new List<StudentInGroupDto>(entityDto.StudentsInGroup) : new List<StudentInGroupDto>();
                    entityDto.StudentsInGroup = null;

                    ExamCommissionDto examCommission = entityDto.ExamCommission;
                    ExamCommissionDto practicalExamCommission = entityDto.PracticalExamCommission;

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

                    entityDto.PracticalExamCommission = practicalExamCommission;
                    if ((await ModifyPracticalExamCommission(entityDto)).IsNotSuccess(out commissionResponse, out entityDto))
                    {
                        return commissionResponse;
                    }

                    mapper.Map(entityDto, entityToUpdate);
                    unitOfWork.Save();

                    mapper.Map(entityToUpdate, entityDto);
                    entityDto.StudentsInGroup = new List<StudentInGroupDto>(studentsInGroup);

                    if ((await ModifyStudentsInGroup(entityDto)).IsNotSuccess(out ActionResponse<StudentGroupDto> response, out entityDto))
                    {
                        return response;
                    }

                    entityDto.SubjectTeachers = subjectTeachers;
                    if ((await ModifySubjectTeachers(entityDto)).IsNotSuccess(out response, out entityDto))
                    {
                        return response;
                    }

                    if ((await GetById(entityToUpdate.Id)).IsNotSuccess(out response, out entityDto))
                    {
                        return response;
                    }

                    if (!string.IsNullOrEmpty(entityDto.EnrolmentDate) && entityDto.Students.Any(s => s.EnrolmentDate == null))
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

                    if ((await GetById(entityDto.Id.Value)).IsNotSuccess(out response, out entityDto))
                    {
                        return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom čitanja novounesenih podataka.");
                    }

                    scope.Complete();
                    return await ActionResponse<StudentGroupDto>.ReturnSuccess(entityDto, "Grupa studenata uspješno izmijenjena.");
                }
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
                entityDto.Students.Where(s => s.EnrolmentDate == null)
                    .ToList().ForEach(async studentDto =>
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
            catch (Exception)
            {
                return await ActionResponse<StudentGroupDto>.ReturnError($"Greška prilikom ažuriranja datuma upisa za studente u grupi.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> ModifyStudentsInGroup(StudentGroupDto studentGroup)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<StudentGroup>()
                    .FindBy(e => e.Id == studentGroup.Id.Value, includeProperties: "StudentsInGroups");
                var currentStudents = mapper.Map<List<StudentsInGroups>, List<StudentInGroupDto>>(entity.StudentsInGroups.ToList());

                var newStudents = studentGroup.StudentsInGroup;

                var studentsToRemove = currentStudents.Where(cet => !newStudents.Select(ns => ns.Id).Contains(cet.Id)).ToList();

                var studentsToModify = newStudents.Where(ns => currentStudents.Select(cs => cs.Id).Contains(ns.Id)).ToList();

                var studentsToAdd = newStudents
                    .Where(nt => !currentStudents.Select(uec => uec.Id).Contains(nt.Id))
                    .Select(nt =>
                    {
                        nt.GroupId = studentGroup.Id;
                        return nt;
                    }).ToList();

                if ((await RemoveStudentsFromGroup(studentsToRemove))
                    .IsNotSuccess(out ActionResponse<List<StudentInGroupDto>> actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješna ažuriranje studenata u grupi.");
                }

                if ((await ModifyStudentsInGroup(studentsToModify))
                    .IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješno ažuriranje podataka studenata u grupi.");
                }

                if ((await AddStudentsInGroup(studentsToAdd))
                    .IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješna promjena studenata u grupi.");
                }
                return await ActionResponse<StudentGroupDto>.ReturnSuccess(studentGroup, "Uspješno izmijenjeni studenti grupe.");
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<List<StudentInGroupDto>>.ReturnError("Greška prilikom micanja studenata iz grupe.");
            }
        }

        public async Task<ActionResponse<StudentInGroupDto>> RemoveStudentFromGroup(StudentInGroupDto studentInGroup)
        {
            try
            {
                unitOfWork.GetGenericRepository<StudentsInGroups>().Delete(studentInGroup.Id.Value);
                unitOfWork.Save();

                if ((await GetAllAttendanceForStudentAndGroup(studentInGroup.StudentId.Value, studentInGroup.GroupId.Value))
                    .IsNotSuccess(out ActionResponse<List<StudentClassAttendanceDto>> attendancesResponse, out List<StudentClassAttendanceDto> attendances))
                {
                    return await ActionResponse<StudentInGroupDto>.ReturnError(attendancesResponse.Message);
                }

                if ((await RemoveAttendances(attendances))
                    .IsNotSuccess(out ActionResponse<List<StudentClassAttendanceDto>> removeAttendanceResponse))
                {
                    return await ActionResponse<StudentInGroupDto>.ReturnError(removeAttendanceResponse.Message);
                }

                return await ActionResponse<StudentInGroupDto>.ReturnSuccess(null, "Student uspješno izbrisan iz grupe i podaci o evidencijama nastave također maknuti.");
            }
            catch (Exception)
            {
                return await ActionResponse<StudentInGroupDto>.ReturnError("Greška prilikom micanja studenta iz grupe.");
            }
        }

        public async Task<ActionResponse<List<StudentInGroupDto>>> AddStudentsInGroup(List<StudentInGroupDto> students)
        {
            try
            {
                var response = await ActionResponse<List<StudentInGroupDto>>.ReturnSuccess(students, "Studenti uspješno dodani u grupu.");
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
            catch (Exception)
            {
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
                mapper.Map(entityToAdd, student);
                return await ActionResponse<StudentInGroupDto>.ReturnSuccess(student, "Student uspješno dodan u grupu.");
            }
            catch (Exception)
            {
                return await ActionResponse<StudentInGroupDto>.ReturnError("Greška prilikom dodavanja studenta u grupu.");
            }
        }

        public async Task<ActionResponse<List<StudentInGroupDto>>> ModifyStudentsInGroup(List<StudentInGroupDto> students)
        {
            try
            {
                var response = await ActionResponse<List<StudentInGroupDto>>.ReturnSuccess(students, "Podaci o studentima unutar grupe uspješno izmijenjeni.");
                students.ForEach(async s =>
                {
                    if ((await ModifyStudentInGroup(s))
                    .IsNotSuccess(out ActionResponse<StudentInGroupDto> actionResponse, out s))
                    {
                        response = await ActionResponse<List<StudentInGroupDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentInGroupDto>>.ReturnError("Greška prilikom ažuriranja studenata u grupu.");
            }
        }

        private async Task<ActionResponse<StudentInGroupDto>> ModifyStudentInGroup(StudentInGroupDto entityDto)
        {
            try
            {
                var entityToUpdate = unitOfWork.GetGenericRepository<StudentsInGroups>().FindSingle(entityDto.Id.Value);
                mapper.Map(entityDto, entityToUpdate);
                unitOfWork.GetGenericRepository<StudentsInGroups>().Update(entityToUpdate);
                unitOfWork.Save();
                mapper.Map(entityToUpdate, entityDto);
                return await ActionResponse<StudentInGroupDto>.ReturnSuccess(entityDto, "Podaci o studentu unutar grupe ažurirani.");
            }
            catch (Exception)
            {
                return await ActionResponse<StudentInGroupDto>.ReturnError("Greška prilikom izmjene podataka o studentu unutar grupe.");
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
                        else if (entityDto.ExamCommission != null)
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
            catch (Exception)
            {
                return await ActionResponse<StudentGroupDto>.ReturnError($"Greška prilikom ažuriranja ispitne komisije za grupu studenata.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> ModifyPracticalExamCommission(StudentGroupDto entityDto)
        {
            try
            {
                if (entityDto.PracticalExamCommissionId.HasValue)
                {
                    if ((await examCommissionService.Update(entityDto.PracticalExamCommission))
                        .IsNotSuccess(out ActionResponse<ExamCommissionDto> examCommissionResponse, out ExamCommissionDto examCommission))
                    {
                        return await ActionResponse<StudentGroupDto>.ReturnError(examCommissionResponse.Message);
                    }
                    entityDto.PracticalExamCommission = examCommission;
                }
                else
                {
                    if (entityDto.Id.HasValue)
                    {
                        var entityToCheck = unitOfWork.GetGenericRepository<StudentGroup>().FindBy(sg => sg.Id == entityDto.Id);
                        if (entityToCheck.PracticalExamCommissionId.HasValue)
                        {
                            if ((await examCommissionService.Delete(entityToCheck.PracticalExamCommissionId.Value))
                                .IsNotSuccess(out ActionResponse<ExamCommissionDto> deleteResponse))
                            {
                                return await ActionResponse<StudentGroupDto>.ReturnError(deleteResponse.Message);
                            }
                        }
                        else if (entityDto.PracticalExamCommission != null)
                        {
                            if ((await examCommissionService.Insert(entityDto.PracticalExamCommission))
                                .IsNotSuccess(out ActionResponse<ExamCommissionDto> insertResponse, out ExamCommissionDto examCommission))
                            {
                                return await ActionResponse<StudentGroupDto>.ReturnError(insertResponse.Message);
                            }
                            entityDto.PracticalExamCommission = examCommission;
                            entityDto.PracticalExamCommissionId = examCommission.Id;
                        }
                    }
                    else
                    {
                        if (entityDto.PracticalExamCommission != null)
                        {
                            if ((await examCommissionService.Insert(entityDto.PracticalExamCommission))
                                .IsNotSuccess(out ActionResponse<ExamCommissionDto> insertResponse, out ExamCommissionDto examCommission))
                            {
                                return await ActionResponse<StudentGroupDto>.ReturnError(insertResponse.Message);
                            }
                            entityDto.PracticalExamCommission = examCommission;
                            entityDto.PracticalExamCommissionId = examCommission.Id;
                        }
                    }
                }
                return await ActionResponse<StudentGroupDto>.ReturnSuccess(entityDto, "Praktična ispitna komisija za grupu studenata uspješno izmijenjena.");
            }
            catch (Exception)
            {
                return await ActionResponse<StudentGroupDto>.ReturnError($"Greška prilikom ažuriranja praktične ispitne komisije za grupu studenata.");
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
                newSubjectTeachers = newSubjectTeachers.Where(st => st.TeacherId != null && st.TeacherId != Guid.Empty).ToList();

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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
                    .Select(nt =>
                    {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return await ActionResponse<StudentClassAttendanceDto>.ReturnError("Greška prilikom ažuriranja teme u danu plana.");
            }
        }

        public async Task<ActionResponse<List<StudentClassAttendanceDto>>> RemoveAttendances(List<StudentClassAttendanceDto> entityDtos)
        {
            try
            {
                var response = await ActionResponse<List<StudentClassAttendanceDto>>.ReturnSuccess(null, "Evidencije nastave uspješno izbrisane.");
                entityDtos.ForEach(async pds =>
                {
                    if ((await RemoveAttendance(pds))
                        .IsNotSuccess(out ActionResponse<StudentClassAttendanceDto> removeResponse, out pds))
                    {
                        response = await ActionResponse<List<StudentClassAttendanceDto>>.ReturnError(removeResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentClassAttendanceDto>>.ReturnError("Greška prilikom brisanja evidencija nastave.");
            }
        }

        public async Task<ActionResponse<StudentClassAttendanceDto>> RemoveAttendance(StudentClassAttendanceDto entityDto)
        {
            try
            {
                unitOfWork.GetGenericRepository<StudentClassAttendance>().Delete(entityDto.Id.Value);
                unitOfWork.Save();
                return await ActionResponse<StudentClassAttendanceDto>.ReturnSuccess(null, "Evidencija nastave za studenta iz grupe uspješno izbrisana.");
            }
            catch (Exception)
            {
                return await ActionResponse<StudentClassAttendanceDto>.ReturnError("Greška prilikom brisanja evidencije nastave za studenta u grupi.");
            }
        }

        public async Task<ActionResponse<List<StudentClassAttendanceDto>>> GetAllAttendanceForStudentAndGroup(int studentId, int studentGroupId)
        {
            try
            {
                var entites = unitOfWork.GetGenericRepository<StudentClassAttendance>()
                    .GetAll(sca => sca.StudentId == studentId && sca.StudentGroupClassAttendance.StudentGroupId == studentGroupId);

                return await ActionResponse<List<StudentClassAttendanceDto>>
                    .ReturnSuccess(mapper.Map<List<StudentClassAttendanceDto>>(entites));
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentClassAttendanceDto>>.ReturnError("Greška prilikom dohvata evidencije nastave za studenta u grupi.");
            }
        }

        #endregion StudentAttendance

        #endregion Class Attendance

        #region CombinedGroups

        #region Readers

        public async Task<ActionResponse<List<CombinedGroupDto>>> GetAllCombined()
        {
            try
            {
                var entities = unitOfWork.GetGenericRepository<CombinedGroup>()
                    .GetAll(includeProperties: includeCombinedProperties);
                return await ActionResponse<List<CombinedGroupDto>>
                    .ReturnSuccess(mapper.Map<List<CombinedGroup>, List<CombinedGroupDto>>(entities));
            }
            catch (Exception)
            {
                return await ActionResponse<List<CombinedGroupDto>>.ReturnError("Greška prilikom dohvata svih kombiniranih grupa studenata.");
            }
        }

        public async Task<ActionResponse<PagedResult<CombinedGroupDto>>> GetAllCombinedPaged(BasePagedRequest pagedRequest)
        {
            try
            {
                var pagedEntityResult = await unitOfWork.GetGenericRepository<CombinedGroup>()
                    .GetAllAsQueryable(includeProperties: includeCombinedProperties)
                    .GetPaged(pagedRequest);

                var pagedResult = new PagedResult<CombinedGroupDto>
                {
                    CurrentPage = pagedEntityResult.CurrentPage,
                    PageSize = pagedEntityResult.PageSize,
                    PageCount = pagedEntityResult.PageCount,
                    RowCount = pagedEntityResult.RowCount,
                    Results = mapper.Map<List<CombinedGroup>, List<CombinedGroupDto>>(pagedEntityResult.Results)
                };

                return await ActionResponse<PagedResult<CombinedGroupDto>>.ReturnSuccess(pagedResult);
            }
            catch (Exception)
            {
                return await ActionResponse<PagedResult<CombinedGroupDto>>.ReturnError("Greška prilikom dohvata straničnih podataka kombiniranih grupa studenata.");
            }
        }

        public async Task<ActionResponse<CombinedGroupDto>> GetCombinedById(int id)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<CombinedGroup>()
                    .FindBy(c => c.Id == id, includeProperties: includeCombinedProperties);

                return await ActionResponse<CombinedGroupDto>
                    .ReturnSuccess(mapper.Map<CombinedGroupDto>(entity));
            }
            catch (Exception)
            {
                return await ActionResponse<CombinedGroupDto>.ReturnError("Greška prilikom dohvata kombinirane grupe studenata.");
            }
        }

        #endregion Readers

        #region Writers

        public async Task<ActionResponse<CombinedGroupDto>> InsertCombined(CombinedGroupDto entityDto)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var studentGroupIds = entityDto.StudentGroupIds != null ?
                        new List<int>(entityDto.StudentGroupIds) : new List<int>();

                    var entityToAdd = mapper.Map<CombinedGroupDto, CombinedGroup>(entityDto);
                    unitOfWork.GetGenericRepository<CombinedGroup>().Add(entityToAdd);
                    unitOfWork.Save();
                    mapper.Map(entityToAdd, entityDto);

                    entityDto.StudentGroupIds = studentGroupIds;
                    if ((await ModifyGroupsInCombinedGroup(entityDto))
                        .IsNotSuccess(out ActionResponse<CombinedGroupDto> response, out entityDto))
                    {
                        scope.Dispose();
                        return response;
                    }

                    if ((await GetCombinedById(entityDto.Id.Value)).IsNotSuccess(out response, out entityDto))
                    {
                        return response;
                    }

                    scope.Complete();
                    return await ActionResponse<CombinedGroupDto>.ReturnSuccess(entityDto, "Kombinirana grupa studenata uspješno upisana.");
                }
            }
            catch (Exception)
            {
                return await ActionResponse<CombinedGroupDto>.ReturnError("Greška prilikom upisa kombinirane grupe studenata.");
            }
        }

        public async Task<ActionResponse<CombinedGroupDto>> UpdateCombined(CombinedGroupDto entityDto)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var studentGroupIds = entityDto.StudentGroupIds != null ?
                    new List<int>(entityDto.StudentGroupIds) : new List<int>();

                var entityToUpdate = mapper.Map<CombinedGroupDto, CombinedGroup>(entityDto);
                unitOfWork.GetGenericRepository<CombinedGroup>().Update(entityToUpdate);
                unitOfWork.Save();
                mapper.Map(entityToUpdate, entityDto);

                entityDto.StudentGroupIds = studentGroupIds;
                if ((await ModifyGroupsInCombinedGroup(entityDto))
                    .IsNotSuccess(out ActionResponse<CombinedGroupDto> response, out entityDto))
                {
                    scope.Dispose();
                    return response;
                }

                if ((await GetCombinedById(entityDto.Id.Value)).IsNotSuccess(out response, out entityDto))
                {
                    return response;
                }

                scope.Complete();
                return await ActionResponse<CombinedGroupDto>.ReturnSuccess(entityDto, "Kombinirana grupa studenata uspješno ažurirana.");
            }
        }

        public async Task<ActionResponse<CombinedGroupDto>> DeleteCombined(int id)
        {
            try
            {
                unitOfWork.GetGenericRepository<CombinedGroup>().Delete(id);
                unitOfWork.Save();
                return await ActionResponse<CombinedGroupDto>.ReturnSuccess(null, "Brisanje kombinirane grupe studenata uspješno.");
            }
            catch (Exception)
            {
                return await ActionResponse<CombinedGroupDto>.ReturnError("Greška prilikom brisanja kombinirane grupe studenata.");
            }
        }

        #region Groups In Combined Groups

        public async Task<ActionResponse<CombinedGroupDto>> ModifyGroupsInCombinedGroup(CombinedGroupDto combinedGroup)
        {
            try
            {
                var entity = unitOfWork.GetGenericRepository<CombinedGroup>()
                    .FindBy(e => e.Id == combinedGroup.Id.Value, includeProperties: "StudentGroups");

                var currentGroups = mapper.Map<List<StudentGroup>, List<StudentGroupDto>>(entity.StudentGroups.ToList());

                var newGroups = combinedGroup.StudentGroupIds;

                var groupsToRemove = currentGroups.Where(cet => !newGroups.Contains(cet.Id.Value)).ToList();

                var groupsToAdd = newGroups
                    .Where(nt => !currentGroups.Select(uec => uec.Id).Contains(nt))
                    .Select(ng => new StudentGroupDto
                    {
                        Id = ng,
                        CombinedGroupId = combinedGroup.Id
                    }).ToList();

                if ((await RemoveGroupsFromCombinedGroup(groupsToRemove))
                    .IsNotSuccess(out ActionResponse<List<StudentGroupDto>> actionResponse))
                {
                    return await ActionResponse<CombinedGroupDto>.ReturnError("Neuspješna ažuriranje grupa u kombiniranoj grupi.");
                }

                if ((await AddGroupsToCombinedGroup(groupsToAdd))
                    .IsNotSuccess(out actionResponse))
                {
                    return await ActionResponse<CombinedGroupDto>.ReturnError("Neuspješna promjena grupa u kombiniranoj grupi.");
                }

                return await ActionResponse<CombinedGroupDto>.ReturnSuccess(combinedGroup, "Uspješno izmijenjene grupe unutar kombinirane grupe.");
            }
            catch (Exception)
            {
                return await ActionResponse<CombinedGroupDto>.ReturnError("Greška prilikom ažuriranja kombinirane grupe studenata.");
            }
        }

        public async Task<ActionResponse<List<StudentGroupDto>>> AddGroupsToCombinedGroup(List<StudentGroupDto> groups)
        {
            try
            {
                var response = await ActionResponse<List<StudentGroupDto>>.ReturnSuccess(groups, "Grupe uspješno dodane u kombiniranu grupu.");
                groups.ForEach(async s =>
                {
                    if ((await AddGroupToCombinedGroup(s))
                    .IsNotSuccess(out ActionResponse<StudentGroupDto> actionResponse, out s))
                    {
                        response = await ActionResponse<List<StudentGroupDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentGroupDto>>.ReturnError("Greška prilikom dodavanja grupa u kombiniranu grupu.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> AddGroupToCombinedGroup(StudentGroupDto group)
        {
            try
            {
                int combinedGroupId = group.CombinedGroupId.Value;
                if ((await GetById(group.Id.Value)).IsNotSuccess(out ActionResponse<StudentGroupDto> response, out group))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom pripreme podataka za dodavanje " +
                        "grupe u kombiniranu grupu.");
                }

                group.CombinedGroupId = combinedGroupId;
                if ((await Update(group)).IsNotSuccess(out response, out group))
                {
                    return response;
                }

                return await ActionResponse<StudentGroupDto>.ReturnSuccess(group, "Grupa uspješno dodana u kombiniranu grupu.");
            }
            catch (Exception)
            {
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom dodavanja grupe u kombiniranu grupu.");
            }
        }

        public async Task<ActionResponse<List<StudentGroupDto>>> RemoveGroupsFromCombinedGroup(List<StudentGroupDto> groups)
        {
            try
            {
                var response = await ActionResponse<List<StudentGroupDto>>.ReturnSuccess(groups, "Grupe uspješno maknute iz kombinirane grupe.");
                groups.ForEach(async s =>
                {
                    if ((await RemoveGroupFromCombinedGroup(s))
                    .IsNotSuccess(out ActionResponse<StudentGroupDto> actionResponse, out s))
                    {
                        response = await ActionResponse<List<StudentGroupDto>>.ReturnError(actionResponse.Message);
                        return;
                    }
                });
                return response;
            }
            catch (Exception)
            {
                return await ActionResponse<List<StudentGroupDto>>.ReturnError("Greška prilikom micanja grupa iz kombinirane grupe.");
            }
        }

        public async Task<ActionResponse<StudentGroupDto>> RemoveGroupFromCombinedGroup(StudentGroupDto group)
        {
            try
            {
                if ((await GetById(group.Id.Value)).IsNotSuccess(out ActionResponse<StudentGroupDto> response, out group))
                {
                    return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom pripreme podataka za micanje " +
                        "grupe iz kombinirane grupe.");
                }

                group.CombinedGroupId = null;
                if ((await Update(group)).IsNotSuccess(out response, out group))
                {
                    return response;
                }

                return await ActionResponse<StudentGroupDto>.ReturnSuccess(group, "Grupa uspješno maknuta iz kombinirane grupe.");
            }
            catch (Exception)
            {
                return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom micanja grupe iz kombinirane grupe.");
            }
        }

        #endregion Groups In Combined Groups

        #endregion Writers

        #endregion CombinedGroups

        #region Print

        private async Task<ActionResponse<List<PlanDayDatesDto>>> GetGroupDays(int groupId)
        {
            try
            {
                var context = unitOfWork.GetContext();

                var groupQuery = from studentGroup in context.StudentGroups
                                 where studentGroup.Id == groupId
                                 select studentGroup;

                var daysQuery = from sg in groupQuery
                                join eduProgram in context.EducationPrograms
                                on sg.ProgramId equals eduProgram.Id
                                join plan in context.Plans
                                on eduProgram.Id equals plan.EducationProgramId
                                join day in context.PlanDays
                                on plan.Id equals day.PlanId
                                where eduProgram.Status == DatabaseEntityStatusEnum.Active
                                && plan.Status == DatabaseEntityStatusEnum.Active
                                && day.Status == DatabaseEntityStatusEnum.Active
                                select day;

                var datesQuery = from sg in groupQuery
                                 join dates in context.StudentGroupClassAttendances
                                 on sg.Id equals dates.StudentGroupId
                                 where dates.Status == DatabaseEntityStatusEnum.Active
                                 select dates;

                var days = daysQuery.ToList();
                var dayDates = datesQuery.ToList();

                var daysDates = new List<PlanDayDatesDto>();

                for (var i = 0; i < dayDates.Count; i++)
                {
                    var dayDate = new PlanDayDatesDto
                    {
                        DayId = days[i].Id,
                        Date = dayDates[i].Date
                    };

                    daysDates.Add(dayDate);
                }

                return await ActionResponse<List<PlanDayDatesDto>>.ReturnSuccess(daysDates);
            }
            catch (Exception)
            {
                return await ActionResponse<List<PlanDayDatesDto>>.ReturnError("Greška prilikom mapiranja dana za ispis.");
            }
        }

        public async Task<ActionResponse<List<TeacherSubjectByDatesPrintModelData>>> GetTeacherClasses(int groupId)
        {
            try
            {
                var context = unitOfWork.GetContext();

                var groupQuery = from studentGroup in context.StudentGroups
                                 where studentGroup.Id == groupId
                                 select studentGroup;

                var teachQuery = from sg in groupQuery
                                 join teach in context.StudentGroupSubjectTeachers
                                 on sg.Id equals teach.StudentGroupId
                                 join user in context.Users
                                 on teach.TeacherId equals user.Id
                                 join userDetails in context.UserDetails
                                 on user.Id equals userDetails.UserId
                                 where sg.Id == groupId
                                 && sg.Status == DatabaseEntityStatusEnum.Active
                                 && user.Status == DatabaseEntityStatusEnum.Active
                                 && teach.Status == DatabaseEntityStatusEnum.Active
                                 && userDetails.Status == DatabaseEntityStatusEnum.Active
                                 select new { userDetails, teach };

                var daysQuery = from sg in groupQuery
                                join eduProgram in context.EducationPrograms
                                on sg.ProgramId equals eduProgram.Id
                                join plan in context.Plans
                                on eduProgram.Id equals plan.EducationProgramId
                                join day in context.PlanDays
                                on plan.Id equals day.PlanId
                                where eduProgram.Status == DatabaseEntityStatusEnum.Active
                                && plan.Status == DatabaseEntityStatusEnum.Active
                                && day.Status == DatabaseEntityStatusEnum.Active
                                select day;

                if ((await GetGroupDays(groupId))
                    .IsNotSuccess(out ActionResponse<List<PlanDayDatesDto>> daysResponse, out List<PlanDayDatesDto> daysDates))
                {
                    return await ActionResponse<List<TeacherSubjectByDatesPrintModelData>>.ReturnError(daysResponse.Message);
                }

                var subjectsQuery = from daySubjects in context.PlanDaySubjects
                                    join subject in context.Subjects
                                    on daySubjects.SubjectId equals subject.Id
                                    group daySubjects by daySubjects into g
                                    select new
                                    {
                                        DayId = g.Key.PlanDayId,
                                        Subject = new SubjectDto
                                        {
                                            Id = g.Select(gs => gs.Subject).FirstOrDefault().Id,
                                            Name = g.Select(gs => gs.Subject).FirstOrDefault().Name
                                        }
                                    };

                var themesQuery = from daySubjects in context.PlanDaySubjects
                                  join dayThemes in context.PlanDaySubjectThemes
                                  on daySubjects.Id equals dayThemes.PlanDaySubjectId
                                  where dayThemes.Status == DatabaseEntityStatusEnum.Active
                                  select new
                                  {
                                      SubjectId = daySubjects.SubjectId,
                                      Theme = new ThemeDto
                                      {
                                          Id = dayThemes.Theme.Id,
                                          Name = dayThemes.Theme.Name,
                                          HoursNumber = (int)dayThemes.HoursNumber
                                      }
                                  };

                var themesBySubject = from themes in themesQuery
                                      group themes by themes.SubjectId into g
                                      select new
                                      {
                                          SubjectId = g.Key,
                                          Themes = g.Select(t => t.Theme)
                                            .GroupBy(t => t.Id)
                                            .Select(x => x.First())
                                            .ToList()
                                      };


                var subjectsByDays = from daysQ in daysDates.AsQueryable()
                                     join subjectDays in subjectsQuery
                                     on daysQ.DayId equals subjectDays.DayId
                                     group subjectDays by subjectDays.Subject.Id into g
                                     select new
                                     {
                                         SubjectId = g.Key,
                                         Subject = g.Select(d => d.Subject).FirstOrDefault(),
                                         MinDate = daysDates.Select(d => d.Date).Min(),
                                         MaxDate = daysDates.Select(d => d.Date).Max()
                                     };

                var teacherAndSubjects = from teacher in teachQuery
                                         join subject in subjectsByDays
                                         on teacher.teach.SubjectId equals subject.SubjectId
                                         join themes in themesBySubject
                                         on subject.Subject.Id equals themes.SubjectId
                                         select new TeacherSubjectByDatesPrintModelData
                                         {
                                             Teacher = new UserDetailsDto
                                             {
                                                 UserId = teacher.userDetails.UserId,
                                                 Profession = teacher.userDetails.Profession,
                                                 Title = teacher.userDetails.Title,
                                                 FirstName = teacher.userDetails.FirstName,
                                                 LastName = teacher.userDetails.LastName
                                             },
                                             Subject = subject.Subject,
                                             Themes = themes.Themes,
                                             MinDate = subject.MinDate,
                                             MaxDate = subject.MaxDate
                                         };

                var groupedList = teacherAndSubjects.ToList();

                return await ActionResponse<List<TeacherSubjectByDatesPrintModelData>>.ReturnSuccess(groupedList, "Podaci dohvaćeni.");
            }
            catch (Exception)
            {
                return await ActionResponse<List<TeacherSubjectByDatesPrintModelData>>.ReturnError("Podaci za ispis neuspješno generirani.");
            }
        }

        public async Task<ActionResponse<List<ThemesByWeekPrintModel>>> GetThemesByWeeks(int groupId)
        {
            try
            {
                if ((await GetGroupDays(groupId))
                    .IsNotSuccess(out ActionResponse<List<PlanDayDatesDto>> daysResponse, out List<PlanDayDatesDto> daysDates))
                {
                    return await ActionResponse<List<ThemesByWeekPrintModel>>.ReturnError(daysResponse.Message);
                }

                var context = unitOfWork.GetContext();

                var themesQuery = from dayThemes in context.PlanDaySubjectThemes
                                  where dayThemes.Status == DatabaseEntityStatusEnum.Active
                                  join theme in context.Themes
                                  on dayThemes.ThemeId equals theme.Id
                                  join daySubject in context.PlanDaySubjects
                                  on dayThemes.PlanDaySubjectId equals daySubject.Id
                                  where daySubject.Status == DatabaseEntityStatusEnum.Active
                                  join subject in context.Subjects
                                  on daySubject.SubjectId equals subject.Id
                                  join day in daysDates
                                  on daySubject.PlanDayId equals day.DayId
                                  select new
                                  {
                                      DayId = day.DayId,
                                      DayDate = day.Date,
                                      Month = day.Date.GetMonthName(),
                                      Week = day.Date.GetWeekNumberOfMonth(),
                                      Year = day.Date.Year,
                                      Theme = new PlanDaySubjectThemeDto
                                      {
                                          Id = dayThemes.Id,
                                          ThemeId = dayThemes.ThemeId,
                                          HoursNumber = dayThemes.HoursNumber,
                                          ClassTypes = dayThemes.ClassTypes,
                                          PerfomingType = dayThemes.PerfomingType,
                                          Theme = new ThemeDto
                                          {
                                              Id = dayThemes.Theme.Id,
                                              Name = dayThemes.Theme.Name,
                                              Subject = new SubjectDto
                                              {
                                                  Id = subject.Id,
                                                  Name = subject.Name
                                              }
                                          }
                                      }
                                  };

                var themes = (from themesQ in themesQuery
                              group themesQ by new { themesQ.Month, themesQ.Week, themesQ.Year } into g
                              select new ThemesByWeekPrintModel
                              {
                                  Week = g.Key.Week,
                                  Month = g.Key.Month,
                                  Year = g.Key.Year,
                                  Themes = g.Select(q => q.Theme).ToList()
                              }
                              ).ToList();

                return await ActionResponse<List<ThemesByWeekPrintModel>>.ReturnSuccess(themes);
            }
            catch (Exception)
            {
                return await ActionResponse<List<ThemesByWeekPrintModel>>.ReturnError("Neuspješan dohvat podataka o temama iz plana.");
            }
        }

        public async Task<ActionResponse<List<ThemesClassesPrintModel>>> GetThemesClasses(int groupId)
        {
            try
            {
                if ((await GetGroupDays(groupId))
                    .IsNotSuccess(out ActionResponse<List<PlanDayDatesDto>> daysResponse, out List<PlanDayDatesDto> daysDates))
                {
                    return await ActionResponse<List<ThemesClassesPrintModel>>.ReturnError(daysResponse.Message);
                }

                var context = unitOfWork.GetContext();

                var themesQuery = from dayThemes in context.PlanDaySubjectThemes
                                  where dayThemes.Status == DatabaseEntityStatusEnum.Active
                                  join theme in context.Themes
                                  on dayThemes.ThemeId equals theme.Id
                                  join daySubject in context.PlanDaySubjects
                                  on dayThemes.PlanDaySubjectId equals daySubject.Id
                                  where daySubject.Status == DatabaseEntityStatusEnum.Active
                                  join subject in context.Subjects
                                  on daySubject.SubjectId equals subject.Id
                                  join day in daysDates
                                  on daySubject.PlanDayId equals day.DayId
                                  select new
                                  {
                                      DayId = day.DayId,
                                      DayDate = day.Date,
                                      Theme = new PlanDaySubjectThemeDto
                                      {
                                          Id = dayThemes.Id,
                                          ThemeId = dayThemes.ThemeId,
                                          HoursNumber = dayThemes.HoursNumber,
                                          ClassTypes = dayThemes.ClassTypes,
                                          PerfomingType = dayThemes.PerfomingType,
                                          Theme = new ThemeDto
                                          {
                                              Id = dayThemes.Theme.Id,
                                              Name = dayThemes.Theme.Name,
                                              SubjectId = subject.Id,
                                              Subject = new SubjectDto
                                              {
                                                  Id = subject.Id,
                                                  Name = subject.Name
                                              }
                                          }
                                      }
                                  };

                var groupQuery = from studentGroup in context.StudentGroups
                                 where studentGroup.Id == groupId
                                 select studentGroup;

                var teachQuery = from sg in groupQuery
                                 join teach in context.StudentGroupSubjectTeachers
                                 on sg.Id equals teach.StudentGroupId
                                 join user in context.Users
                                 on teach.TeacherId equals user.Id
                                 join userDetails in context.UserDetails
                                 on user.Id equals userDetails.UserId.Value
                                 join signature in context.UploadedFiles
                                 on userDetails.SignatureId equals signature.Id into uSig
                                 from sig in uSig.DefaultIfEmpty()
                                 where sg.Id == groupId
                                 && sg.Status == DatabaseEntityStatusEnum.Active
                                 && user.Status == DatabaseEntityStatusEnum.Active
                                 && teach.Status == DatabaseEntityStatusEnum.Active
                                 && userDetails.Status == DatabaseEntityStatusEnum.Active
                                 select new
                                 {
                                     SubjectId = teach.SubjectId,
                                     UserDetails = new UserDetailsDto
                                     {
                                         Id = userDetails.Id,
                                         UserId = userDetails.UserId,
                                         FirstName = userDetails.FirstName,
                                         LastName = userDetails.LastName,
                                         SignatureId = userDetails.SignatureId,
                                         Signature = new FileDto
                                         {
                                             Id = sig.Id,
                                             FileName = sig.FileName
                                         },
                                         Profession = userDetails.Profession,
                                         Title = userDetails.Title,
                                         Qualifications = userDetails.Qualifications
                                     }
                                 };

                var themesInDaysQuery = from themes in themesQuery
                                        join teach in teachQuery
                                        on themes.Theme.Theme.SubjectId.Value equals teach.SubjectId
                                        select new
                                        {
                                            DayId = themes.DayId,
                                            Date = themes.DayDate,
                                            DayClass = new PlanDayThemeAndTeacherDto
                                            {
                                                DayTheme = themes.Theme,
                                                SubjectTeacher = teach.UserDetails
                                            }
                                        };

                var dayClasses = (from themes in themesInDaysQuery
                                  group themes by themes.DayId into g
                                  select new ThemesClassesPrintModel
                                  {
                                      Date = g.Select(q => q.Date).FirstOrDefault(),
                                      DayClasses = g
                                         .Select(q => q.DayClass)
                                         .ToList()
                                  }).ToList();

                var dayClassesList = dayClasses.ToList();

                dayClassesList.ForEach(dayClass =>
                    {
                        var newDayThemeList = new List<PlanDayThemeAndTeacherDto>();

                        dayClass.DayClasses.ForEach(dc =>
                        {
                            for (var i = 0; i < dc.DayTheme.HoursNumber; i++)
                            {
                                newDayThemeList.Add(dc);
                            }
                        });

                        dayClass.DayClasses = newDayThemeList;
                    }
                );

                return await ActionResponse<List<ThemesClassesPrintModel>>.ReturnSuccess(dayClassesList);
            }
            catch (Exception)
            {
                return await ActionResponse<List<ThemesClassesPrintModel>>.ReturnError("Neuspješan dohvat podataka za ispis.");
            }
        }

        #endregion Print
    }
}
