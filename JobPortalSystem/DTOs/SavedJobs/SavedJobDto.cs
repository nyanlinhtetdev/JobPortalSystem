namespace JobPortalSystem.Api.DTOs.SavedJobs
{
    public class SavedJobDto
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string Title { get; set; } = null!;
        public string? Location { get; set; }
        public int? Salary { get; set; }
    }
}
