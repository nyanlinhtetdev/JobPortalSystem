using JobPortalSystem.Api.DTOs.ApiResponse;
using JobPortalSystem.Api.DTOs.Job;
using JobPortalSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Security.Claims;

namespace JobPortalSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService jobService;
        public JobsController(IJobService jobService)
        {
            this.jobService = jobService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] JobSearchRequestDto request)
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
            var result = await jobService.SearchJobs(request);
            return Ok(new ApiResponse<List<JobDto>>
            {
                Success = true,
                Message = "Results",
                Data = result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobPostById(Guid id)
        {
            var job = await jobService.GetJobPostById(id);
            if(job is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Job post with that ID does not exist."
                });
            }
            return Ok(new ApiResponse<JobDto>
            {
                Success = true,
                Message = "Job post was found.",
                Data = job
            });
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        public async Task<IActionResult> CreateJob(JobRequestDto request)
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "You are not authorized."
                });
            }
            var employerId = Guid.Parse(userId);
            var result = await jobService.CreateJob(request, employerId);

            return CreatedAtAction(nameof(GetJobPostById), new { id = result.Id }, new ApiResponse<JobDto>
            {
                Success = true,
                Message = "Job post was successfully created.",
                Data = result
            });
        }

        [Authorize(Roles = "Employer")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyJobs()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "You are not authorized."
                });
            }
            var employerId = Guid.Parse(userId);

            var jobList = await jobService.GetMyJobs(employerId);
            
            return Ok(new ApiResponse<List<JobDto>>
            {
                Success = true,
                Message = "Your Jobs",
                Data = jobList
            });
        }

        [Authorize(Roles = "Employer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(Guid id, JobRequestDto request)
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
            var result = await jobService.UpdateJob(id, request);
            if(result is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Job post with that ID does not exist."
                });
            }
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Job Post was successfully updated."
            });
        }

        [Authorize(Roles = "Employer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var result = await jobService.DeleteJob(id);
            if (result is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Job post with that ID does not exist."
                });
            }
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Job Post was successfully deleted."
            });
        }

    }
}
