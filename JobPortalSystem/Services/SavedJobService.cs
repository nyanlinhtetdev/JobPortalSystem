using JobPortal.Database.AppDbContextModels;
using JobPortalSystem.Api.DTOs.SavedJobs;
using JobPortalSystem.Api.Repositories.Interfaces;
using JobPortalSystem.Api.Services.Interfaces;

namespace JobPortalSystem.Api.Services
{
    public class SavedJobService : ISavedJobService
    {
        private readonly ISavedJobRepository _savedJobRepository;

        public SavedJobService(ISavedJobRepository savedJobRepository)
        {
            _savedJobRepository = savedJobRepository;
        }

        public async Task<bool?> SaveJob(Guid userId, SaveJobRequestDto request)
        {
            if (!await _savedJobRepository.JobExists(request.JobId))
            {
                return null;
            }

            if (await _savedJobRepository.SavedJobExists(userId, request.JobId))
            {
                return false;
            }

            var savedJob = new SavedJob
            {
                UserId = userId,
                JobId = request.JobId
                // CreatedAt is DB default (getutcdate())
            };

            await _savedJobRepository.CreateSavedJob(savedJob);
            return true;
        }

        public async Task<List<SavedJobDto>> GetSavedJobs(Guid userId)
        {
            return await _savedJobRepository.GetSavedJobs(userId);
        }
    }
}