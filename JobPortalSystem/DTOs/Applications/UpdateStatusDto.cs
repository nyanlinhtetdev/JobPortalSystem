using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.Api.DTOs.Applications
{
    public class UpdateStatusDto
    {
        [Required]
        [RegularExpression("^(Accept|Reject)$", ErrorMessage = "Status must be Accept or Reject.")]
        public string Status { get; set; } = null!;
    }
}