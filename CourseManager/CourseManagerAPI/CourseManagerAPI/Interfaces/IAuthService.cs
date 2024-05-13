using CourseManagerAPI.DTOs.Auth;
using CourseManagerAPI.DTOs.Course;
using CourseManagerAPI.DTOs.General;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseManagerAPI.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralServiceResponseDto> SeedRolesAsync();
        Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto);
        Task<GeneralServiceResponseDto> UpdateRoleAsync(ClaimsPrincipal User, UpdateRoleDto updateRoleDto);
        Task<LoginServiceResponseDto?> MeAsync(MeDto meDto);
   
        Task<IEnumerable<UserInfoResult>> GetUserListAsync();
        Task<IEnumerable<TeacherNameDto>> GetTeacherNamesListAsync();
        Task<UserInfoResult> GetUserDetailsByUserNameAsync(string UserName);
        
    }
}
