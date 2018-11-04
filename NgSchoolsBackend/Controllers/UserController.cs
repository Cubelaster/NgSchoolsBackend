using Microsoft.AspNetCore.Mvc;

namespace NgSchoolsBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public JsonResult GetAll()
        {
            return new JsonResult(new string[] { "Kita" });
        }
    }
}