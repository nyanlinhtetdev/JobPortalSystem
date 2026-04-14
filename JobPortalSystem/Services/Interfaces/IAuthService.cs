using JobPortalSystem.Api.DTOs.Auth;

namespace JobPortalSystem.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterRequestDto request);
        Task<TokenResponseDto?> Login(LoginRequestDto request);
        Task<TokenResponseDto?> RefreshToken(RefreshTokenRequestDto request);

    }
}