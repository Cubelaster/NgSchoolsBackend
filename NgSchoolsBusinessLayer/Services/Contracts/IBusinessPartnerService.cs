using System.Collections.Generic;
using System.Threading.Tasks;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface IBusinessPartnerService
    {
        Task<ActionResponse<BusinessPartnerDto>> Delete(int id);
        Task<ActionResponse<List<BusinessPartnerDto>>> GetAll();
        Task<ActionResponse<PagedResult<BusinessPartnerDto>>> GetAllPaged(BasePagedRequest pagedRequest);
        Task<ActionResponse<BusinessPartnerDto>> GetById(int id);
        Task<ActionResponse<BusinessPartnerDto>> Insert(BusinessPartnerDto entityDto);
        Task<ActionResponse<BusinessPartnerDto>> Update(BusinessPartnerDto entityDto);
    }
}