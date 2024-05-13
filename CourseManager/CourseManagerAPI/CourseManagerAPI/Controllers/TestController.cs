using CourseManagerAPI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CourseManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("get-public")]
        public IActionResult GetPublicData()
        {
            return Ok("Public data");
        }

        [HttpGet]
        [Route("get-user-role")]
        [Authorize(Roles = StaticUserRoles.USER)]
        public IActionResult GetUserData()
        {
            return Ok("User Role Data");
        }

        [HttpGet]
        [Route("get-lecturer-role")]
        [Authorize(Roles = StaticUserRoles.LECTURER)]
        public IActionResult GetLecturerData()
        {
            return Ok("Lecturer role data");
        }
        [HttpGet]
        [Route("get-admin-role")]
        [Authorize(Roles = StaticUserRoles.ADMIN)]
        public IActionResult GetAdminData()
        {
            return Ok("Admin role data");
        }
        [HttpGet]
        [Route("get-owner-role")]
        [Authorize(Roles = StaticUserRoles.OWNER)]
        public IActionResult GetOwnerData()
        {
            return Ok("Owner role data");
        }




    }

}
