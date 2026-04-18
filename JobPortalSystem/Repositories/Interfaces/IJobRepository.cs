using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.Job;

namespace JobPortalSystem.Api.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task CreateJob(Job request);
        Task<List<JobDto>> SearchJobs(JobSearchRequestDto request);
        Task<Job?> GetJobPostById(Guid id);
        Task<List<JobDto>> GetJobs(Guid id);
        Task<int?> UpdateJob(Guid id, JobRequestDto request);
        Task<int?> DeleteJob(Guid id);
    }
}