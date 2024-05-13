using CourseManagerAPI.DTOs.Course;
using CourseManagerAPI.DTOs.General;
using CourseManagerAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseManagerAPI.Interfaces
{
    public interface ICourseService
    {
        Task<GeneralServiceResponseDto> CreateCourseAsync(ClaimsPrincipal User, CreateCourseDto createCourseDto);
        Task<IEnumerable<GetCoursesDto>> GetEnrolledCoursesAsync(ClaimsPrincipal user);
        Task<IEnumerable<GetCoursesDto>> GetCoursesAsync();
        Task<IEnumerable<GetCourseTitlesDto>> GetCourseTitlesAsync();
        Task<GetCoursesDto> GetCourseAsync(int courseId);
        Task<GeneralServiceResponseDto> SetTeacherToCourseAsync(string teacherId, int courseId);
    }
}
