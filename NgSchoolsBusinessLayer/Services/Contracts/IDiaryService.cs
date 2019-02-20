using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IDiaryService
    {
        Task<ActionResponse<List<DiaryDto>>> GetAll();
        Task<ActionResponse<PagedResult<DiaryDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<DiaryDto>> GetById(int id);
        Task<ActionResponse<List<DiaryStudentGroupDto>>> AddStudentGroups(List<DiaryStudentGroupDto> studentGroupDtos);
        Task<ActionResponse<DiaryStudentGroupDto>> DeleteDiaryStudentGroup(DiaryStudentGroupDto entityDto);
        Task<ActionResponse<DiaryDto>> Insert(DiaryDto entityDto);
        Task<ActionResponse<DiaryStudentGroupDto>> InsertDiaryStudentGroup(DiaryStudentGroupDto entityDto);
        Task<ActionResponse<List<DiaryStudentGroupDto>>> RemoveStudentGroups(List<DiaryStudentGroupDto> studentGroupDtos);
        Task<ActionResponse<DiaryDto>> Update(DiaryDto entityDto);
        Task<ActionResponse<DiaryDto>> Delete(int id);
        Task<ActionResponse<int>> GetTotalNumber();
    }
}