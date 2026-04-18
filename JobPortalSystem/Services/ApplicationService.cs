using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.Application;
using JobPortalSystem.Api.DTOs.Applications;
using JobPortalSystem.Api.Repositories;
using JobPortalSystem.Api.Repositories.Interfaces;
using JobPortalSystem.Api.Services.Interfaces;

namespace JobPortalSystem.Api.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobRepository _jobRepository;
        public ApplicationService(IApplicationRepository applicationRepository, IJobRepository jobRepository)
        {
            _applicationRepository = applicationRepository;
            _jobRepository = jobRepository;
        }

        public async Task<JobApplicationDto?> Apply(Guid userId, ApplyJobDto request)
        {
            if (!await _applicationRepository.JobExists(request.JobId))
            {
                return null;
            }

            if (await _applicationRepository.ApplicationExists(request.JobId, userId))
            {
                return null;
            }

            var application = new JobApplication
            {
                JobId = request.JobId,
                UserId = userId            
            };

            await _applicationRepository.CreateApplication(application);

            return new JobApplicationDto
            {
                Id = application.Id,
                JobId = application.JobId,
                Status = application.Status,
                AppliedAt = application.AppliedAt
            };
        }

        public async Task<List<JobApplicationDto>> GetMyApplications(Guid userId)
        {
            return await _applicationRepository.GetApplicationsByUserId(userId);         
        }

        public async Task<List<ApplicantDto>?> GetApplicants(Guid employerId, Guid jobId)
        {
            var job = await _jobRepository.GetJobPostById(jobId);
            if (job is null || job.IsDeleted)
            {
                return null;
            }

            if (job.EmployerId != employerId)
            {
                return null;
            }

            return await _applicationRepository.GetApplicantsForJob(jobId);
        }
        public async Task<bool?> UpdateStatus(Guid employerId, Guid applicationId, UpdateStatusDto request)
        {
            var application = await _applicationRepository.GetApplicationById(applicationId);
            if (application is null)
            {
                return null;
            }

            var job = await _jobRepository.GetJobPostById(application.JobId);
            if (job is null || job.IsDeleted)
            {
                return null;
            }

            if (job.EmployerId != employerId)
            {
                return null;
            }

            application.Status = request.Status;

            var changed = await _applicationRepository.SaveChangesAsync();
            return changed > 0;
        }
    }
}
