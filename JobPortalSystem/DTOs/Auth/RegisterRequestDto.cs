using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.Api.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
    }
}
