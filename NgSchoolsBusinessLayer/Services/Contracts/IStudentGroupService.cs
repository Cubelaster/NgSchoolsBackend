using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IStudentGroupService
    {
        Task<ActionResponse<StudentGroupDto>> GetById(int id);

        Task<ActionResponse<List<StudentGroupDto>>> GetAll();
        Task<ActionResponse<int>> GetTotalNumber();
        Task<ActionResponse<PagedResult<StudentGroupDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<StudentGroupDto>> Delete(int id);
        Task<ActionResponse<StudentGroupDto>> Insert(StudentGroupDto entityDto);
        Task<ActionResponse<StudentGroupDto>> Update(StudentGroupDto entityDto);
        Task<ActionResponse<List<CombinedGroupDto>>> GetAllCombined();
        Task<ActionResponse<PagedResult<CombinedGroupDto>>> GetAllCombinedPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<CombinedGroupDto>> GetCombinedById(int id);
        Task<ActionResponse<CombinedGroupDto>> InsertCombined(CombinedGroupDto request);
        Task<ActionResponse<CombinedGroupDto>> UpdateCombined(CombinedGroupDto request);
        Task<ActionResponse<CombinedGroupDto>> DeleteCombined(int id);
    }
}