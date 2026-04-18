using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.Applications;

namespace JobPortalSystem.Api.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<bool> ApplicationExists(Guid jobId, Guid userId);
        Task CreateApplication(JobApplication application);
        Task<List<JobApplicationDto>> GetApplicationsByUserId(Guid userId);
        Task<bool> JobExists(Guid jobId);
    }
}