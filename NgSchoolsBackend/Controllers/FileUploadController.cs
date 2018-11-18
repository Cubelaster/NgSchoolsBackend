using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
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

        [HttpPost]
        public async Task<ActionResponse<string>> Upload([FromBody] FileUploadRequest fileUploadRequest)
        {
            return await fileUploadService.Upload(fileUploadRequest);
        }
    }
}
