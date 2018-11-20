using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IClassTypeService
    {
        Task<ActionResponse<List<ClassTypeDto>>> GetAll();
        Task<ActionResponse<PagedResult<ClassTypeDto>>> GetAllClassTypesPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<ClassTypeDto>> GetById(int id);
        Task<ActionResponse<ClassTypeDto>> Insert(ClassTypeDto classType);
        Task<ActionResponse<ClassTypeDto>> Update(ClassTypeDto classType);
        Task<ActionResponse<ClassTypeDto>> Delete(int id);
    }
}