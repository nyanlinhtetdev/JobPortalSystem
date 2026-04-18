using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.Api.DTOs.Job
{
    public class JobSearchRequestDto
    {
        public string? Title { get; set; }    
        public string? Location { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "MinSalary must be >= 0")]
        public int MinimumSalary { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Page must be >= 0")]
        public int Page { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Page Size must be >= 0")]
        public int PageSize { get; set; }
    }
}
