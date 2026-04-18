using JobPortalSystem.Api.DTOs.Job;

namespace JobPortalSystem.Api.Services.Interfaces
{
    public interface IJobService
    {
        Task<JobDto> CreateJob(JobRequestDto request, Guid employerId);
        Task<JobDto?> GetJobPostById(Guid id);
        Task<List<JobDto>> GetMyJobs(Guid id);
        Task<int?> UpdateJob(Guid id, JobRequestDto request);
        Task<int?> DeleteJob(Guid id);
        Task<List<JobDto>> SearchJobs(JobSearchRequestDto request);
    }
}