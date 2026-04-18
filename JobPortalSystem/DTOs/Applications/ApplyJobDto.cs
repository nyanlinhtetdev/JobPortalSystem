using System.ComponentModel.DataAnnotations;

namespace JobPortalSystem.Api.DTOs.Application
{
    public class ApplyJobDto
    {
        [Required]
        public Guid JobId { get; set; }
    }
}
