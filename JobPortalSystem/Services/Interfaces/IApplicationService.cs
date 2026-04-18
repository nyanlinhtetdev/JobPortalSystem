using JobPortalSystem.Api.DTOs.Application;
using JobPortalSystem.Api.DTOs.Applications;

namespace JobPortalSystem.Api.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<JobApplicationDto?> Apply(Guid userId, ApplyJobDto request);
        Task<List<JobApplicationDto>> GetMyApplications(Guid userId);
        Task<List<ApplicantDto>?> GetApplicants(Guid employerId, Guid jobId);
        Task<bool?> UpdateStatus(Guid employerId, Guid applicationId, UpdateStatusDto request);
     }
}