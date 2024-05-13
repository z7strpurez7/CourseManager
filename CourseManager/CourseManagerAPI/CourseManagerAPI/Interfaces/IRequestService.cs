using CourseManagerAPI.DTOs.General;

using CourseManagerAPI.DTOs.Request;
using System.Security.Claims;

namespace CourseManagerAPI.Interfaces
{
    public interface IRequestService
    {
        Task<GeneralServiceResponseDto> CreateNewTeachingRequestAsync(ClaimsPrincipal User, CreateTeachingRequestDto teacherRequestDto);
        Task<IEnumerable<GetTeachingRequestsDto>> GetAllTeachingRequestsAsync();
        Task<IEnumerable<GetTeachingRequestsDto>> GetAllTeachingInActiveRequestsAsync();
        Task<IEnumerable<GetTeachingRequestsDto>> GetMyTeachingRequestsAsync(ClaimsPrincipal User);
        Task<IEnumerable<GetTeachingRequestsDto>> GetMyRejectedTeachingRequestsAsync(ClaimsPrincipal User);
        Task<IEnumerable<GetTeachingRequestsDto>> GetMyAcceptedTeachingRequestsAsync(ClaimsPrincipal User);
        Task<GeneralServiceResponseDto> AcceptTeacherRequestAsync(int RequestId);
        Task<GeneralServiceResponseDto> RejectTeacherRequestAsync(int RequestId);

    }
}
