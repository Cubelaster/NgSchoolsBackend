using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Services.Contracts;
using NgSchoolsDataLayer.Models;
using System;
using System.Threading.Tasks;

namespace NgSchoolsBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<ActionResponse<User>> GetAll()
        {
            var user = userService.GetUserById(Guid.Empty);
            return await ActionResponse<User>.ReturnSuccess();
        }
    }
}