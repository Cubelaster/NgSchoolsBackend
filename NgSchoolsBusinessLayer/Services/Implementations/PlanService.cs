using AutoMapper;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Implementations
{
    public class PlanService : IPlanService
    {
        #region Ctors and Members

        private readonly IMapper mapper;
        private readonly ILoggerService loggerService;
        private readonly IUnitOfWork unitOfWork;

        public PlanService(IMapper mapper, ILoggerService loggerService,
            IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.loggerService = loggerService;
            this.unitOfWork = unitOfWork;
        }

        #endregion Ctors and Members

        public async Task<ActionResponse<PlanDto>> Insert(PlanDto entityDto)
        {
            try
            {
                var planToAdd = mapper.Map<PlanDto, Plan>(entityDto);
                unitOfWork.GetGenericRepository<Plan>().Add(planToAdd);
                mapper.Map(planToAdd, entityDto);

                return await ActionResponse<PlanDto>.ReturnSuccess(entityDto, "Plan uspješno upisan.");
            }
            catch (Exception ex)
            {
                loggerService.LogErrorToEventLog(ex, entityDto);
                return await ActionResponse<PlanDto>.ReturnError("Greška prilikom upisivanja plana.");
            }
        }

        //public async Task<ActionResponse<PlanDto>> ModifyStudentsInGroup(PlanDto studentGroup)
        //{
        //    try
        //    {
        //        var entity = unitOfWork.GetGenericRepository<StudentGroup>()
        //            .FindBy(e => e.Id == studentGroup.Id.Value, includeProperties: "StudentsInGroups.Student");
        //        var currentStudents = mapper.Map<List<StudentsInGroups>, List<StudentInGroupDto>>(entity.StudentsInGroups.ToList());

        //        var newStudents = studentGroup.StudentIds;

        //        var studentsToRemove = currentStudents.Where(cet => !newStudents.Contains(cet.StudentId.Value)).ToList();

        //        var studentsToAdd = newStudents
        //            .Where(nt => !currentStudents.Select(uec => uec.StudentId).Contains(nt))
        //            .Select(nt => new StudentInGroupDto { StudentId = nt, GroupId = studentGroup.Id })
        //            .ToList();

        //        if ((await RemoveStudentsFromGroup(studentsToRemove))
        //            .IsNotSuccess(out ActionResponse<List<StudentInGroupDto>> actionResponse))
        //        {
        //            return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješna ažuriranje studenata u grupi.");
        //        }

        //        if ((await AddStudentsInGroup(studentsToAdd))
        //            .IsNotSuccess(out actionResponse))
        //        {
        //            return await ActionResponse<StudentGroupDto>.ReturnError("Neuspješna promjena studenata u grupi.");
        //        }
        //        return await ActionResponse<StudentGroupDto>.ReturnSuccess(studentGroup, "Uspješno izmijenjeni studenti grupe.");
        //    }
        //    catch (Exception ex)
        //    {
        //        loggerService.LogErrorToEventLog(ex, studentGroup);
        //        return await ActionResponse<StudentGroupDto>.ReturnError("Greška prilikom ažuriranja grupe studenata.");
        //    }
        //}

        //public async Task<ActionResponse<List<StudentInGroupDto>>> RemoveStudentsFromGroup(List<StudentInGroupDto> studentsInGroup)
        //{
        //    try
        //    {
        //        var response = await ActionResponse<List<StudentInGroupDto>>.ReturnSuccess(null, "Studenti uspješno maknuti iz grupe.");
        //        studentsInGroup.ForEach(async sig =>
        //        {
        //            if ((await RemoveStudentFromGroup(sig))
        //            .IsNotSuccess(out ActionResponse<StudentInGroupDto> actionResponse))
        //            {
        //                response = await ActionResponse<List<StudentInGroupDto>>.ReturnError(actionResponse.Message);
        //                return;
        //            }
        //        });
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        loggerService.LogErrorToEventLog(ex, studentsInGroup);
        //        return await ActionResponse<List<StudentInGroupDto>>.ReturnError("Some sort of fuckup. Try again.");
        //    }
        //}

        //public async Task<ActionResponse<StudentInGroupDto>> RemoveStudentFromGroup(StudentInGroupDto studentInGroup)
        //{
        //    try
        //    {
        //        unitOfWork.GetGenericRepository<StudentsInGroups>().Delete(studentInGroup.Id.Value);
        //        unitOfWork.Save();
        //        return await ActionResponse<StudentInGroupDto>.ReturnSuccess(null, "Student upsješno izbrisan iz grupe.");
        //    }
        //    catch (Exception ex)
        //    {
        //        loggerService.LogErrorToEventLog(ex, studentInGroup);
        //        return await ActionResponse<StudentInGroupDto>.ReturnError("Greška prilikom micanja studenta iz grupe.");
        //    }
        //}

        //public async Task<ActionResponse<List<StudentInGroupDto>>> AddStudentsInGroup(List<StudentInGroupDto> students)
        //{
        //    try
        //    {
        //        var response = await ActionResponse<List<StudentInGroupDto>>.ReturnSuccess(null, "Studenti uspješno dodani u grupu.");
        //        students.ForEach(async s =>
        //        {
        //            if ((await AddStudentInGroup(s))
        //            .IsNotSuccess(out ActionResponse<StudentInGroupDto> actionResponse))
        //            {
        //                response = await ActionResponse<List<StudentInGroupDto>>.ReturnError(actionResponse.Message);
        //                return;
        //            }
        //        });
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        loggerService.LogErrorToEventLog(ex, students);
        //        return await ActionResponse<List<StudentInGroupDto>>.ReturnError("Greška prilikom dodavanja studenata u grupu.");
        //    }
        //}

        //public async Task<ActionResponse<StudentInGroupDto>> AddStudentInGroup(StudentInGroupDto student)
        //{
        //    try
        //    {
        //        var entityToAdd = mapper.Map<StudentInGroupDto, StudentsInGroups>(student);
        //        unitOfWork.GetGenericRepository<StudentsInGroups>().Add(entityToAdd);
        //        unitOfWork.Save();
        //        return await ActionResponse<StudentInGroupDto>
        //            .ReturnSuccess(mapper.Map<StudentsInGroups, StudentInGroupDto>(entityToAdd),
        //            "Student uspješno dodan u grupu.");
        //    }
        //    catch (Exception ex)
        //    {
        //        loggerService.LogErrorToEventLog(ex, student);
        //        return await ActionResponse<StudentInGroupDto>.ReturnError("Greška prilikom dodavanja studenta u grupu.");
        //    }
        //}
    }
}
