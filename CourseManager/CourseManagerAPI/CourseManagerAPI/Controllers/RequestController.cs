using Azure.Core;
using CourseManagerAPI.Constants;
using CourseManagerAPI.DTOs.General;

using CourseManagerAPI.DTOs.Request;
using CourseManagerAPI.Entities;
using CourseManagerAPI.Interfaces;
using CourseManagerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CourseManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = StaticUserRoles.OWNERADMINLECTURER)]
        public async Task<ActionResult<GeneralServiceResponseDto>> CreateNewRequest([FromBody] CreateTeachingRequestDto teachingRequestDto)
        {
            var result = await _requestService.CreateNewTeachingRequestAsync(User, teachingRequestDto);
            return result;
        }

        //Get all requests for owner and Admin
        [HttpGet]
        [Route("activeRequests")]
        [Authorize(Roles = StaticUserRoles.OWNERADMIN)]
        public async Task<ActionResult<IEnumerable<GetTeachingRequestsDto>>> GetAllTeachingRequests()
        {
            var requests = await _requestService.GetAllTeachingRequestsAsync();
            return Ok(requests);
        }

        //Get all requests for owner and Admin
        [HttpGet]
        [Route("inactiveRequests")]
        [Authorize(Roles = StaticUserRoles.OWNERADMIN)]
        public async Task<ActionResult<IEnumerable<GetTeachingRequestsDto>>> GetAllInactiveRequests()
        {
            
            var requests = await _requestService.GetAllTeachingInActiveRequestsAsync();
            return Ok(requests);
        }

        //Get current user messages (If lecturer or admin)
        [HttpGet]
        [Route("myRequests")]
        [Authorize(Roles = StaticUserRoles.OWNERADMINLECTURER)]
        public async Task<ActionResult<IEnumerable<GetTeachingRequestsDto>>> GetMyTeachingRequests()
        {
            var requests = await _requestService.GetMyTeachingRequestsAsync(User);
            return Ok(requests);
        }

        [HttpGet]
        [Route("MyRejectedRequests")]
        [Authorize(Roles = StaticUserRoles.OWNERADMINLECTURER)]
        public async Task<ActionResult<IEnumerable<GetTeachingRequestsDto>>> GetMyRejectedTeachingRequests()
        {
            var requests = await _requestService.GetMyRejectedTeachingRequestsAsync(User);
            return Ok(requests);

        }
        [HttpGet]
        [Route("MyAcceptedRequests")]
        [Authorize(Roles = StaticUserRoles.OWNERADMINLECTURER)]
        public async Task<ActionResult<IEnumerable<GetTeachingRequestsDto>>> GetMyAcceptedTeachingRequests()
        {
            var requests = await _requestService.GetMyAcceptedTeachingRequestsAsync(User);
            return Ok(requests);
        }
        [HttpPut]
        [Route("AcceptRequest/{RequestId}")]
        [Authorize(Roles = StaticUserRoles.OWNERADMIN)]
        public async Task<IActionResult> AcceptTeacherRequest([FromRoute] int RequestId)
        {
            var result = await _requestService.AcceptTeacherRequestAsync(RequestId);

            if (result.IsSuccess)
            {
                Ok(result);
            }


            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpPut]
        [Route("RejectRequest/{RequestId}")]
        [Authorize(Roles = StaticUserRoles.OWNERADMIN)]
        public async Task<IActionResult> RejectTeacherRequest([FromRoute] int RequestId)
        {
            var result = await _requestService.RejectTeacherRequestAsync(RequestId);

            if (result.IsSuccess)
            {
                Ok(result);
            }


            return StatusCode(result.StatusCode, result.Message);
        }

    }
}
