using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService)
        {
            this.fileUploadService = fileUploadService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResponse<FileDto>> Upload([FromBody] FileUploadRequest fileUploadRequest)
        {
            return await fileUploadService.Upload(fileUploadRequest);
        }
    }
}
