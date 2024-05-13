using CourseManagerAPI.Constants;
using CourseManagerAPI.Context;
using CourseManagerAPI.DTOs.General;
using CourseManagerAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace CourseManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly courseDbContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(courseDbContext context, UserManager<UserEntity> userManager, ILogger<EnrollmentController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        [HttpPost]
        [Route("{courseId}")]
        [Authorize(Roles = StaticUserRoles.ADMINLECTURERUSER)]
        public async Task<GeneralServiceResponseDto> EnrollInCourse([FromRoute] int courseId)
        {
            var userName = User.Identity.Name;
            if (userName == null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "Unauthorized",
                    StatusCode = 403,
                };
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = 404,
                };
            }

            var course = await _context.Courses.Include(c => c.Enrollments).FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "Course not found",
                    StatusCode = 404,
                };
            }

            // Check if enrollment is at least 2 weeks before course start
            if (DateTime.Now.AddDays(14) > course.StartDate)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "Enrollment must be at least 2 weeks before the course starts",
                    StatusCode = 400, // Bad Request
                };
            }

            // Check max participants
            if (course.Enrollments != null && course.Enrollments.Count >= course.MaxParticipants)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "Course has reached maximum capacity",
                    StatusCode = 409, // Conflict
                };
            }

            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == user.Id && e.CourseId == courseId);

            if (existingEnrollment != null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "User is already enrolled in this course",
                    StatusCode = 409, // Conflict status code
                };
            }

            var enrollment = new Enrollment
            {
                UserId = user.Id,
                CourseId = courseId,
                EnrollmentDate = DateTime.Now,
                Status = "Active"
            };

            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            return new GeneralServiceResponseDto
            {
                IsSuccess = true,
                Message = "Successfully Enrolled",
                StatusCode = 200,
            };
        }


    }
}
