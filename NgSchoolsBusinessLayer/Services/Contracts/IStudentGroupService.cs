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
    }
}