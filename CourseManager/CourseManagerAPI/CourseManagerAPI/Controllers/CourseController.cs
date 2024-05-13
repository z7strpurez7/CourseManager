using CourseManagerAPI.Constants;
using CourseManagerAPI.DTOs.Course;
using CourseManagerAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CourseManagerAPI.Interfaces;
using CourseManagerAPI.DTOs.General;
using System.Security.Claims;

namespace CourseManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = StaticUserRoles.OWNERADMIN)]
        public async Task<IActionResult> CreateNewCourse([FromBody] CreateCourseDto courseDto)
        {
            var result = await _courseService.CreateCourseAsync(User, courseDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return StatusCode(result.StatusCode, result.Message);
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<GetCoursesDto>> GetCourseById([FromRoute] int Id)
        {
            var course = await _courseService.GetCourseAsync(Id);

            return Ok(course);
        }

        [HttpGet]
        [Route("enrolled-courses")]
        [Authorize] // Ensure only authorized users can access
        public async Task<ActionResult<IEnumerable<GetCoursesDto>>> GetEnrolledCourses()
        {
            try
            {
                var courses = await _courseService.GetEnrolledCoursesAsync(User);
                return Ok(courses);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCourses")]
        public async Task<ActionResult<IEnumerable<GetCoursesDto>>> GetCourses()
        {
            var courses = await _courseService.GetCoursesAsync();
            return Ok(courses);
        }

        [HttpGet]
        [Route("GetTitles")]
        public async Task<ActionResult<IEnumerable<GetCourseTitlesDto>>> GetCourseTitles()
        {
            var courses = await _courseService.GetCourseTitlesAsync();
            return Ok(courses);
        }



        [HttpPut("{courseId}/{teacherId}")]
        public async Task<IActionResult> SetTeacherToCourseByID(string teacherId, int courseId)
        {
            var result = await _courseService.SetTeacherToCourseAsync(teacherId, courseId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return StatusCode(result.StatusCode, result.Message);
        }





    }
}
