using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Security.Jwt.Contracts;
using NgSchoolsBusinessLayer.Services.Contracts;
using System;
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

        public AuthController(IAuthService authService, IJwtFactory jwtFactory)
        {
            this.authService = authService;
            this.jwtFactory = jwtFactory;
        }

        #endregion Ctors and Members

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResponse<object>> Login([FromBody] LoginRequest loginRequest)
        {
            var result = authService.Login(loginRequest);
            return await result;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResponse<object>> ForgotPassword()
        {
            Console.WriteLine("Kita");
            return await ActionResponse<object>.ReturnSuccess();
        }
    }
}
