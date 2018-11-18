using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IEducationLevelService
    {
        Task<ActionResponse<List<EducationLevelDto>>> GetAll();
        Task<ActionResponse<PagedResult<EducationLevelDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<EducationLevelDto>> GetById(int id);
        Task<ActionResponse<EducationLevelDto>> Insert(EducationLevelDto entityDto);
        Task<ActionResponse<EducationLevelDto>> Update(EducationLevelDto entityDto);
    }
}