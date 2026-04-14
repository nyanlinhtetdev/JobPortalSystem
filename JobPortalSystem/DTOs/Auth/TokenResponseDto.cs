namespace JobPortalSystem.Api.DTOs.Auth
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
