using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessPartnerController
    {
        private readonly IBusinessPartnerService businessPartnerService;

        public BusinessPartnerController(IBusinessPartnerService businessPartnerService)
        {
            this.businessPartnerService = businessPartnerService;
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<BusinessPartnerDto>> GetById(SimpleRequestBase request)
        {
            return await businessPartnerService.GetById(request.Id);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<BusinessPartnerDto>> Insert([FromBody] BusinessPartnerDto classType)
        {
            return await businessPartnerService.Insert(classType);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<BusinessPartnerDto>> Update([FromBody] BusinessPartnerDto classType)
        {
            return await businessPartnerService.Update(classType);
        }

        [HttpPost]
        public async Task<ActionResponse<List<BusinessPartnerDto>>> GetAll()
        {
            return await businessPartnerService.GetAll();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<BusinessPartnerDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await businessPartnerService.GetAllPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<BusinessPartnerDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await businessPartnerService.Delete(request.Id);
        }
    }
}
