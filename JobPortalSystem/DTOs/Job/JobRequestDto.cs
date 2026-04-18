using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.Api.DTOs.Job
{
    public class JobRequestDto
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public string Location { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "MinSalary must be >= 0")]
        public int Salary { get; set; }
    }
}
