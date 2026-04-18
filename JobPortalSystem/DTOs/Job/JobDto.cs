namespace JobPortalSystem.Api.DTOs.Job
{
    public class JobDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? Location { get; set; }

        public int? Salary { get; set; }
    }
}
