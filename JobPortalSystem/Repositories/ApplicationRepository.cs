using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.Applications;
using JobPortalSystem.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Api.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> JobExists(Guid jobId)
        {
            return await _context.Jobs.AnyAsync(j => j.Id == jobId && j.IsDeleted == false);
        }

        public async Task<bool> ApplicationExists(Guid jobId, Guid userId)
        {
            return await _context.JobApplications.AnyAsync(a => a.JobId == jobId && a.UserId == userId);
        }

        public async Task CreateApplication(JobApplication application)
        {
            await _context.JobApplications.AddAsync(application);
            await _context.SaveChangesAsync();
        }

        public async Task<List<JobApplicationDto>> GetApplicationsByUserId(Guid userId)
        {
            return await _context.JobApplications
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppliedAt)
                .AsNoTracking()
                .Select(a => new JobApplicationDto
                {
                    Id = a.Id,
                    JobId = a.JobId,
                    Status = a.Status,
                    AppliedAt = a.AppliedAt
                })
                .ToListAsync();
        }
        public async Task<List<ApplicantDto>> GetApplicantsForJob(Guid jobId)
        {
            return await _context.JobApplications
                .Where(a => a.JobId == jobId)
                .OrderByDescending(a => a.AppliedAt)
                .AsNoTracking()
                .Select(a => new ApplicantDto
                {
                    ApplicationId = a.Id,
                    JobId = a.JobId,
                    UserId = a.UserId,
                    Email = a.User.Email,
                    Status = a.Status,
                    AppliedAt = a.AppliedAt
                })
                .ToListAsync();
        }
        public async Task<JobApplication?> GetApplicationById(Guid applicationId)
        {
            return await _context.JobApplications.FirstOrDefaultAsync(a => a.Id == applicationId);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}