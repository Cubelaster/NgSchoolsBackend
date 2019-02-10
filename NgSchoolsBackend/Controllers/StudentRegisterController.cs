using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentRegisterController
    {
        private readonly IStudentRegisterService studentRegisterService;

        public StudentRegisterController(IStudentRegisterService studentRegisterService)
        {
            this.studentRegisterService = studentRegisterService;
        }

        [HttpPost]
        public async Task<ActionResponse<StudentRegisterDto>> GetById(SimpleRequestBase request)
        {
            return await studentRegisterService.GetById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<List<StudentRegisterDto>>> GetAll()
        {
            return await studentRegisterService.GetAll();
        }

        [HttpPost]
        public async Task<ActionResponse<List<StudentRegisterDto>>> GetAllNotFull()
        {
            return await studentRegisterService.GetAllNotFull();
        }

        [HttpPost]
        public async Task<ActionResponse<PagedResult<StudentRegisterDto>>> GetAllPaged(BasePagedRequest pagedRequest)
        {
            return await studentRegisterService.GetAllPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<StudentRegisterEntryDto>> GetEntryById(SimpleRequestBase request)
        {
            return await studentRegisterService.GetEntryById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<List<StudentRegisterEntryDto>>> GetAllEntries()
        {
            return await studentRegisterService.GetAllEntries();
        }

        [HttpPost]
        public async Task<ActionResponse<PagedResult<StudentRegisterEntryDto>>> GetAllEntriesPaged(BasePagedRequest pagedRequest)
        {
            return await studentRegisterService.GetAllEntriesPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<PagedResult<StudentRegisterEntryDto>>> GetAllEntriesByBookIdPaged(BasePagedRequest pagedRequest)
        {
            return await studentRegisterService.GetAllEntriesByBookIdPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<StudentRegisterEntryDto>> InsertEntry(StudentRegisterEntryInsertRequest request)
        {
            return await studentRegisterService.InsertEntry(request);
        }

        [HttpPost]
        public async Task<ActionResponse<StudentRegisterEntryDto>> UpdateEntry(StudentRegisterEntryInsertRequest request)
        {
            return await studentRegisterService.UpdateEntry(request);
        }
    }
}
