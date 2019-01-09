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
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<StudentDto>> GetById(SimpleRequestBase request)
        {
            return await studentService.GetById(request.Id);
        }

        [HttpPost]
        public async Task<ActionResponse<StudentDto>> GetByOib(SimpleRequestBase request)
        {
            return await studentService.GetByOib(request.SearchParam);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<StudentDto>> Insert([FromBody] StudentDto student)
        {
            return await studentService.Insert(student);
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<StudentDto>> Update([FromBody] StudentDto student)
        {
            return await studentService.Update(student);
        }

        [HttpPost]
        public async Task<ActionResponse<List<StudentDto>>> GetAll()
        {
            return await studentService.GetAll();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<StudentDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await studentService.GetAllPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<StudentDto>> Delete([FromBody] SimpleRequestBase request)
        {
            return await studentService.Delete(request.Id);
        }
    }
}