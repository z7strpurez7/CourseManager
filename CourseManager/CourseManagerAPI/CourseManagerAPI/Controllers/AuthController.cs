using CourseManagerAPI.Constants;
using CourseManagerAPI.DTOs.Auth;
using CourseManagerAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace CourseManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //Route-> Seed Roles to DB
        [HttpPost]
        [Route("Seed-Roles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seedResult = await _authService.SeedRolesAsync();
            return StatusCode(seedResult.StatusCode, seedResult.Message);
        }

        //route -> register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
        {
            var registerResult = await _authService.RegisterAsync(registerDto);
            return StatusCode(registerResult.StatusCode);
        }
   

        //Route -> login
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<LoginServiceResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var loginResult = await _authService.LoginAsync(loginDto);
            if(loginResult is null)
            {
                 return Unauthorized("Invalid credentials");
            }
            return Ok(loginResult);
        }

        //route -> update user role
        //Owner can change everything
        //Admin can just change roles below(Lecturer/user)
        [HttpPost]
        [Route("update-Role")]
        [Authorize(Roles = StaticUserRoles.OWNERADMIN)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto updateRoleDto)
        {
            var updateRoleResult = await _authService.UpdateRoleAsync(User, updateRoleDto);
            if (updateRoleResult.IsSuccess)
            {
                return Ok(updateRoleResult.Message);
            } else
            {
                return StatusCode(updateRoleResult.StatusCode, updateRoleResult.Message);
            }

        }

        //route -> getting data user from JWT
        [HttpPost]
        [Route("me")]
        [Authorize]
        public async Task<ActionResult<LoginServiceResponseDto>> Me([FromBody] MeDto token)
        {
          try
            {
                var me = await _authService.MeAsync(token); 
                if(me is not null)
                {
                   return Ok(me);
                }else
                {
                    return Unauthorized("Invalid Token");
                }
            }
            catch (Exception ex)
            {
                return Unauthorized("Invalid Token");
            }
        }


        // Route -> List of all users
        [HttpGet]
        [Route("users")]
        [Authorize(Roles = StaticUserRoles.OWNERADMIN)]
        public async Task<ActionResult<IEnumerable<UserInfoResult>>> GetUserList()
        {
            var userList = await _authService.GetUserListAsync();
            return Ok(userList);
        }

        //Route -> Get a User by UserName
        [HttpGet]
        [Route("users/{userName}")]
        [Authorize(Roles = StaticUserRoles.OWNERADMIN)]
        public async Task<ActionResult<UserInfoResult>> GetUserDetailsByUserName([FromRoute] string userName)
        {
            var user = await _authService.GetUserDetailsByUserNameAsync(userName);
            if(user is not null)
            {
                return Ok(user);
            } else
            {
                return NotFound("UserName not found");
            }
        }

        // Route -> Get List of all usernames for send message
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherNameDto>>> GetTeacherNamesList()
        {
            var teacherNames = await _authService.GetTeacherNamesListAsync();

            return Ok(teacherNames);

        }
    }
}
