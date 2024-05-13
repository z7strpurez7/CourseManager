using CourseManagerAPI.Constants;
using CourseManagerAPI.Context;
using CourseManagerAPI.DTOs.Course;
using CourseManagerAPI.DTOs.General;
using CourseManagerAPI.Entities;
using CourseManagerAPI.Interfaces;
using CourseManagerAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseManagerAPI.Services
{
    public class CourseService : ICourseService
    {
    
        private readonly ILogService _logService;
        private readonly courseDbContext _context;
        private readonly UserManager<UserEntity> _userManager;

        public CourseService(ILogService logService, courseDbContext context, UserManager<UserEntity> userManager)
        {
            _logService = logService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<GeneralServiceResponseDto> CreateCourseAsync(ClaimsPrincipal User, CreateCourseDto courseDto)
        {
            if(User.IsInRole(StaticUserRoles.ADMIN) || User.IsInRole(StaticUserRoles.OWNER))
            {
                var course = new CourseEntity()
                {
                    CourseName = courseDto.CourseName,
                    Description = courseDto.Description,
                    StartDate = courseDto.StartDate,
                    FinishDate = courseDto.FinishDate,
                    Address = courseDto.Address,
                    MaxParticipants = courseDto.MaxParticipants
                };

                
                await _context.Courses.AddAsync(course);
                await _context.SaveChangesAsync();
                await _logService.SaveNewLog(User.Identity.Name, "Created new course");


                return new GeneralServiceResponseDto
                {
                    IsSuccess = true,
                    Message = "Successfully published a course",
                    StatusCode = 201
                };
            } else
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "Access Denied: You dont have access",
                    StatusCode = 401
                };
            }
        }

        public  async Task<GetCoursesDto> GetCourseAsync(int courseId)
        {
            var course = _context.Courses
                      .Include(c => c.Enrollments).Include(c=> c.Teacher)
                      .FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                 return null;
            }
            return new GetCoursesDto
            {
                CourseName = course.CourseName,
                Description = course.Description,
                StartDate = course.StartDate,
                FinishDate = course.FinishDate,
                Address = course.Address,
                MaxParticipants = course.MaxParticipants,
                Enrollments = course.Enrollments == null ? 0 : course.Enrollments.Count,
                TeacherName = course.Teacher != null ? course.Teacher.FirstName + " " + course.Teacher.LastName : "Not Decided",
                CreatedAt = course.CreatedAt
            };
        }
        //burde passe på at antall folk ikke overstiger inne i api-en også
       /* public async Task<IActionResult> MyCourseEnrollments()
        {
            var course = await dbContext.Courses.FindAsync(courseId);
            var currentEnrollmentCount = course.Enrollments.Count();

            if (currentEnrollmentCount >= course.MaxParticipants)
            {
                // Handle the case where the course is full
                return View("Error", new ErrorViewModel { ErrorMessage = "Course is full." });
            }


            var userId = User.Identity.GetUserId();
            var enrollments = await dbContext.Enrollments
                                            .Where(e => e.UserID == userId)
                                            .Include(e => e.Course)
                                            .ToListAsync();

            return View(enrollments);


        } */

        public async Task<IEnumerable<GetCoursesDto>> GetCoursesAsync()
        {
                var courses = await _context.Courses.Include(e=> e.Enrollments).Select(e => new GetCoursesDto
                {
                    CourseName = e.CourseName,
                    Description = e.Description,
                    StartDate = e.StartDate,
                    FinishDate = e.FinishDate,
                    Address = e.Address,
                    MaxParticipants = e.MaxParticipants,
                    Enrollments = e.Enrollments == null ? 0 : e.Enrollments.Count,
                    TeacherName = e.Teacher != null? e.Teacher.FirstName + " " + e.Teacher.LastName: "Not Decided",
                    CreatedAt = e.CreatedAt
                }).OrderByDescending(e => e.CreatedAt).ToListAsync();
              
            return courses;
        }
        public async Task<IEnumerable<GetCoursesDto>> GetEnrolledCoursesAsync(ClaimsPrincipal user)
        {
            // Extract the user's identity name from the ClaimsPrincipal
            var userName = user.Identity?.Name;

            if (string.IsNullOrEmpty(userName))
            {
                // Handle the scenario where the user is not found or not authenticated
                throw new InvalidOperationException("User not found or not authenticated");
            }

            var currentUser = await _userManager.FindByNameAsync(userName);
            if (currentUser == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var enrolledCourses = await _context.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.User)
                .Where(c => c.Enrollments.Any(e => e.UserId == currentUser.Id))
                .Select(c => new GetCoursesDto
                {
                    CourseName = c.CourseName,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    FinishDate = c.FinishDate,
                    Address = c.Address,
                    MaxParticipants = c.MaxParticipants,
                    Enrollments = c.Enrollments.Count,
                    TeacherName = c.Teacher != null ? c.Teacher.FirstName + " " + c.Teacher.LastName : "Not Decided",
                    CreatedAt = c.CreatedAt
                })
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return enrolledCourses;
        }

        public async Task<IEnumerable<GetCourseTitlesDto>> GetCourseTitlesAsync()
        {
            var courses = await _context.Courses.Select(e => new GetCourseTitlesDto
            {
                CourseId = e.Id,
               CourseName= e.CourseName,
               startDate = e.StartDate,
           
            }).ToListAsync();

            return courses;
        }

        public async Task<GeneralServiceResponseDto> SetTeacherToCourseAsync(string teacherId, int courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            var Teacher = await _userManager.FindByIdAsync(teacherId);
            var userRoles = await _userManager.GetRolesAsync(Teacher);
            if (teacherId == null || Teacher == null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid id",
                    StatusCode = 404,
                };
            }
            if (course == null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "course does not exist",
                    StatusCode = 404,
                };
            }
            if (!userRoles.Contains(StaticUserRoles.LECTURER))
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "This User is not a Teacher",
                    StatusCode = 401,
                };
            }

            //check if Teacher is already set
            if (course.Teacher != null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "This course already has a teacher",
                    StatusCode = 409,
                };
            }

            course.Teacher = Teacher;
            course.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            await _logService.SaveNewLog(Teacher.UserName, "Selected as courseTeacher");

            return new GeneralServiceResponseDto
            {
                IsSuccess = true,
                Message = "Teacher has been added to Course",
                StatusCode = 200,
            };
        }
    }
}
