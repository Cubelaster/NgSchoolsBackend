using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IClassLocationsService
    {
        Task<ActionResponse<List<ClassLocationsDto>>> GetAll();
        Task<ActionResponse<PagedResult<ClassLocationsDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<ClassLocationsDto>> GetById(int id);
        Task<ActionResponse<ClassLocationsDto>> Insert(ClassLocationsDto classType);
        Task<ActionResponse<ClassLocationsDto>> Update(ClassLocationsDto classType);
    }
}