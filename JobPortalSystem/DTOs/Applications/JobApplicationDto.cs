namespace JobPortalSystem.Api.DTOs.Applications
{
    public class JobApplicationDto
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string? Status { get; set; }
        public DateTime? AppliedAt { get; set; }
    }
}
