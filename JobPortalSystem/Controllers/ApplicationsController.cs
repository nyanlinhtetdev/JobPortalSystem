using JobPortalSystem.Api.DTOs.ApiResponse;
using JobPortalSystem.Api.DTOs.Application;
using JobPortalSystem.Api.DTOs.Applications;
using JobPortalSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobPortalSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplyJobDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Errors = ModelState
                });
            }

            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdValue is null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "You are not authorized."
                });
            }

            var userId = Guid.Parse(userIdValue);

            var result = await _applicationService.Apply(userId, request);
            if (result is null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Cannot apply to this job (job not found or already applied)."
                });
            }

            return Ok(new ApiResponse<JobApplicationDto>
            {
                Success = true,
                Message = "Successfully applied.",
                Data = result
            });
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyApplications()
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdValue is null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "You are not authorized."
                });
            }

            var userId = Guid.Parse(userIdValue);

            var result = await _applicationService.GetMyApplications(userId);

            return Ok(new ApiResponse<List<JobApplicationDto>>
            {
                Success = true,
                Message = "Your applications.",
                Data = result
            });
        }
    }
}