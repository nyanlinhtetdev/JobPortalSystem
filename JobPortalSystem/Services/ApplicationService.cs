using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.Application;
using JobPortalSystem.Api.DTOs.Applications;
using JobPortalSystem.Api.Repositories.Interfaces;
using JobPortalSystem.Api.Services.Interfaces;

namespace JobPortalSystem.Api.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
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
    }
}
