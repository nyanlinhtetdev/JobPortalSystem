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
                return BadRequest(ModelState);
            }
            List<string> roles = new List<string> { "Job Seeker", "Employer" };
            if(!roles.Any(role => request.Role == role))
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Role must be Job Seeker or Employer."
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
                return BadRequest(ModelState);
            }

            var result = await _authService.Login(request);
            if(result is null)
            {
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                });
            }
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshToken(request);
            if(result is null)
            {
                return Unauthorized("Invalid refresh token.");
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult OnlyAuthenticatedUser()
        {
            return Ok("You are authenticated.");
        }
    }
}
