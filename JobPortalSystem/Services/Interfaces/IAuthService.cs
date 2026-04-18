using JobPortalSystem.Api.DTOs.ApiResponse;
using JobPortalSystem.Api.DTOs.Auth;

namespace JobPortalSystem.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<object>> Register(RegisterRequestDto request);
        Task<TokenResponseDto?> Login(LoginRequestDto request);
        Task<TokenResponseDto?> RefreshToken(RefreshTokenRequestDto request);

    }
}