using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface ISubjectService
    {
        #region Readers

        Task<ActionResponse<SubjectDto>> GetById(int id);
        Task<ActionResponse<List<SubjectDto>>> GetAll();
        Task<ActionResponse<List<SubjectDto>>> GetAllByEducationProgramId(int educationProgramId);
        Task<ActionResponse<PagedResult<SubjectDto>>> GetAllPaged(BasePagedRequest pagedRequest);

        #endregion Readers

        Task<ActionResponse<SubjectDto>> Insert(SubjectDto entityDto);
        Task<ActionResponse<List<SubjectDto>>> InsertSubjects(List<SubjectDto> subjects);
        Task<ActionResponse<List<SubjectDto>>> ModifySubjectsForEducationProgram(List<SubjectDto> subjects);
        Task<ActionResponse<SubjectDto>> Update(SubjectDto entityDto);
        Task<ActionResponse<List<SubjectDto>>> UpdateSubjects(List<SubjectDto> subjects);
        Task<ActionResponse<SubjectDto>> Delete(int id);
        Task<ActionResponse<List<SubjectDto>>> DeleteSubjects(List<SubjectDto> subjects);
        Task<ActionResponse<int>> GetTotalNumber();
    }
}