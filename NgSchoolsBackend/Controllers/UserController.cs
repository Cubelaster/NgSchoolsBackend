using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsDataLayer.Models;
using System.Threading.Tasks;

namespace NgSchoolsBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResponse<User>> GetAll()
        {
            var result = ActionResponse<User>.ReturnSuccess();
            return await Task.FromResult(result);
        }
    }
}