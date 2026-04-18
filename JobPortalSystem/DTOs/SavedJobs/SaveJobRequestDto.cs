using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.Api.DTOs.SavedJobs
{
    public class SaveJobRequestDto
    {
        [Required]
        public Guid JobId { get; set; }
    }
}
