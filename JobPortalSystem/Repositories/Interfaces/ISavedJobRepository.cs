using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.SavedJobs;

namespace JobPortalSystem.Api.Repositories.Interfaces
{
    public interface ISavedJobRepository
    {
        Task CreateSavedJob(SavedJob savedJob);
        Task<List<SavedJobDto>> GetSavedJobs(Guid userId);
        Task<bool> JobExists(Guid jobId);
        Task<bool> SavedJobExists(Guid userId, Guid jobId);
    }
}