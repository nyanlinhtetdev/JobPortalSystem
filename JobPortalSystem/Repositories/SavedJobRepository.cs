using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.SavedJobs;
using JobPortalSystem.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobPortalSystem.Api.Repositories
{
    public class SavedJobRepository : ISavedJobRepository
    {
        private readonly AppDbContext _context;

        public SavedJobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> JobExists(Guid jobId)
        {
            return await _context.Jobs.AnyAsync(j => j.Id == jobId && j.IsDeleted == false);
        }

        public async Task<bool> SavedJobExists(Guid userId, Guid jobId)
        {
            return await _context.SavedJobs.AnyAsync(s => s.UserId == userId && s.JobId == jobId);
        }

        public async Task CreateSavedJob(SavedJob savedJob)
        {
            await _context.SavedJobs.AddAsync(savedJob);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SavedJobDto>> GetSavedJobs(Guid userId)
        {
            return await _context.SavedJobs
                .Where(s => s.UserId == userId && s.Job.IsDeleted == false)
                .OrderByDescending(s => s.CreatedAt)
                .AsNoTracking()
                .Select(s => new SavedJobDto
                {
                    Id = s.Id,
                    JobId = s.JobId,
                    CreatedAt = s.CreatedAt,
                    Title = s.Job.Title,
                    Location = s.Job.Location,
                    Salary = s.Job.Salary
                })
                .ToListAsync();
        }
    }
}