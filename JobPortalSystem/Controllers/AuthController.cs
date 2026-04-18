using JobPortalSystem.Api.DTOs.ApiResponse;
using JobPortalSystem.Api.DTOs.Auth;
using JobPortalSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortalSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
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
            List<string> roles = new List<string> { "Job Seeker", "Employer" };
            if(!roles.Any(role => request.Role == role))
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Role must be Job Seeker or Employer.",                  
                });               
            }
            var result = await _authService.Register(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
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

            var result = await _authService.Login(request);
            if(result is null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid email or password."                   
                });             
            }
            return Ok(new ApiResponse<TokenResponseDto>
            {
                Success = true,
                Message = "Successfully created access token and refresh token.",
                Data = result
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshToken(request);
            if(result is null)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid refresh token."
                });
            }
            return Ok(new ApiResponse<TokenResponseDto>
            {
                Success = true,
                Message = "Successfully created access token and refresh token.",
                Data = result
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult OnlyAuthenticatedUser()
        {
            return Ok("You are authenticated.");
        }
    }
}
