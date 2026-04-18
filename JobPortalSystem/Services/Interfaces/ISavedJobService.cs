using JobPortalSystem.Api.DTOs.SavedJobs;

namespace JobPortalSystem.Api.Services.Interfaces
{
    public interface ISavedJobService
    {
        Task<List<SavedJobDto>> GetSavedJobs(Guid userId);
        Task<bool?> SaveJob(Guid userId, SaveJobRequestDto request);
    }
}