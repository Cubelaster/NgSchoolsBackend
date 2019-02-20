using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IEducationGroupService
    {
        Task<ActionResponse<List<EducationGroupDto>>> GetAll();
        Task<ActionResponse<PagedResult<EducationGroupDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<EducationGroupDto>> GetById(int id);
        Task<ActionResponse<EducationGroupDto>> Insert(EducationGroupDto entityDto);
        Task<ActionResponse<EducationGroupDto>> Update(EducationGroupDto entityDto);
        Task<ActionResponse<EducationGroupDto>> Delete(int id);
        Task<ActionResponse<int>> GetTotalNumber();
    }
}