using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Responses;
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

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
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
    }
}
