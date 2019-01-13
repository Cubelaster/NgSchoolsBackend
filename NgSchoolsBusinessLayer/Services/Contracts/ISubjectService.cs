using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface ISubjectService
    {
        Task<ActionResponse<SubjectDto>> Insert(SubjectDto entityDto);
        Task<ActionResponse<List<SubjectDto>>> ModifySubjectsForEducationProgram(List<SubjectDto> subjects);
        Task<ActionResponse<SubjectDto>> Update(SubjectDto entityDto);
    }
}