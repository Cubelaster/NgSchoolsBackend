using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Extensions;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Responses;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Ctors and Members

        private readonly IAuthService authService;
        private readonly IJwtFactory jwtFactory;
        private readonly IPdfGeneratorService pdfGeneratorService;

        public AuthController(IAuthService authService, IJwtFactory jwtFactory, IPdfGeneratorService pdfGeneratorService)
        {
            this.authService = authService;
            this.jwtFactory = jwtFactory;
            this.pdfGeneratorService = pdfGeneratorService;
        }

        #endregion Ctors and Members

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResponse<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            return await authService.Login(loginRequest);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResponse<object>> ForgotPassword()
        {
            return await ActionResponse<object>.ReturnSuccess();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResponse<FileDto>> GeneratePdf()
        {
            if ((await pdfGeneratorService.GeneratePdf("Test.pdf"))
                .IsNotSuccess(out ActionResponse<FileDto> actionResponse, out FileDto file))
            {
                return await ActionResponse<FileDto>.ReturnError("Greška prilikom generiranja PDF-a");
            }
            return actionResponse;
        }
    }
}
