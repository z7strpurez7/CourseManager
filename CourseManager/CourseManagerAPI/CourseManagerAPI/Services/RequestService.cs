using CourseManagerAPI.Constants;
using CourseManagerAPI.Context;
using CourseManagerAPI.DTOs.General;
using CourseManagerAPI.DTOs.Request;
using CourseManagerAPI.Entities;
using CourseManagerAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseManagerAPI.Services
{
    public class RequestService : IRequestService
    {
        private readonly courseDbContext _context;
        private readonly ILogService _logService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly ICourseService _courseService;

        public RequestService(courseDbContext context, ILogService logService, UserManager<UserEntity> userManager, ICourseService courseService)
        {
            _context = context;
            _logService = logService;
            _userManager = userManager;
            _courseService = courseService;
        }

        public async Task<GeneralServiceResponseDto> CreateNewTeachingRequestAsync(ClaimsPrincipal User, CreateTeachingRequestDto teachingRequestDto)
        {
            var user = await _userManager.GetUserAsync(User);
            var request = await _context.Courses.FindAsync(teachingRequestDto.CourseId);
            
            if (request == null) {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "Course not found",
                    StatusCode = 404
                };
            }
            if (user == null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "Please log in first",
                    StatusCode = 401
                };
            }
            
            //Sjekker om bruker har allerede sendt en teachingRequest
            bool requestExists = await _context.TeachingRequests.AnyAsync(e =>
            e.TeacherId == user.Id &&
            e.CourseId == teachingRequestDto.CourseId);


            if (requestExists)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    Message = "Teaching request already exists for this course and teacher.",
                    StatusCode = 400 // Or any other appropriate status code
                };
            }


            TeachingRequest newRequest = new TeachingRequest()
            {
                TeacherName = user.FirstName + " " + user.LastName,
                TeacherId = user.Id,
                CourseId = teachingRequestDto.CourseId,
               

            };

            await _context.TeachingRequests.AddAsync(newRequest);
            await _context.SaveChangesAsync();
            await _logService.SaveNewLog(User.Identity.Name, "Sent TeachingRequest");

            return new GeneralServiceResponseDto()
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "Request created successfully"
            };
        }

        public async Task<IEnumerable<GetTeachingRequestsDto>> GetAllTeachingRequestsAsync()
        {
            var requests = await _context.TeachingRequests
                .Include(r => r.Course) 
                .Include(r => r.Teacher) 
                .Where(r => r.isActive)
                .Select(r => new GetTeachingRequestsDto()
                {
                    Id = r.Id,
                    SenderUserName = r.Teacher.UserName,
                    CourseName = r.Course.CourseName,
                    CourseId = r.CourseId,
                    RequestStatus = r.RequestStatus,
                    CreatedAt = r.CreatedAt
                }).OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return requests;
        }
        public async Task<IEnumerable<GetTeachingRequestsDto>> GetAllTeachingInActiveRequestsAsync()
        {
            var requests = await _context.TeachingRequests
                .Include(r => r.Course) // Ensure you have a navigation property to Course
                .Include(r => r.Teacher) // Ensure you have a navigation property to Teacher
                .Where(e => !e.isActive)
                .Select(e => new GetTeachingRequestsDto()
                {
                    Id = e.Id,
                    SenderUserName = e.Teacher.UserName, // Assuming you have a Teacher entity
                    CourseName = e.Course.CourseName,    // Assuming you have a Course entity
                    CourseId = e.CourseId,
                    RequestStatus = e.RequestStatus,
                    CreatedAt = e.CreatedAt
                }).OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
            return requests;
        }


        //Lærer kan se sine meldinger
        public async Task<IEnumerable<GetTeachingRequestsDto>> GetMyTeachingRequestsAsync(ClaimsPrincipal User)
        {
            //UserName er unik

            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return null;
            }

            var requests = await _context.TeachingRequests.Where(e => e.TeacherId == user.Id && e.isActive).Select(e => new GetTeachingRequestsDto
            {
                Id = e.Id,
                SenderUserName = e.TeacherName,
                CourseId = e.CourseId,
                RequestStatus = e.RequestStatus,
                CreatedAt = e.CreatedAt
            }).OrderByDescending(e => e.CreatedAt).ToListAsync();
            return requests;
        }
        public async Task<IEnumerable<GetTeachingRequestsDto>> GetMyRejectedTeachingRequestsAsync(ClaimsPrincipal User)
        {
            //UserName er unik
            var user = await _userManager.GetUserAsync(User);

            var requests = await _context.TeachingRequests
                .Where(e => e.TeacherId == user.Id && e.RequestStatus == StaticRequestStatus.REJECTED)
                .Select(e => new GetTeachingRequestsDto
                {
                    Id = e.Id,
                    SenderUserName = e.TeacherName,
                    CourseId = e.CourseId,
                    RequestStatus = e.RequestStatus,
                    CreatedAt = e.CreatedAt
                }).OrderByDescending(e => e.CreatedAt).ToListAsync();
            return requests;
        }
        public async Task<IEnumerable<GetTeachingRequestsDto>> GetMyAcceptedTeachingRequestsAsync(ClaimsPrincipal User)
        {
            var user = await _userManager.GetUserAsync(User);

            var requests = await _context.TeachingRequests
                .Where(e => e.TeacherId == user.Id && e.RequestStatus == StaticRequestStatus.ACCEPTED)
                .Select(e => new GetTeachingRequestsDto
                {
                    Id = e.Id,
                    SenderUserName = e.TeacherName,
                    CourseId = e.CourseId,
                    RequestStatus = e.RequestStatus,
                    CreatedAt = e.CreatedAt
                }).OrderByDescending(e => e.CreatedAt).ToListAsync();
            return requests;
        }

        public async Task<GeneralServiceResponseDto> AcceptTeacherRequestAsync(int RequestId)
        {
            var request = await _context.TeachingRequests.FindAsync(RequestId);
            if (request == null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Request with id not found"
                };
            }
            //Bruker course service til å sette Teacher til course
            var SetTeacherToCourseResult = await _courseService.SetTeacherToCourseAsync(request.TeacherId, request.CourseId);
           //sender CourseService feilkode hvis den ikke gikk gjennom
            if(!SetTeacherToCourseResult.IsSuccess)
            {
                return SetTeacherToCourseResult;
            }
         
            request.RequestStatus = StaticRequestStatus.ACCEPTED;
            request.isActive = false;
            await _logService.SaveNewLog(request.TeacherName, "TeacherRequest denied");
            await _context.SaveChangesAsync();

            return new GeneralServiceResponseDto
            {
                IsSuccess = true,
                StatusCode = 202,
                Message = "Request status successfully accepted"
            };

        }
        public async Task<GeneralServiceResponseDto> RejectTeacherRequestAsync(int RequestId)
        {
            var request = await _context.TeachingRequests.FindAsync(RequestId);
            if (request == null)
            {
                return new GeneralServiceResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Request with id not found"
                };
            } else { 
            }

            request.RequestStatus = StaticRequestStatus.REJECTED;
            request.isActive =false;
            await _logService.SaveNewLog(request.TeacherName, "TeacherRequest Accepted");
            await _context.SaveChangesAsync();

            return new GeneralServiceResponseDto
            {
                IsSuccess = true,
                StatusCode = 202,
                Message = "Request status successfully rejected"
            };
        }



    }
}

