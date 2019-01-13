using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface ISubjectService
    {
        Task<ActionResponse<SubjectDto>> Insert(SubjectDto entityDto);
        Task<ActionResponse<List<SubjectDto>>> InsertSubjects(List<SubjectDto> subjects);
        Task<ActionResponse<List<SubjectDto>>> ModifySubjectsForEducationProgram(List<SubjectDto> subjects);
        Task<ActionResponse<SubjectDto>> Update(SubjectDto entityDto);
        Task<ActionResponse<List<SubjectDto>>> UpdateSubjects(List<SubjectDto> subjects);
        Task<ActionResponse<SubjectDto>> Delete(int id);
        Task<ActionResponse<List<SubjectDto>>> DeleteSubjects(List<SubjectDto> subjects);
    }
}