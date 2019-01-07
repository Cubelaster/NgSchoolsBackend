using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IEducationProgramService
    {
        Task<ActionResponse<EducationProgramDto>> Delete(int id);
        Task<ActionResponse<List<EducationProgramDto>>> GetAll();
        Task<ActionResponse<PagedResult<EducationProgramDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<EducationProgramDto>> GetById(int id);
        Task<ActionResponse<EducationProgramDto>> Insert(EducationProgramDto entityDto);
        Task<ActionResponse<EducationProgramDto>> Update(EducationProgramDto entityDto);
    }
}