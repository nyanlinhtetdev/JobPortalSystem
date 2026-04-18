using JobPortalSystem.Api.DTOs.ApiResponse;
using JobPortalSystem.Api.DTOs.SavedJobs;
using JobPortalSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobPortalSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavedJobsController : ControllerBase
    {
        private readonly ISavedJobService _savedJobService;

        public SavedJobsController(ISavedJobService savedJobService)
        {
            _savedJobService = savedJobService;
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpPost]
        public async Task<IActionResult> SaveJob(SaveJobRequestDto request)
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

            var result = await _savedJobService.SaveJob(userId, request);

            if (result is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Job not found."
                });
            }

            if (result == false)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Job already saved."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Job saved."
            });
        }

        [Authorize(Roles = "Job Seeker")]
        [HttpGet]
        public async Task<IActionResult> GetSavedJobs()
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

            var result = await _savedJobService.GetSavedJobs(userId);

            return Ok(new ApiResponse<List<SavedJobDto>>
            {
                Success = true,
                Message = "Saved jobs.",
                Data = result
            });
        }
    }
}