using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Requests;
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

        [HttpPost]
        public async Task<ActionResponse<LoginRequest>> Login([FromBody] LoginRequest loginRequest)
        {
            var result = authService.Login(loginRequest);
            return await result;
        }
    }
}
