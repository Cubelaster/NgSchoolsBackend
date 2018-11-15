using Microsoft.AspNetCore.Mvc;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Models.Common.Paging;
using NgSchoolsBusinessLayer.Models.Dto;
using NgSchoolsBusinessLayer.Models.Requests;
using NgSchoolsBusinessLayer.Models.Requests.Base;
using NgSchoolsBusinessLayer.Services.Contracts;
using System.Collections.Generic;
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
        
        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<List<UserDto>>> GetAll()
        {
            return await userService.GetAllUsers();
        }

        // TODO: Authorize
        [HttpPost]
        public async Task<ActionResponse<PagedResult<UserDto>>> GetAllPaged([FromBody] BasePagedRequest pagedRequest)
        {
            return await userService.GetAllUsersPaged(pagedRequest);
        }

        [HttpPost]
        public async Task<ActionResponse<UserDto>> GetById([FromBody]UserGetRequest request)
        {
            return await userService.GetById(request.Id.Value);
        }

        [HttpPost]
        public async Task<ActionResponse<UserDto>> Create([FromBody]UserDto request)
        {
            return await userService.Create(request);
        }

        [HttpPost]
        public async Task<ActionResponse<UserDto>> Update([FromBody]UserDto request)
        {
            return await userService.Update(request);
        }

        [HttpPost]
        public async Task<ActionResponse<object>> Delete([FromBody]UserGetRequest request)
        {
            return await userService.Delete(request);
        }
    }
}