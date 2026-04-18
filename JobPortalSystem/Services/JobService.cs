using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.Job;
using JobPortalSystem.Api.Repositories.Interfaces;
using JobPortalSystem.Api.Services.Interfaces;
using System.Security.Claims;

namespace JobPortalSystem.Api.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<JobDto> CreateJob(JobRequestDto request, Guid employerId)
        {
            var job = new Job
            {
                Title = request.Title,
                Description = request.Description,
                Salary = request.Salary,
                Location = request.Location,
                EmployerId = employerId
            };
            await _jobRepository.CreateJob(job);

            return new JobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Salary = job.Salary,
                Location = job.Location
            };
        }

        public async Task<List<JobDto>> SearchJobs(JobSearchRequestDto request)
        {
            return await _jobRepository.SearchJobs(request);
        }
        public async Task<JobDto?> GetJobPostById(Guid id)
        {
            var job = await _jobRepository.GetJobPostById(id);
            if(job is null)
            {
                return null;
            }
            return new JobDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Salary = job.Salary,
                Location = job.Location
            };
        }
        public async Task<List<JobDto>> GetMyJobs(Guid id)
        {
            return await _jobRepository.GetJobs(id);
        }
        public async Task<int?> UpdateJob(Guid id, JobRequestDto request)
        {
            return await _jobRepository.UpdateJob(id, request);
        }
        public async Task<int?> DeleteJob(Guid id)
        {
            return await _jobRepository.DeleteJob(id);
        }

    }
}
