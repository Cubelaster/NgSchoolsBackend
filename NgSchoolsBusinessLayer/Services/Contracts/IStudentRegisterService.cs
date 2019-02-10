using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IStudentRegisterService
    {
        Task<ActionResponse<List<StudentRegisterDto>>> GetAll();
        Task<ActionResponse<List<StudentRegisterDto>>> GetAllNotFull();
        Task<ActionResponse<List<StudentRegisterDto>>> GetAllForCache();
        Task<ActionResponse<PagedResult<StudentRegisterDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<StudentRegisterDto>> GetById(int id);
        Task<ActionResponse<PagedResult<StudentRegisterDto>>> GetBySearchQuery(BasePagedRequest pagedRequest);
    }
}