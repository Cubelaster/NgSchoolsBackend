using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsDataLayer.Models;
using System.Threading.Tasks;

namespace NgSchoolsWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResponse<User>> Login()
        {
            var result = ActionResponse<User>.ReturnSuccess();
            return await Task.FromResult(result);
        }
    }
}
