namespace JobPortalSystem.Api.DTOs.Applications
{
    public class ApplicantDto
    {
        public Guid ApplicationId { get; set; }
        public Guid JobId { get; set; }

        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;

        public string? Status { get; set; }
        public DateTime? AppliedAt { get; set; }
    }
}